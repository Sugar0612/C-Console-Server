using Cysharp.Threading.Tasks;
using HttpServer.Frame.Helper;
using HttpServer.Frame.Http;
using HttpServer.Frame.Storage;
using HttpServer.RunTime.Event;
using LitJson;

public class ClassEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<ClassInfo> infs = await StorageHelper.GetInfo(StorageHelper.m_storageObj.classesInfo);

        Dictionary<string, int> classesNumber = new Dictionary<string, int>();
        foreach (var usr in StorageHelper.m_storageObj.usersInfo)
        {
            if (usr.Identity != "学生") continue;
            
            if (usr != null && !string.IsNullOrEmpty(usr.UnitName) && !classesNumber.ContainsKey(usr.UnitName))
            {
                classesNumber.Add(usr.UnitName, 0);
            }
            
            if (usr != null && !string.IsNullOrEmpty(usr.UnitName)) 
                classesNumber[usr.UnitName]++; 
        }

        foreach (var _class in infs)
        {
            _class.Number = classesNumber.ContainsKey(_class.Class) ? classesNumber[_class.Class] : 0;
        }

        string inf = JsonMapper.ToJson(infs);
        httpMethod.HttpSendAsync(pkg.Context, inf, EventType.ClassEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        ClassInfo info = JsonMapper.ToObject<ClassInfo>(pkg.messPkg.ret);
        List<ClassInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.m_storageObj.classesInfo, x => x.Class == info.Class);

        string body = JsonMapper.ToJson(new_list);
        httpMethod.HttpSendAsync(pkg.Context, body, EventType.ClassEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        ClassInfo info = JsonMapper.ToObject<ClassInfo>(pkg.messPkg.ret);
        List<ClassInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.m_storageObj.classesInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(inf);
        httpMethod.HttpSendAsync(pkg.Context, s_inf, EventType.ClassEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        ClassInfo info = JsonMapper.ToObject<ClassInfo>(pkg.messPkg.ret);
        List<ClassInfo> new_list = 
            await StorageHelper.DeleteInfo(StorageHelper.m_storageObj.classesInfo, (x) => {return x.id == info.id;});
        
        string body = JsonMapper.ToJson(new_list);
        httpMethod.HttpSendAsync(pkg.Context, body, EventType.ClassEvent, OperateType.DELETE);
    }
        
    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        ClassInfo info = JsonMapper.ToObject<ClassInfo>(pkg.messPkg.ret);
        List<ClassInfo> inf = StorageHelper.SearchInf(StorageHelper.m_storageObj.classesInfo, x => x.Class == info.Class);

        string s_inf = JsonMapper.ToJson(inf);
        httpMethod.HttpSendAsync(pkg.Context, s_inf, EventType.ClassEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }    
}