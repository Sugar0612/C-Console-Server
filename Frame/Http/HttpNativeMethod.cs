using HttpServer.Frame.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer.Frame.Http
{
    public class HttpNativeMethod
    {
        public HttpNativeMethod() { }

        public string GetRequestContent(HttpListenerContext context)
        {
            Stream stream = context.Request.InputStream;
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string content = reader.ReadToEnd();
            content = Tools.Tools.StringToUnicode(content);
            return content;
        }

        public void NativePostRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            string? actionName = request.Url?.AbsolutePath;
            if (actionName == null) return;

            Console.WriteLine($"actionName: {actionName}");

            HttpHelper httpHelper = new HttpHelper();
            httpHelper.RequestProcess(actionName, request.HttpMethod, context);
        }

        public void NativeSend(HttpListenerResponse response, string retMessage)
        {
            response.ContentLength64 = Encoding.UTF8.GetByteCount(retMessage);
            response.ContentType = "text/html; charset=UTF-8";
            Stream output = response.OutputStream;
            StreamWriter writer = new StreamWriter(output);
            writer.Write(retMessage);
            writer.Close();
        }
    }
}
