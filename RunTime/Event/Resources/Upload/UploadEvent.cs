using Cysharp.Threading.Tasks;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Tools;
using HttpServer.RunTime.Event;
using LitJson;
using static HttpServer.Core.CHttpServer;
using static System.Net.Mime.MediaTypeNames;

public class UploadEvent : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg asynExPkg) 
    {
        FilePackage data = JsonMapper.ToObject<FilePackage>(asynExPkg.messPkg.ret);
        string savepath = FPath.STORAGE_ROOT_PATH + "\\Data\\" + data.relativePath;
        Tools.Bytes2File(data.fileData, savepath);

        StorageHelper.UpdateThisFileInfo(data.relativePath);
    }
}

/// <summary>
/// ÎÄ¼þ°ü
/// </summary>
public class FilePackage
{
    public string fileName;
    public string relativePath;
    public byte[] fileData;
}

