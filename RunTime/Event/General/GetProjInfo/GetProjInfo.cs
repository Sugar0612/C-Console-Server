using Cysharp.Threading.Tasks;
using HttpServer.Core;
using HttpServer.Frame.Helper;
using HttpServer.RunTime.Event;
using LitJson;
using static HttpServer.Core.CHttpServer;

public class GetProjInfo : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg asynExPkg)
    {
        await UniTask.Yield();

        List<Proj> projs = new List<Proj>();
        foreach (var item in StorageHelper.m_storageObj.columnsInfo)
        {
            Proj proj = new Proj
            {
                Columns = item.Name
            };

            List<string> courses = new List<string>();
            foreach (var course in StorageHelper.m_storageObj.courseInfo)
            {
                if (course.Columns == item.Name)
                {
                    courses.Add(course.CourseName);
                }
            }
            proj.Courses = courses;
            projs.Add(proj);
        }

        string inf = JsonMapper.ToJson(projs);
        CHttpServer.HttpSendAsync(asynExPkg.Context, inf, EventType.GetProjInfo, OperateType.NONE);
    }
}

public struct Proj
{
    public string Columns; //项目名字
    public List<string> Courses; // 子项目列表姓名
}