using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

using Microsoft.Win32;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using static keyupMusic2.Native;
using static keyupMusic2.winBinWallpaper;
using Microsoft.VisualBasic.FileIO; // 需要添加引用

namespace keyupMusic2
{
    public partial class Common
    {
        private static string wallpapersPath = Path.Combine(
        Directory.GetCurrentDirectory(), "image", "downloaded_images");
        private static string stateFilePath = Path.Combine(
            Directory.GetCurrentDirectory(), "log", "wallpaper_state.json");

        // 缓存目录结构和文件信息
        private static Dictionary<string, List<string>> idToFilesMap = new Dictionary<string, List<string>>();
        private static List<string> allIds = new List<string>();
        private static int currentIdIndex = -1;
        private static int currentFileIndex = -1;

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
            currentIdIndex = (currentIdIndex + 1) % allIds.Count;
            currentFileIndex = 0;

            // 跳过空的ID文件夹
            while (idToFilesMap[allIds[currentIdIndex]].Count == 0)
            {
                currentIdIndex = (currentIdIndex + 1) % allIds.Count;
            }

            SaveState();
            return GetCurrentWallpaperPath();
        }

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
            currentIdIndex = (currentIdIndex - 1 + allIds.Count) % allIds.Count;
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
        private static string GetCurrentWallpaperPath()
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
                try
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"加载状态时出错: {ex.Message}");
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
                ResetToFirstFile();
            }
        }

        // 用于状态持久化的类
        private class WallpaperState
        {
            public string CurrentId { get; set; }
            public string CurrentFile { get; set; }
        }
        public static bool ishide_DesktopWallpaper = false;
        // 在 WallpaperManager 类中添加以下方法
        public static void DeleteCurrentWallpaper()
        {
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
        }
        public static void SetDesktopToBlack()
        {
            try
            {
                ishide_DesktopWallpaper = true;
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

    }
}