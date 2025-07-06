using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using static keyupMusic2.Native;
using SearchOption = System.IO.SearchOption; // 需要添加引用

namespace keyupMusic2
{
    public partial class Common
    {
        private static string wallpapersPath = Path.Combine(
        Directory.GetCurrentDirectory(), "image", "downloaded_images");
        private static string stateFilePath = Path.Combine(
            Directory.GetCurrentDirectory(), "log", "wallpaper_state.json");
        private static string smallPath = Path.Combine(
            Directory.GetCurrentDirectory(), "image", "downloaded_images", "3small");

        // 缓存目录结构和文件信息
        private static Dictionary<string, List<string>> idToFilesMap = new Dictionary<string, List<string>>();
        private static List<string> allIds = new List<string>();
        private static int currentIdIndex = -1;
        private static int currentFileIndex = -1;

        public static void OpenDir(string id)
        {
            if (id.Length != 5) return;
            string idPath = Path.Combine(wallpapersPath, id);
            //if (Directory.Exists(idPath)) { Directory.Delete(idPath, recursive: true); }
            Process.Start("explorer.exe", idPath);
        }
        public static void DeleteDir(string id)
        {
            if (id.Length != 5) return;
            string idPath = Path.Combine(wallpapersPath, id);
            //if (Directory.Exists(idPath)) { Directory.Delete(idPath, recursive: true); }

            try
            {
                FileSystem.DeleteDirectory(
                    idPath,
                    UIOption.OnlyErrorDialogs,
                    RecycleOption.SendToRecycleBin
                );
            }
            catch (Exception e)
            {
                MessageBox.Show("can't delete");
            }
        }

        public static int min_size = 40;
        static int iii = 0;
        public static void MoveSmallFilesRecursive(string sourceDir = "", string destDir = "")
        {
            if ((iii++ > 1000)) { MessageBox.Show("MoveSmallFilesRecursive over 1000" + sourceDir + destDir); return; }
            if (string.IsNullOrEmpty(sourceDir))
                sourceDir = wallpapersPath;
            if (string.IsNullOrEmpty(destDir))
                destDir = smallPath;
            if (sourceDir.Contains("small")) return;
            // 创建目标目录（如果不存在）
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            try
            {
                // 处理当前目录下的文件
                foreach (string filePath in Directory.GetFiles(sourceDir))
                {
                    FileInfo fileInfo = new FileInfo(filePath);

                    // 检查文件大小（100KB = 100 * 1024 字节）
                    if (fileInfo.Length < min_size * 1024)
                    {
                        string destFilePath = Path.Combine(destDir, fileInfo.Name);

                        // 处理同名文件（添加数字后缀）
                        if (File.Exists(destFilePath))
                        {
                            destFilePath = GetUniqueFilePath(destDir, fileInfo.Name);
                        }

                        // 移动文件
                        fileInfo.MoveTo(destFilePath);
                        Console.WriteLine($"已移动: {filePath} -> {destFilePath}");
                    }
                }

                // 递归处理子目录
                foreach (string subDir in Directory.GetDirectories(sourceDir))
                {
                    string subDirName = Path.GetFileName(subDir);
                    //string destSubDir = Path.Combine(destDir, subDirName);

                    MoveSmallFilesRecursive(subDir, destDir);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"警告: 访问被拒绝 - {sourceDir}");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"警告: 目录不存在 - {sourceDir}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理目录 {sourceDir} 时出错: {ex.Message}");
            }
        }

