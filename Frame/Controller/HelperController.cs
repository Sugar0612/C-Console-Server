using HttpServer.Frame.Helper;

namespace HttpServer.Frame.Controller
{
    internal class HelperController
    {
        private HttpHelper m_httpHelper = new HttpHelper();
        private StorageHelper m_storageHelper = new StorageHelper();

        public void Register()
        {
            // m_httpHelper = new HttpHelper();
            m_storageHelper = new StorageHelper();
        }

        public void StartUpComponents()
        {
            // m_httpHelper.Laucher();
            m_storageHelper.LoadHardDisk2Memory();
        }

        public void Close()
        {
            m_storageHelper.SaveHardDisk();
        }
    }
}
