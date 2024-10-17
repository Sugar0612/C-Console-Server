using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HttpServer.Core;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Storage;
using HttpServer.RunTime.Event;
using LitJson;
using static HttpServer.Core.CHttpServer;

public class CourseEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<CourseInfo> infs = await StorageHelper.GetInfo(StorageHelper.m_storageObj.courseInfo);
        
        string inf = JsonMapper.ToJson(infs);
        CHttpServer.HttpSendAsync(pkg.Context, inf, EventType.CourseEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        CourseInfo info = JsonMapper.ToObject<CourseInfo>(pkg.messPkg.ret);
        List<CourseInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.m_storageObj.courseInfo, x => x.CourseName == info.CourseName);

        string body = JsonMapper.ToJson(new_list);
        CHttpServer.HttpSendAsync(pkg.Context, body, EventType.CourseEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        CourseInfo info = JsonMapper.ToObject<CourseInfo>(pkg.messPkg.ret);
        List<CourseInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.m_storageObj.courseInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(inf);
        CHttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.CourseEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        CourseInfo info = JsonMapper.ToObject<CourseInfo>(pkg.messPkg.ret);
        List<CourseInfo> new_list = 
            await StorageHelper.DeleteInfo(StorageHelper.m_storageObj.courseInfo, (x) => {return x.id == info.id;});
        
        string body = JsonMapper.ToJson(new_list);
        CHttpServer.HttpSendAsync(pkg.Context, body, EventType.CourseEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        CourseInfo info = JsonMapper.ToObject<CourseInfo>(pkg.messPkg.ret);
        List<CourseInfo> inf = StorageHelper.SearchInf(StorageHelper.m_storageObj.courseInfo, x => x.CourseName == info.CourseName);

        string s_inf = JsonMapper.ToJson(inf);
        CHttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.CourseEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }  
}