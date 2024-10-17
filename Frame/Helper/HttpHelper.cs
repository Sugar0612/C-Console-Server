using HttpServer.Core;

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
    }
}
