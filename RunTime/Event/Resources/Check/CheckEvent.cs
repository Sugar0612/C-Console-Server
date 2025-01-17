using Cysharp.Threading.Tasks;
using HttpServer.Core;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Http;
using HttpServer.Frame.Storage;
using HttpServer.RunTime.Event;
using LitJson;
using static HttpServer.Core.CHttpServer;

public class CheckEvent : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg expand_pkg)
    {
        UpdatePackage up = JsonMapper.ToObject<UpdatePackage>(expand_pkg.messPkg.ret);
        string relativePath = up.relativePath;
        List<ResourcesInfo> bufInfo = new List<ResourcesInfo>();
        // Debug.Log($"relative Path : {relativePath}");
        foreach (ResourcesInfo inf in StorageHelper.m_storageObj.rsCheck)
        {
            ResourcesInfo info = new ResourcesInfo(inf);
            if (inf.relaPath.Contains(relativePath))
            {
                bufInfo.Add(info);
            }
        }

        foreach (ResourcesInfo inf in bufInfo)
        {
            // Debug.Log($"Download Inf: {inf.relaPath}");
            int i = up.filesInfo.FindIndex(x => x.relaPath == inf.relaPath);
            if (i >= 0 && i < up.filesInfo.Count)
                inf.need_updata = up.filesInfo[i].version_code != inf.version_code ? true : false;              
            else
                inf.need_updata = true;
        }
        up.filesInfo = bufInfo;
        // Debug.Log($"filesInfo Count: {up.filesInfo.Count}");
        string body = await JsonHelper.AsyncToJson(up);
        httpMethod.HttpSendAsync(expand_pkg.Context, body, EventType.CheckEvent, OperateType.NONE);
        await UniTask.Yield();
    }
}

public class UpdatePackage
{
    public string relativePath;
    public List<ResourcesInfo> filesInfo = new List<ResourcesInfo>();
}
