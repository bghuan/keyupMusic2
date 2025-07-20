using Microsoft.Win32;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.Json;
using System.Windows.Forms;
using static keyupMusic2.Native;
using Point = System.Drawing.Point;
using RECT = keyupMusic2.Native.RECT;
namespace keyupMusic2
{
    public partial class Common
    { 
        public class KeyProcessor
        {
            public void press(Keys[] keys)
            {
                Common.press(keys);
            }
        }

        /// <summary>
        /// 解析字符串并通过反射执行对应方法
        /// </summary>
        /// <param name="command">格式如：'press([Keys.A,Keys.B])'</param>
        /// <param name="target">包含目标方法的实例</param>
        public static void ExecuteCommand(string command)
        {
            try
            {
                // 1. 解析方法名（提取press(...)中的方法名）
                int openParenIndex = command.IndexOf('(');
                int closeParenIndex = command.LastIndexOf(')');
                if (openParenIndex <= 0 || closeParenIndex <= openParenIndex)
                {
                    throw new FormatException("命令格式错误，缺少括号");
                }
                string methodName = command.Substring(0, openParenIndex).Trim();
                string paramContent = command.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1).Trim();

                // 2. 解析参数（处理[Keys.A, Keys.B]格式）
                var parameters = ParseParameters(paramContent);

                // 3. 获取静态类Common的类型
                Type staticType = typeof(Common);  // 静态类通过typeof获取类型
                                                   // 定义要获取的方法类型（包含所有访问级别和静态方法）
                BindingFlags bindingFlags =
                    BindingFlags.Static |          // 只获取静态方法
                    BindingFlags.Public |          // 包含公共方法
                    BindingFlags.NonPublic;        // 包含私有/内部方法（如private、internal）

                // 获取所有符合条件的方法
                MethodInfo[] methods = staticType.GetMethods(bindingFlags);

                // 过滤掉滤掉特殊方法（如静态构造函数、Object基类方法等）
                //var filteredMethods = methods
                //    .Where(m => !m.IsSpecialName)  // 排除特殊方法（如构造函数、属性访问器等）
                //    .OrderBy(m => m.Name)          // 按方法名排序
                //    .ToList();

                // 4. 获取静态方法（注意BindingFlags需Static）
                MethodInfo method = staticType.GetMethod(
                    methodName,
                    bindingFlags,
                    null,
                    new[] { typeof(Keys[]) ,typeof(int) },  // 方法参数类型
                    null
                );

                if (method == null)
                {
                    throw new MissingMethodException($"未找到方法：{methodName}(Keys[])");
                }
                object[] a = new object[] { parameters, 10 };     //
                // 4. 执行方法
                method.Invoke(staticType, a);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"执行失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 解析参数字符串为Keys数组
        /// </summary>
        /// <param name="paramContent">如："[Keys.A, Keys.B]" 或 "Keys.A, Keys.B"</param>
        /// <returns>解析后的Keys数组</returns>
        private static Keys[] ParseParameters(string paramContent)
        {
            // 移除可能的方括号
            paramContent = paramContent.Trim('[', ']').Trim();
            if (string.IsNullOrEmpty(paramContent))
            {
                return Array.Empty<Keys>();
            }

            // 分割参数项（如"Keys.A, Keys.B" → ["Keys.A", "Keys.B"]）
            string[] keyParts = paramContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(p => p.Trim())
                                           .ToArray();

            // 转换为Keys枚举值
            List<Keys> keys = new List<Keys>();
            foreach (string part in keyParts)
            {
                // 提取枚举成员名（如"Keys.A" → "A"）
                string enumMember = part.Replace("Keys.", "").Trim();
                if (Enum.TryParse<Keys>(enumMember, out Keys key))
                {
                    keys.Add(key);
                }
                else
                {
                    throw new ArgumentException($"无效的Keys值：{part}");
                }
            }

            return keys.ToArray();
        }
    }
}
