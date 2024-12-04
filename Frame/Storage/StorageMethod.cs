using HttpServer.Frame.Helper;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer.Frame.Storage
{
    public class StorageMethod
    {
        public static void HardDisk2Memory<T>(string filePath, out List<T> targetList)
        {
            string jsonString = FileHelper.ReadTextFile(filePath);
            if (jsonString.Count() == 0)
            {
                targetList = new List<T>();
                return;
            }
            targetList = JsonMapper.ToObject<List<T>>(jsonString);
        }

        public static void Memory2HardDisk<T>(string savePath, List<T> saveList)
        {
            if (saveList == null) return;
            string json = JsonMapper.ToJson(saveList);
            Console.WriteLine($"{savePath}: {json}");
            FileHelper.WriteTextFile(savePath, json);
        }
    }
}
