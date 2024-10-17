using Cysharp.Threading.Tasks;
using HttpServer.Core;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Storage;
using HttpServer.RunTime.Event;
using LitJson;
using static HttpServer.Core.CHttpServer;

public class FacultyEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<FacultyInfo> infs = await StorageHelper.GetInfo(StorageHelper.m_storageObj.faculiesInfo);
        
        string inf = JsonMapper.ToJson(infs);
        CHttpServer.HttpSendAsync(pkg.Context, inf, EventType.FacultyEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.m_storageObj.faculiesInfo, x => x.Name == info.Name);

        string body = JsonMapper.ToJson(new_list);
        CHttpServer.HttpSendAsync(pkg.Context, body, EventType.FacultyEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.m_storageObj.faculiesInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(inf);
        CHttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.FacultyEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> new_list = new List<FacultyInfo>(StorageHelper.m_storageObj.faculiesInfo);
        int i = -1;
        i = StorageHelper.m_storageObj.majorInfo.FindIndex(x => x.FacultyName == info.Name);
        if (i == -1)
        {
            new_list = 
                await StorageHelper.DeleteInfo(StorageHelper.m_storageObj.faculiesInfo, (x) => {return x.id == info.id;});
        }
        
        
        string body = JsonMapper.ToJson(new_list);
        CHttpServer.HttpSendAsync(pkg.Context, body, EventType.FacultyEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> inf = StorageHelper.SearchInf(StorageHelper.m_storageObj.faculiesInfo, x => x.Name == info.Name);

        string s_inf = JsonMapper.ToJson(inf);
        CHttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.FacultyEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }
}