using Cysharp.Threading.Tasks;
using HttpServer.Core;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Storage;
using HttpServer.RunTime.Event;
using LitJson;
using static HttpServer.Core.CHttpServer;

public class MajorEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<MajorInfo> infs = await StorageHelper.GetInfo(StorageHelper.m_storageObj.majorInfo);
        
        string s_inf = JsonMapper.ToJson(infs);
        CHttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.MajorEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.m_storageObj.majorInfo, x => x.MajorName == info.MajorName);

        string s_inf = JsonMapper.ToJson(new_list);
        CHttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.MajorEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> infs = await StorageHelper.ReviseInfo(info, StorageHelper.m_storageObj.majorInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(infs);
        CHttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.MajorEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> infs = new List<MajorInfo>(StorageHelper.m_storageObj.majorInfo);

        int i = -1;
        i = StorageHelper.m_storageObj.classesInfo.FindIndex(x => x.Major == info.MajorName);
        if (i == -1) { infs = await StorageHelper.DeleteInfo(StorageHelper.m_storageObj.majorInfo, x => x.id == info.id); }

        string s_inf = JsonMapper.ToJson(infs);
        CHttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.MajorEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> inf = StorageHelper.SearchInf(StorageHelper.m_storageObj.majorInfo, x => x.MajorName == info.MajorName);

        string s_inf = JsonMapper.ToJson(inf);
        CHttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.MajorEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }
}