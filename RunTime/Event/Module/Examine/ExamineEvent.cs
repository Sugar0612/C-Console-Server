using LitJson;
using Cysharp.Threading.Tasks;
using HttpServer.RunTime.Event;
using static HttpServer.Core.CHttpServer;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Storage;
using HttpServer.Core;
using HttpServer.Frame.Http;

public class ExamineEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<ExamineInfo> infs = await StorageHelper.GetInfo(StorageHelper.m_storageObj.examineesInfo);
        
        string inf = JsonMapper.ToJson(infs);
        httpMethod.HttpSendAsync(pkg.Context, inf, EventType.ExamineEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        ExamineInfo info = JsonMapper.ToObject<ExamineInfo>(pkg.messPkg.ret);
        List<ExamineInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.m_storageObj.examineesInfo, 
            x => x.CourseName == info.CourseName && x.RegisterTime == info.RegisterTime);

        string body = JsonMapper.ToJson(new_list);
        httpMethod.HttpSendAsync(pkg.Context, body, EventType.ExamineEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        //Console.WriteLine($"Examine ReviseInfoEvent...");
        List<ExamineInfo> infoList = JsonMapper.ToObject<List<ExamineInfo>>(pkg.messPkg.ret);
        foreach (var info in infoList)
        {
            //Console.WriteLine($"{info.id} : {info.CourseName}");
            List<ExamineInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.m_storageObj.examineesInfo, x => x.id == info.id);   
            string s_inf = JsonMapper.ToJson(inf);
            httpMethod.HttpSendAsync(pkg.Context, s_inf, EventType.ExamineEvent, OperateType.REVISE);
        }
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        ExamineInfo info = JsonMapper.ToObject<ExamineInfo>(pkg.messPkg.ret);
        List<ExamineInfo> new_list = 
            await StorageHelper.DeleteInfo(StorageHelper.m_storageObj.examineesInfo, (x) => {return x.id == info.id;});
        
        string body = JsonMapper.ToJson(new_list);
        httpMethod.HttpSendAsync(pkg.Context, body, EventType.ExamineEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        ExamineInfo info = JsonMapper.ToObject<ExamineInfo>(pkg.messPkg.ret);
        List<ExamineInfo> inf = StorageHelper.SearchInf(StorageHelper.m_storageObj.examineesInfo, x => x.CourseName == info.CourseName);

        string s_inf = JsonMapper.ToJson(inf);
        httpMethod.HttpSendAsync(pkg.Context, s_inf, EventType.ExamineEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }      
}