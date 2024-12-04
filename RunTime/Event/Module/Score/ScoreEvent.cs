using Cysharp.Threading.Tasks;
using HttpServer.Core;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Http;
using HttpServer.Frame.Storage;
using HttpServer.RunTime.Event;
using LitJson;
using System.Diagnostics;
using static HttpServer.Core.CHttpServer;

public class ScoreEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<ScoreInfo> inf = await StorageHelper.GetInfo(StorageHelper.m_storageObj.scoresInfo);
        
        string s_inf = JsonMapper.ToJson(inf);
        httpMethod.HttpSendAsync(pkg.Context, s_inf, EventType.ScoreEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        ScoreInfo info = JsonMapper.ToObject<ScoreInfo>(pkg.messPkg.ret);
        List<ScoreInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.m_storageObj.scoresInfo, x => x.userName == info.userName 
                                && x.courseName == info.courseName && x.registerTime == info.registerTime && x.className == info.className);

        int examIdx = StorageHelper.m_storageObj.examineesInfo.FindIndex(x => x.ColumnsName == info.columnsName && x.CourseName == info.courseName
            && x.RegisterTime == info.registerTime);
        StorageHelper.m_storageObj.examineesInfo[examIdx].PNum += 1;
        
        string s_inf = JsonMapper.ToJson(new_list);
        httpMethod.HttpSendAsync(pkg.Context, s_inf, EventType.ScoreEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        ScoreInfo info = JsonMapper.ToObject<ScoreInfo>(pkg.messPkg.ret);
        int index = StorageHelper.m_storageObj.scoresInfo.FindIndex(x => x.userName == info.userName 
                            && x.courseName == info.courseName && x.registerTime == info.registerTime && x.className == info.className);
        // Console.WriteLine($"{info.columnsName} | {info.courseName} | {info.registerTime} | {info.trainingFinished} and index: {index}");
        if (index < 0 || index >= StorageHelper.m_storageObj.scoresInfo.Count) 
        {
            StorageHelper.m_storageObj.scoresInfo.Add(info);
            int examIdx = StorageHelper.m_storageObj.examineesInfo.FindIndex(x => x.ColumnsName == info.columnsName && x.CourseName == info.courseName
                        && x.RegisterTime == info.registerTime);
            if (examIdx >= 0 && examIdx < StorageHelper.m_storageObj.examineesInfo.Count)
                StorageHelper.m_storageObj.examineesInfo[examIdx].PNum += 1;
        }
        else StorageHelper.m_storageObj.scoresInfo[index] = info;
        
        string s_inf = JsonMapper.ToJson(StorageHelper.m_storageObj.scoresInfo);
        httpMethod.HttpSendAsync(pkg.Context, s_inf, EventType.ScoreEvent, OperateType.REVISE);
        await UniTask.Yield();
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        ScoreInfo info = JsonMapper.ToObject<ScoreInfo>(pkg.messPkg.ret);
        List<ScoreInfo> infs = await StorageHelper.DeleteInfo(StorageHelper.m_storageObj.scoresInfo, x => x.userName == info.userName 
                                && x.courseName == info.courseName && x.registerTime == info.registerTime && x.className == info.className);

        string s_inf = JsonMapper.ToJson(infs);
        httpMethod.HttpSendAsync(pkg.Context, s_inf, EventType.ScoreEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        ScoreInfo info = JsonMapper.ToObject<ScoreInfo>(pkg.messPkg.ret);
        bool isSearch = false;
        List<ScoreInfo> inf = new List<ScoreInfo>();
        foreach (var scoreInf in StorageHelper.m_storageObj.scoresInfo) {inf.Add(scoreInf.Clone());}
      
        if (info.className.Count() > 0) {inf = StorageHelper.SearchInf(inf, x => x.className == info.className); isSearch = true; }
        if (info.Name.Count() > 0) {inf = StorageHelper.SearchInf(inf, x => x.Name == info.Name); isSearch = true; }
        if (info.courseName.Count() > 0) {inf = StorageHelper.SearchInf(inf, x => x.courseName == info.courseName); isSearch = true; }
        if (info.registerTime.Count() > 0) {inf = StorageHelper.SearchInf(inf, x => x.registerTime == info.registerTime); isSearch = true; }
        if (isSearch == false) inf.Clear();
        
        string s_inf = JsonMapper.ToJson(inf);
        httpMethod.HttpSendAsync(pkg.Context, s_inf, EventType.ScoreEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }
}