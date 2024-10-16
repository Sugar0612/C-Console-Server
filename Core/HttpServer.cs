using HttpServer.Frame.Tools;
using System.Net;
using System.Text;

namespace HttpServer.Core
{
    public class CHttpServer
    {
        private HttpListener listener = new HttpListener();
        private string content = "";
        private string m_Ip = "";

        public CHttpServer(string url, string port)
        {
            m_Ip = $"http://{url}:{port}/";
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

            if (request.HttpMethod == "OPTIONS")
            {
                OptionsRequestProcess(context);
            }
            else if (request.HttpMethod == "POST") 
            {
                PostRequestProcess(context); 
            }
            else if (request.HttpMethod == "GET") 
            {
                GetRequestProcess(context); 
            }
        }

        public static void HttpSendAsync(HttpListenerContext context, string mess)
        {
            // Debug.Log("Send: " + mess);
            string totalInfoPkg = "|" + mess + "@";
            long totalLength = totalInfoPkg.Count();
            string finalPkg = totalLength.ToString() + totalInfoPkg;

            HttpListenerResponse response = context.Response;
            response.ContentLength64 = Encoding.UTF8.GetByteCount(finalPkg);
            response.ContentType = "text/html; charset=UTF-8";
            Stream output = response.OutputStream;
            StreamWriter writer = new StreamWriter(output);
            writer.Write(finalPkg);
            writer.Close();
        }

        public void OptionsRequestProcess(HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;
            response.AddHeader("Access-Control-Allow-Credentials", "true");
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Request-Method, X-Access-Token, X-Application-Name, X-Request-Sent-Time, X-Requested-With");
            response.AddHeader("Access-Control-Allow-Methods", "GET,PUT,POST,DELETE,OPTIONS");
            response.AppendHeader("Access-Control-Allow-Origin", "*");
            HttpSendAsync(context, "");
        }

        public void PostRequestProcess(HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;
            response.AppendHeader("Access-Control-Allow-Origin", "*");

            Stream stream = context.Request.InputStream;
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);

            content = reader.ReadToEnd();
            content = Tools.StringToUnicode(content);

            string log = "Post content: " + content;
            Console.WriteLine(log);

            HttpSendAsync(context, content);
        }

        public void GetRequestProcess(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            var content = request.QueryString;
        }
    }
}