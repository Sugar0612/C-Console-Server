using LitJson;
using System.Text;

namespace HttpServer.Frame.Helper
{
    internal class FileHelper
    {
        /// <summary>
        /// 文本文件文件写入
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="contents">内容</param>
        public static void WriteTextFile(string path, string contents)
        {
            if (!File.Exists(path))  // 判断是否已有相同文件 
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.SetLength(0);
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.WriteLine(contents);
                    }
                }
            }
        }

        /// <summary>
        /// 读取本地文本文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns>Json内容</returns>
        public static string ReadTextFile(string filepath)
        {
            string content = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    content = sr.ReadToEnd().ToString();
                }
            }

            JsonMapper.ToObject(content);
            return content;
        }
    }
}
