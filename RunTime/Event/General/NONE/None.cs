using Cysharp.Threading.Tasks;
using HttpServer.RunTime.Event;
using static HttpServer.Core.CHttpServer;

public class None : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg asynExPkg)
    {
        await UniTask.Yield();
    }
}