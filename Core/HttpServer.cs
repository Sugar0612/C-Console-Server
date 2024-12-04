using HttpServer.Frame.Helper;
using HttpServer.Frame.Http;
using HttpServer.Frame.Storage;
using HttpServer.Frame.Tools;
using HttpServer.RunTime.Event;
using LitJson;
using System.Net;
using System.Text;
using HttpMethod = HttpServer.Frame.Http.HttpMethod;

namespace HttpServer.Core
{
    public class CHttpServer
    {
        private HttpListener listener = new HttpListener();
        private string m_Ip = "";
        public static Queue<AsyncExpandPkg> MessQueue = new Queue<AsyncExpandPkg>();

        public CHttpServer()
        {
            string url = FileHelper.ReadTextFile(FPath.IP);
            string[] split = url.Split(":");
            if (split.Length == 2)
            {
                m_Ip = $"http://{split[0]}:{split[1]}/";
                Thread thread = new Thread(new ThreadStart(Launcher));
                thread.Start();
            }
        }

        public void Launcher()
        {
            // Init
            listener = new HttpListener();
            listener.Prefixes.Add(m_Ip);
            listener.Start();

            // 提示信息
            string log = $"Server is Running! {DateTime.Now.ToString()}, Address url: {m_Ip}";
            Console.WriteLine(log);

            // 使用异步监听Web请求，当客户端的网络请求到来时会自动执行委托

            listener.BeginGetContext(Respones, null);

            while (true) ;
        }

        /// <summary>
        /// 客户端请求信息接收
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public void Respones(IAsyncResult ar)
        {
            // 再次开启异步监听
            listener.BeginGetContext(Respones, null);

            // 获取context对象
            var context = listener.EndGetContext(ar);

            // 获取请求体
            var request = context.Request;
            var response = context.Response;

            string log = $"{DateTime.Now} new Request , Method is : {request.HttpMethod}";
            Console.WriteLine(log);

            HttpMethod method = new HttpMethod();
            method.NativePostRequest(context);
        }

        /// <summary>
        /// 为消息队列 Clone pkg 并且存放
        /// </summary>
        /// <param name="pkg"></param>
        public static void MessQueueAdd(AsyncExpandPkg pkg)
        {
            AsyncExpandPkg exp_pkg = new AsyncExpandPkg(pkg);
            MessQueue.Enqueue(exp_pkg);
        }
    }
}