        // 生成唯一文件名（处理同名冲突）
        static string GetUniqueFilePath(string directory, string originalName)
        {
            string fileName = Path.GetFileNameWithoutExtension(originalName);
            string fileExt = Path.GetExtension(originalName);
            int counter = 1;

            string newPath;
            do
            {
                newPath = Path.Combine(directory, $"{fileName}({counter}){fileExt}");
                counter++;
            } while (File.Exists(newPath));

            return newPath;
        }
        public static List<string> GetAllFiles(string directoryPath, bool includeSubdirectories = true)
        {
            try
            {
                // 检查目录是否存在
                if (!Directory.Exists(directoryPath))
                {
                    Console.WriteLine($"错误：目录 '{directoryPath}' 不存在");
                    return new List<string>();
                }

                // 获取文件的搜索选项（是否包含子目录）
                SearchOption searchOption = includeSubdirectories
                    ? SearchOption.AllDirectories
                    : SearchOption.TopDirectoryOnly;

                // 获取所有文件的完整路径
                string[] filePaths = Directory.GetFiles(directoryPath, "*", searchOption);

                // 提取文件名（不包含路径）
                List<string> fileNames = new List<string>();
                foreach (string filePath in filePaths)
                {
                    fileNames.Add(Path.GetFileName(filePath));
                }

                return fileNames;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取文件时发生错误: {ex.Message}");
                return new List<string>();
            }
        }
        // 初始化壁纸管理器（异步加载目录结构）
        public static async Task InitializeFromCurrentWallpaper()
        {
            if (!Directory.Exists(wallpapersPath))
            {
                Directory.CreateDirectory(wallpapersPath);
                return;
            }

            // 并行加载状态和扫描目录
            var loadStateTask = Task.Run(() => LoadState());
            var scanDirTask = Task.Run(() => ScanDirectories());

            await Task.WhenAll(loadStateTask, scanDirTask);

            // 验证当前状态
            ValidateCurrentState();
        }

