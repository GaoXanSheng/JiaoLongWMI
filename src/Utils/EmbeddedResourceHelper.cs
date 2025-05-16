using System;
using System.IO;
using System.Reflection;

namespace JiaoLongWMI.Utils
{
    public static class EmbeddedResourceHelper
    {
        /// <summary>
        /// 从嵌入资源中提取文件并写入指定路径
        /// </summary>
        /// <param name="resourceName">资源名称（完整命名空间 + 路径 + 文件名，点分隔）</param>
        /// <param name="outputFilePath">输出文件完整路径</param>
        public static void ExtractEmbeddedResourceToFile(string resourceName, string outputFilePath)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using Stream? resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
                throw new Exception($"未找到嵌入资源: {resourceName}");

            // 确保输出目录存在
            var directory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using FileStream fileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);
            resourceStream.CopyTo(fileStream);
        }

        /// <summary>
        /// 将嵌入资源解压到当前程序运行目录
        /// </summary>
        /// <param name="resourceName">嵌入资源完整名称</param>
        /// <param name="fileName">解压后的文件名</param>
        /// <returns>返回解压后的完整文件路径</returns>
        public static string ExtractResourceToExeDir(string resourceName, string fileName)
        {
            string exeDir = AppContext.BaseDirectory;
            string outputPath = Path.Combine(exeDir, fileName);
            ExtractEmbeddedResourceToFile(resourceName, outputPath);
            return outputPath;
        }

        /// <summary>
        /// 删除当前程序运行目录的指定文件（如果存在）
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static void DeleteFileInExeDir(string fileName)
        {
            string exeDir = AppContext.BaseDirectory;
            string filePath = Path.Combine(exeDir, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    // 根据需要处理异常，比如日志记录
                    Console.WriteLine($"删除文件失败: {filePath}, 异常: {ex.Message}");
                }
            }
        }
    }
}
