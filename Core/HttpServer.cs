using HttpServer.Frame.Helper;
using HttpServer.Frame.Storage;
using HttpServer.Frame.Tools;
using HttpServer.RunTime.Event;
using LitJson;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace HttpServer.Core
{
    public class CHttpServer
    {
        private HttpListener listener = new HttpListener();
        private string content = "";
        private string m_Ip = "";
        public static Queue<AsyncExpandPkg> MessQueue = new Queue<AsyncExpandPkg>();

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

            NativePostRequest(context);
        }

        public static void Send(HttpListenerContext context, string mess)
        {
            HttpListenerResponse response = context.Response;
            response.ContentLength64 = Encoding.UTF8.GetByteCount(mess);
            response.ContentType = "text/html; charset=UTF-8";
            Stream output = response.OutputStream;
            StreamWriter writer = new StreamWriter(output);
            writer.Write(mess);
            writer.Close();
        }

        public static void HttpSendAsync(HttpListenerContext context, string mess, EventType event_type, OperateType operateType)
        {
            // Console.WriteLine("Send: " + mess);
            string front = FrontPackage(mess, event_type, operateType);
            string totalInfoPkg = "|" + front + "#" + mess + "@";
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
            Send(context, "");
        }

        public async void NativePostRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            string? actionName = request.Url?.AbsolutePath;
            if (actionName == null) return;

            Console.WriteLine($"actionName: {actionName}");

            if (actionName == "/getUserList" && request.HttpMethod == "POST")
            {
                List<UserInfo> usersList = await StorageHelper.GetInfo(StorageHelper.m_storageObj.usersInfo);
                string retContext = JsonMapper.ToJson(usersList);
                NativeSend(context.Response, retContext);
            }
            else if (actionName == "/getSoftwareList" && request.HttpMethod == "POST")
            {
                List<SoftwareInfo> softwareList = await StorageHelper.GetInfo(StorageHelper.m_storageObj.softwareInfo);
                string retContext = JsonMapper.ToJson(softwareList);
                NativeSend(context.Response, retContext);
            }
            else if (actionName == "/Login" && request.HttpMethod == "POST")
            {
                string content = GetRequestContent(context);
                UserInfo inf = JsonMapper.ToObject<UserInfo>(content);
                inf = StorageHelper.CheckUserLogin(inf);
                string s_inf = JsonMapper.ToJson(inf);
                NativeSend(context.Response, s_inf);
            }
            else if (actionName == "/Register" && request.HttpMethod == "POST")
            {
                string content = GetRequestContent(context);
                UserInfo inf = JsonMapper.ToObject<UserInfo>(content);
                inf = await StorageHelper.Register(inf);
                string s_inf = JsonMapper.ToJson(inf);
                NativeSend(context.Response, s_inf);
            }
            else
            {
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

        public void PostRequestProcess(HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;
            response.AppendHeader("Access-Control-Allow-Origin", "*");

            Stream stream = context.Request.InputStream;
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);

            content = reader.ReadToEnd();
            content = Tools.StringToUnicode(content);

            string log = "Post content: " + content;
            // Console.WriteLine(log);

            MessPackage client_pkg = new MessPackage();
            AsyncExpandPkg pkg = new AsyncExpandPkg();
            pkg.messPkg = client_pkg;
            pkg.Context = context;

            string[] messages = content.Split("@");
            foreach (var message in messages)
            {
                InforProcessing(message, pkg);
            }
        }

        public void GetRequestProcess(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            var content = request.QueryString;
        }

        /// <summary>
        /// 前置包
        /// </summary>
        /// <param name="cli"></param>
        /// <param name="mess"></param>
        public static string FrontPackage(string mess, EventType event_type, OperateType operateType)
        {
            MessPackage data = new MessPackage()
            {
                length = mess.Length,
                event_type = event_type.ToString(),
                operate_type = operateType.ToString()
            };
            string s_info = JsonMapper.ToJson(data);
            return s_info;
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

        /// <summary>
        /// 前置包和内容包解析
        /// </summary>
        /// <param name="pkg"></param>
        public static void ParsingThePackageBody(string package, AsyncExpandPkg pkg)
        {
            string[] Split = package.Split("#");
            string front = Split[0];
            string main = Split[1];

            JsonData data = JsonMapper.ToObject(front);
            pkg.messPkg.ip = data["ip"].ToString();
            pkg.messPkg.length = int.Parse(data["length"].ToString());
            pkg.messPkg.event_type = data["event_type"].ToString();
            pkg.messPkg.operate_type = data["operate_type"].ToString();
            pkg.messPkg.get_length = true;
            pkg.messPkg.ret = main;
            MessQueueAdd(pkg);
            pkg.messPkg.Clear();
        }

        /// <summary>
        /// 进度检查
        /// </summary>
        /// <param name="pkg"></param>
        public static void check(AsyncExpandPkg pkg)
        {
            int messLength = pkg.messPkg.ret.Count() + 2;
            float percent = messLength * 1.0f / pkg.messPkg.length * 1.0f * 100.0f;
            // Debug.Log($"messLength: {messLength}, messLength: {pkg.messPkg.ret}");
            Console.WriteLine($"pkg.messPkg.length: {pkg.messPkg.length}, percent: {percent}");
            if (percent >= 100.0f || percent >= 99.99f)
            {
                pkg.messPkg.finish = true;
                ParsingThePackageBody(pkg.messPkg.ret, pkg);
            }
        }

        /// <summary>
        /// 信息处理
        /// </summary>
        /// <param name="mess"></param>
        /// <param name="mp"></param>
        public static void InforProcessing(string mess, AsyncExpandPkg pkg)
        {
            if (mess.Count() == 0 || mess == null) return;

            // Debug.Log("================ mess : " + mess + " || " + mess.Count());
            string[] lengthSplit = mess.Split("|");
            string totalLength = lengthSplit[0];
            if (!pkg.messPkg.get_length && !string.IsNullOrEmpty(totalLength))
            {
                pkg.messPkg.length = int.Parse(totalLength);
                pkg.messPkg.get_length = true;
                pkg.messPkg.ret += lengthSplit[1];
            }
            else
            {
                if (pkg.messPkg.length > pkg.messPkg.ret.Count())
                {
                    pkg.messPkg.ret += mess;
                }
            }
            check(pkg);
        }

        public static string GetRequestContent(HttpListenerContext context)
        {
            Stream stream = context.Request.InputStream;
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string content = reader.ReadToEnd();
            content = Tools.StringToUnicode(content);
            return content;
        }

        /// <summary>
        /// 这是一个接受完整信息的 信息包类
        /// </summary>
        public class MessPackage
        {
            // public Socket socket = default; // 发送信息的soket
            public string ip = ""; // ip
            public string ret = ""; // 发送的信息
            public string operate_type = ""; // 操作类型
            public string event_type = ""; // 这个信息属于什么类型
            public int length = 0; // 这个包的总长度
            public bool finish = false; // 是否完全收包
            public bool get_length = false; // 是否已经通过前置包获取到了内容包的总长度

            public void Clear()
            {
                // socket = default;
                ip = "";
                ret = "";
                event_type = "";
                length = 0;
                finish = false;
                get_length = false;
            }

            public MessPackage() { }

            public MessPackage(MessPackage pkg)
            {
                // socket = pkg.socket;
                ip = pkg.ip;
                ret = pkg.ret;
                event_type = pkg.event_type;
                operate_type = pkg.operate_type;
                length = pkg.length;
                finish = pkg.finish;
                get_length = pkg.get_length;
            }
        }

        /// <summary>
        /// 异步回调扩展包
        /// </summary>
        public class AsyncExpandPkg
        {
            public HttpListenerContext Context;
            public MessPackage messPkg;

            public AsyncExpandPkg() { }

            public AsyncExpandPkg(AsyncExpandPkg pkg)
            {
                Context = pkg.Context;
                messPkg = new MessPackage(pkg.messPkg);
            }
        }
    }
}