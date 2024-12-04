using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HttpServer.Core;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Http;
using HttpServer.Frame.Storage;
using HttpServer.RunTime.Event;
using LitJson;
using static HttpServer.Core.CHttpServer;

public class ColumnsEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<ColumnsInfo> infs = await StorageHelper.GetInfo(StorageHelper.m_storageObj.columnsInfo);
        
        string inf = JsonMapper.ToJson(infs);
        httpMethod.HttpSendAsync(pkg.Context, inf, EventType.ColumnsEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        ColumnsInfo info = JsonMapper.ToObject<ColumnsInfo>(pkg.messPkg.ret);
        List<ColumnsInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.m_storageObj.columnsInfo, x => x.Name == info.Name);

        string body = JsonMapper.ToJson(new_list);
        httpMethod.HttpSendAsync(pkg.Context, body, EventType.ColumnsEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        ColumnsInfo info = JsonMapper.ToObject<ColumnsInfo>(pkg.messPkg.ret);
        List<ColumnsInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.m_storageObj.columnsInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(inf);
        httpMethod.HttpSendAsync(pkg.Context, s_inf, EventType.ColumnsEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        ColumnsInfo info = JsonMapper.ToObject<ColumnsInfo>(pkg.messPkg.ret);
        List<ColumnsInfo> new_list = new List<ColumnsInfo>(StorageHelper.m_storageObj.columnsInfo);

        int i = -1;
        i = StorageHelper.m_storageObj.courseInfo.FindIndex(x => x.Columns == info.Name);
        if (i == -1) new_list = await StorageHelper.DeleteInfo(StorageHelper.m_storageObj.columnsInfo, (x) => {return x.id == info.id;});
        
        string body = JsonMapper.ToJson(new_list);
        httpMethod.HttpSendAsync(pkg.Context, body, EventType.ColumnsEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        ColumnsInfo info = JsonMapper.ToObject<ColumnsInfo>(pkg.messPkg.ret);
        List<ColumnsInfo> inf = StorageHelper.SearchInf(StorageHelper.m_storageObj.columnsInfo, x => x.Name == info.Name);

        string s_inf = JsonMapper.ToJson(inf);
        httpMethod.HttpSendAsync(pkg.Context, s_inf, EventType.ColumnsEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }      
}