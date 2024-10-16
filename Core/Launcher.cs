using HttpServer.Frame.Controller;

namespace HttpServer.Core
{
    internal class Launcher
    {
        private static HelperController m_helperConsole = new HelperController();
       
        static void Main(string[] args)
        {
            m_helperConsole = new HelperController();

            Register();
            StartUpComponents();
        }

        static void Register()
        {
            m_helperConsole.Register();
        }

        static void StartUpComponents()
        {
            m_helperConsole.StartUpComponents();
        }
    }
}
