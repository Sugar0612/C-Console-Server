using HttpServer.Frame.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer.Frame.Helper
{
    internal class StorageHelper
    {
        public void LoadHardDisk2Memory()
        {
            StorageObject storage = new StorageObject();
            storage.Init();
        }
    }
}
