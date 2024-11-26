using Cysharp.Threading.Tasks;
using HttpServer.Core;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Storage;
using HttpServer.Frame.Tools;
using HttpServer.RunTime.Event;
using LitJson;
using static HttpServer.Core.CHttpServer;

public class ResEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<ResourcesInfo> infs = await StorageHelper.GetInfo(StorageHelper.m_storageObj.rsCheck);
        
        string inf = JsonMapper.ToJson(infs);
        CHttpServer.HttpSendAsync(pkg.Context, inf, EventType.ResEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        await UniTask.Yield();
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        await UniTask.Yield();
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        ResourcesInfo info = JsonMapper.ToObject<ResourcesInfo>(pkg.messPkg.ret);
        List<ResourcesInfo> new_list = new List<ResourcesInfo>(StorageHelper.m_storageObj.rsCheck);

        int i = -1;
        i = StorageHelper.m_storageObj.rsCheck.FindIndex(x => x.relaPath == info.relaPath);
        if (i != -1) 
        {
            string deletePath = FPath.STORAGE_ROOT_PATH + "\\Data\\" + info.relaPath;
            File.Delete(deletePath);
            new_list = await StorageHelper.DeleteInfo(StorageHelper.m_storageObj.rsCheck, (x) => {return x.relaPath == info.relaPath;});
        }
        
        string body = JsonMapper.ToJson(new_list);
        CHttpServer.HttpSendAsync(pkg.Context, body, EventType.ResEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        await UniTask.Yield();
    }      
}