        // 扫描目录并缓存文件信息
        private static void ScanDirectories()
        {
            try
            {
                // 获取所有ID目录（按名称排序）
                allIds = Directory.GetDirectories(wallpapersPath)
                    .Where(d => !d.Contains("small"))
                    .Select(d => Path.GetFileName(d))
                    .OrderBy(d => d)
                    .ToList();

                // 并行扫描每个ID目录下的图片文件
                Parallel.ForEach(allIds, id =>
                {
                    string idPath = Path.Combine(wallpapersPath, id);
                    var imageFiles = GetImageFiles(idPath);

                    lock (idToFilesMap)
                    {
                        idToFilesMap[id] = imageFiles;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"扫描目录时出错: {ex.Message}");
            }
        }

        // 获取下一张壁纸（O(1) 时间复杂度）
        public static string GetNextWallpaper()
        {
            if (allIds.Count == 0) return null;

            // 如果是首次调用，初始化当前位置
            if (currentIdIndex == -1 || currentFileIndex == -1)
            {
                ResetToFirstFile();
                return GetCurrentWallpaperPath();
            }

            // 尝试获取当前ID中的下一个文件
            if (currentFileIndex < idToFilesMap[allIds[currentIdIndex]].Count - 1)
            {
                var currentPath = GetWallpaperFromRegistry();
                if (currentPath.Contains(Common.keyupMusic))
                    currentFileIndex++;
            }
            else
            {
                // 当前ID中的文件已处理完，移动到下一个ID
                if (currentIdIndex < allIds.Count - 1)
                {
                    currentIdIndex++;
                    currentFileIndex = 0;
                }
                else
                {
                    // 所有ID已处理完，回到第一个
                    ResetToFirstFile();
                }
            }

            SaveState();
            return GetCurrentWallpaperPath();
        }

        // 获取上一张壁纸（O(1) 时间复杂度）
        public static string GetPreviousWallpaper()
        {
            if (allIds.Count == 0) return null;

            // 如果是首次调用，初始化到最后一个文件
            if (currentIdIndex == -1 || currentFileIndex == -1)
            {
                GoToLastFile();
                return GetCurrentWallpaperPath();
            }

            // 尝试获取当前ID中的上一个文件
            if (currentFileIndex > 0)
            {
                currentFileIndex--;
            }
            else
            {
                // 当前ID中的第一个文件，移动到上一个ID的最后一个文件
                if (currentIdIndex > 0)
                {
                    currentIdIndex--;
                    currentFileIndex = idToFilesMap[allIds[currentIdIndex]].Count - 1;
                }
                else
                {
                    // 已经是第一个文件，移动到最后一个文件
                    GoToLastFile();
                }
            }

            SaveState();
            return GetCurrentWallpaperPath();
        }

        // 获取下一个ID文件夹的第一张壁纸（O(1) 时间复杂度）
        public static string GetNextIdFolder()
        {
            if (allIds.Count == 0) return null;

            // 如果是首次调用，初始化当前位置
            if (currentIdIndex == -1)
            {
                ResetToFirstFile();
                return GetCurrentWallpaperPath();
            }

            // 移动到下一个ID
            //currentIdIndex = (currentIdIndex + 1) % allIds.Count;
            lastId = currentIdIndex;
            currentIdIndex = new Random().Next(0, allIds.Count);
            currentFileIndex = 0;

            // 跳过空的ID文件夹
            while (idToFilesMap[allIds[currentIdIndex]].Count == 0)
            {
                currentIdIndex = (currentIdIndex + 1) % allIds.Count;
            }

            SaveState();
            return GetCurrentWallpaperPath();
        }
        static int lastId = 0;

        // 获取上一个ID文件夹的最后一张壁纸（O(1) 时间复杂度）
        public static string GetPreviousIdFolder()
        {
            if (allIds.Count == 0) return null;

            // 如果是首次调用，初始化到最后一个ID
            if (currentIdIndex == -1)
            {
                GoToLastFile();
                return GetCurrentWallpaperPath();
            }

            // 移动到上一个ID
            //currentIdIndex = (currentIdIndex - 1 + allIds.Count) % allIds.Count;
            if (currentIdIndex == lastId) currentIdIndex = (currentIdIndex - 1 + allIds.Count) % allIds.Count;
            else
                currentIdIndex = lastId;
            currentFileIndex = 0;

            // 跳过空的ID文件夹
            while (currentFileIndex < 0)
            {
                currentIdIndex = (currentIdIndex - 1 + allIds.Count) % allIds.Count;
                currentFileIndex = idToFilesMap[allIds[currentIdIndex]].Count - 1;
            }

            SaveState();
            return GetCurrentWallpaperPath();
        }

        // 辅助方法
        public static string GetCurrentWallpaperPath()
        {
            if (currentIdIndex < 0 || currentIdIndex >= allIds.Count)
                return null;

            var currentId = allIds[currentIdIndex];
            if (currentFileIndex < 0 || currentFileIndex >= idToFilesMap[currentId].Count)
                return null;

            return Path.Combine(wallpapersPath, currentId, idToFilesMap[currentId][currentFileIndex]);
        }

        private static void ResetToFirstFile()
        {
            if (allIds.Count > 0)
            {
                currentIdIndex = 0;
                // 找到第一个非空ID文件夹
                while (idToFilesMap[allIds[currentIdIndex]].Count == 0 && currentIdIndex < allIds.Count - 1)
                {
                    currentIdIndex++;
                }
                currentFileIndex = 0;
                SaveState();
            }
        }

        private static void GoToLastFile()
        {
            if (allIds.Count > 0)
            {
                currentIdIndex = allIds.Count - 1;
                // 找到最后一个非空ID文件夹
                while (idToFilesMap[allIds[currentIdIndex]].Count == 0 && currentIdIndex > 0)
                {
                    currentIdIndex--;
                }
                currentFileIndex = idToFilesMap[allIds[currentIdIndex]].Count - 1;
                SaveState();
            }
        }

        private static List<string> GetImageFiles(string directory)
        {
            try
            {
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp" };
                return Directory.GetFiles(directory)
                    .Where(f => imageExtensions.Contains(Path.GetExtension(f).ToLower()))
                    .OrderBy(f => f)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取图片文件时出错: {ex.Message}");
                return new List<string>();
            }
        }

        private static void LoadState()
        {
            if (File.Exists(stateFilePath))
            {
                var state = JsonConvert.DeserializeObject<WallpaperState>(File.ReadAllText(stateFilePath));
                if (state != null)
                {
                    if (allIds.Contains(state.CurrentId))
                    {
                        currentIdIndex = allIds.IndexOf(state.CurrentId);
                        var files = idToFilesMap[state.CurrentId];
                        if (files.Contains(state.CurrentFile))
                        {
                            currentFileIndex = files.IndexOf(state.CurrentFile);
                        }
                        else { currentFileIndex = 0; }
                    }
                }
            }
        }

        private static void SaveState()
        {
            try
            {
                var state = new WallpaperState
                {
                    CurrentId = currentIdIndex >= 0 && currentIdIndex < allIds.Count ? allIds[currentIdIndex] : null,
                    CurrentFile = currentFileIndex >= 0 && currentIdIndex < allIds.Count
                        ? idToFilesMap[allIds[currentIdIndex]][currentFileIndex] : null
                };

                File.WriteAllTextAsync(stateFilePath, JsonConvert.SerializeObject(state));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存状态时出错: {ex.Message}");
            }
        }

        private static void ValidateCurrentState()
        {
            // 确保当前位置有效
            if (currentIdIndex < 0 || currentIdIndex >= allIds.Count ||
                currentFileIndex < 0 || currentFileIndex >= idToFilesMap[allIds[currentIdIndex]].Count)
            {
                var state = JsonConvert.DeserializeObject<WallpaperState>(File.ReadAllText(stateFilePath));
                if (state != null)
                {
                    if (allIds.Contains(state.CurrentId))
                    {
                        currentIdIndex = allIds.IndexOf(state.CurrentId);
                        var files = idToFilesMap[state.CurrentId];
                        if (files.Contains(state.CurrentFile))
                        {
                            currentFileIndex = files.IndexOf(state.CurrentFile);
                        }
                    }
                }
                throw new Exception("当前状态无效，重置到第一个文件" + currentIdIndex + " " + currentFileIndex + " " + state.CurrentId + " " + state.CurrentFile);
                ResetToFirstFile();
            }
        }

        // 用于状态持久化的类
        private class WallpaperState
        {
            public string CurrentId { get; set; }
            public string CurrentFile { get; set; }
        }
        // 在 WallpaperManager 类中添加以下方法
        public static void DeleteCurrentWallpaper()
        {
            //if (!can_set_wallpaper)
            //    return;
            var _currentPath = GetWallpaperFromRegistry();
            if (!_currentPath.Contains(keyupMusic)) return;
            if (!IsDesktopFocused() && !isctrl()) return;
            string currentPath = GetCurrentWallpaperPath();
            if (string.IsNullOrEmpty(currentPath) || !File.Exists(currentPath))
            {
                Console.WriteLine("当前壁纸不存在或路径无效");
                return;
            }

            try
            {
                // 获取当前文件信息
                string currentId = allIds[currentIdIndex];
                string currentFile = idToFilesMap[currentId][currentFileIndex];
                idToFilesMap[currentId].Remove(currentFile);

                //SetDesktopToBlack();
                //Sleep(100);
                play_sound_di2();
                // 移动到下一个壁纸
                SetDesktopWallpaper(GetCurrentWallpaperPath(), WallpaperStyle.Fit);


                // 物理删除文件
                //File.Delete(currentPath);
                //Console.WriteLine($"已删除壁纸: {currentPath}");
                // 将文件移入回收站
                FileSystem.DeleteFile(
                    currentPath,
                    UIOption.OnlyErrorDialogs,
                    RecycleOption.SendToRecycleBin
                );


                // 检查当前ID文件夹是否为空
                if (idToFilesMap[currentId].Count == 0)
                {
                    // 可选：删除空文件夹
                    // Directory.Delete(Path.Combine(wallpapersPath, currentId));
                    // allIds.Remove(currentId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除壁纸时出错: {ex.Message}");
            }
        }// 查找相同的 PNG 文件
        public static List<string> FindDuplicatePNGs(string targetFile, string searchDirectory)
        {
            var duplicates = new List<string>();

            // 检查目标文件是否存在
            if (!File.Exists(targetFile) || Path.GetExtension(targetFile).ToLower() != ".png")
            {
                throw new ArgumentException("目标文件不存在或不是 PNG 格式");
            }

            // 获取目标文件的大小和哈希
            long targetSize = new FileInfo(targetFile).Length;


            // 递归处理子目录
            foreach (string subDir in Directory.GetDirectories(searchDirectory))
            {
                string subDirName = Path.GetFileName(subDir);
                //string destSubDir = Path.Combine(destDir, subDirName);
                // 遍历目录下的所有 PNG 文件
                foreach (string file in Directory.EnumerateFiles(subDir, "*.png", SearchOption.AllDirectories))
                {
                    // 跳过自身
                    if (file.Equals(targetFile, StringComparison.OrdinalIgnoreCase))
                        continue;

                    // 比较文件大小
                    long fileSize = new FileInfo(file).Length;
                    //if (!(fileSize < targetSize+10&&fileSize>targetSize-10))
                    if (fileSize != targetSize)
                        continue;
                    duplicates.Add(file);
                }

            }

            return duplicates;
        }
        public static void SetDesktopToBlack()
        {
            var currentPath = GetWallpaperFromRegistry();
            if (!currentPath.Contains(keyupMusic)) return;

            try
            {
                // 步骤 1: 设置壁纸为空
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, "", SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);

                // 步骤 2: 修改注册表设置背景颜色为黑色
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Colors", true))
                {
                    if (key != null)
                    {
                        key.SetValue("Background", "0 0 0"); // RGB黑色
                    }
                }

                // 步骤 3: 设置壁纸样式为居中 (确保纯色显示)
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
                {
                    if (key != null)
                    {
                        key.SetValue("WallpaperStyle", "0"); // 居中
                        key.SetValue("TileWallpaper", "0");  // 不平铺
                    }
                }

                // 步骤 4: 强制刷新桌面
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, "", SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);

                Console.WriteLine("桌面背景已设置为纯黑色");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"设置桌面背景时出错: {ex.Message}");
            }
        }
        public static void DeleteDuplicateFiles(string rootDirectory)
        {
            // 用于记录已存在的文件名
            Dictionary<string, string> existingFiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                // 获取所有文件路径（包括子目录）
                string[] allFiles = Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories);

