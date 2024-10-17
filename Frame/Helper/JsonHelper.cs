using Cysharp.Threading.Tasks;
using HttpServer.Frame.Storage;
using LitJson;

namespace HttpServer.Frame.Helper
{
    internal class JsonHelper
    {
        /// <summary>
        /// string => Obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T String2Object<T>(string json) where T : class
        {
            object Obj = JsonMapper.ToObject(json);
            return Obj == null ? null : Obj as T;
        }

        /// <summary>
        /// Obj => string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Obj2Str<T>(T obj)
        {
            return JsonMapper.ToJson(obj);
        }

        /// <summary>
        /// 异步去做ToJson
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static async UniTask<string> AsyncToJson(object obj)
        {
            string result = "";
            await UniTask.Run(() =>
            {
                result = JsonMapper.ToJson(obj);
            });
            return result;
        }
    }
}
