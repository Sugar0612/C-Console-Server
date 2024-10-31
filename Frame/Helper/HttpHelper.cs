using HttpServer.Core;
using HttpServer.Frame.Storage;
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
            CHttpServer httpServer = new CHttpServer("10.0.2.155", "5800");

            Thread thread = new Thread(new ThreadStart(httpServer.Launcher));
            thread.Start();
        }

        public static void LoginRequestProcess(HttpListenerContext context)
        {

        }
    }
}