                // 遍历所有文件，按文件名去重
                foreach (string filePath in allFiles)
                {
                    string fileName = Path.GetFileName(filePath);

                    // 检查文件名是否已存在
                    if (existingFiles.ContainsKey(fileName))
                    {
                        Console.WriteLine($"发现重复文件: {filePath}");
                        Console.WriteLine($"保留第一个文件: {existingFiles[fileName]}");

                        // 删除重复文件
                        try
                        {
                            //File.Delete(filePath);
                            FileSystem.DeleteFile(filePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin
                          );
                            Console.WriteLine($"已删除: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"删除失败: {filePath} - {ex.Message}");
                        }
                    }
                    else
                    {
                        // 记录首次出现的文件名和路径
                        existingFiles.Add(fileName, filePath);
                        Console.WriteLine($"首次发现文件: {filePath}");
                    }
                }

                Console.WriteLine($"处理完成，共检查 {allFiles.Length} 个文件，保留 {existingFiles.Count} 个唯一文件");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理过程中发生错误: {ex.Message}");
            }
        }
        private const int SPI_GETDESKWALLPAPER = 0x0073;
        private const int MAX_PATH = 260;
        public static string GetDesktopWallpaperPath()
        {
            StringBuilder wallpaperPath = new StringBuilder(MAX_PATH);
            var result = SystemParametersInfo(SPI_GETDESKWALLPAPER, MAX_PATH, wallpaperPath.ToString(), 0);
            if (result > 0)
                return wallpaperPath.ToString();
            else
                throw new Exception("获取壁纸路径失败");
        }
        public static string GetWallpaperFromRegistry()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop"))
            {
                if (key != null)
                {
                    var wallpaper = key.GetValue("WallPaper") as string;
                    return wallpaper ?? string.Empty;
                }
            }
            return string.Empty;
        }
    }
}