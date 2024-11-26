using HttpServer.Core;
using HttpServer.Frame.Storage;
using HttpServer.Frame.Tools;
using HttpServer.RunTime.Event;
using LitJson;
using System.Net;
using System.Text;

namespace HttpServer.Frame.Helper
{
    internal class HttpHelper
    {
        public void Laucher()
        {
            string url = FileHelper.ReadTextFile(FPath.IP);
            string[] split = url.Split(":");
            if (split.Length == 2)
            {
                CHttpServer httpServer = new CHttpServer(split[0], split[1]);
                Thread thread = new Thread(new ThreadStart(httpServer.Launcher));
                thread.Start();
            }
        }

        public static void LoginRequestProcess(HttpListenerContext context)
        {

        }
    }
}
