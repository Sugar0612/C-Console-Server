using HttpServer.Frame.Controller;
using HttpServer.RunTime.Event;
using System.Runtime.InteropServices;

namespace HttpServer.Core
{
    internal class Launcher
    {
        private static bool m_isClose = false;
        private static HelperController m_helperConsole = new HelperController();
        private static EventDispatcher m_dispatcher = new EventDispatcher();

        static void Main(string[] args)
        {
            m_helperConsole = new HelperController();

            Register();
            StartUpComponents();
            Update();
            Close();
        }

        static void Register()
        {
            m_helperConsole.Register();
        }

        static void StartUpComponents()
        {
            CHttpServer server = new CHttpServer();
            m_helperConsole.StartUpComponents();
        }

        static void Looper()
        {
            while (!m_isClose)
            {
                if (CHttpServer.MessQueue.Count > 0)
                {
                    var pkg = CHttpServer.MessQueue.Dequeue();
                    m_dispatcher.Dispatcher(pkg);
                }
            }
        }

        static void Update()
        {
            Thread thread = new Thread(() => { Looper(); });
            thread.Start();
        }

        static void Close()
        {
            SetConsoleCtrlHandler(cancelHandler, true);
        }

        #region 激活关闭窗口事件
        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerAppClose, bool Add);
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerAppClose);

        /// <summary>
        /// 关闭窗口时的事件
        /// </summary>
        /// <param name="CtrlType"></param>
        /// <returns></returns>
        static bool HandlerAppClose(int CtrlType)
        {
            m_helperConsole.Close();
            m_isClose = true;
            return false;
        }
        #endregion
    }
}
