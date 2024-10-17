using Cysharp.Threading.Tasks;
using HttpServer.Core;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Storage;
using HttpServer.RunTime.Event;
using LitJson;
using static HttpServer.Core.CHttpServer;

public class RegisterEvent : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg asynExPkg)
    {
        await UniTask.Run(async () =>
        {
            UserInfo inf = JsonMapper.ToObject<UserInfo>(asynExPkg.messPkg.ret);
            inf = await StorageHelper.Register(inf);
            
            string s_inf = JsonMapper.ToJson(inf);
            CHttpServer.HttpSendAsync(asynExPkg.Context, s_inf, EventType.RegisterEvent, OperateType.NONE);
        });
    }
}
