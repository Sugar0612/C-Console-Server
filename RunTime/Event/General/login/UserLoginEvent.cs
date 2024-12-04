using HttpServer.Core;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Http;
using HttpServer.Frame.Storage;
using HttpServer.RunTime.Event;
using LitJson;

public class UserLoginEvent : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg asynExPkg)
    {
        UserInfo inf = JsonMapper.ToObject<UserInfo>(asynExPkg.messPkg.ret);
        inf = StorageHelper.CheckUserLogin(inf);
        string s_inf = JsonMapper.ToJson(inf);
        httpMethod.HttpSendAsync(asynExPkg.Context, s_inf, EventType.UserLoginEvent, OperateType.NONE);
    }
}