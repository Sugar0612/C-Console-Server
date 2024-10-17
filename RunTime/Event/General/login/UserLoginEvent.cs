using HttpServer.Core;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Storage;
using HttpServer.RunTime.Event;
using LitJson;
using static HttpServer.Core.CHttpServer;

public class UserLoginEvent : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg asynExPkg)
    {
        UserInfo inf = JsonMapper.ToObject<UserInfo>(asynExPkg.messPkg.ret);
        inf = StorageHelper.CheckUserLogin(inf);
        string s_inf = JsonMapper.ToJson(inf);
        CHttpServer.HttpSendAsync(asynExPkg.Context, s_inf, EventType.UserLoginEvent, OperateType.NONE);
    }
}