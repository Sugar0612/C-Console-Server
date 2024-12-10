using HttpServer.Frame.Helper;
using HttpServer.Frame.Storage;
using HttpServer.Frame.Tools;
using LitJson;
using System.Net;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace HttpServer.Frame.Http
{
    public class HttpProcess
    {
        private HttpListenerContext m_HttpContext;
        private HttpMethod m_Method;

        public HttpProcess(HttpListenerContext context) 
        {
            m_HttpContext = context;
            m_Method = new HttpMethod();
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        public async void GetUserListAsync()
        {           
            List<UserInfo> usersList = await StorageHelper.GetInfo(StorageHelper.m_storageObj.usersInfo);
            string retContext = JsonMapper.ToJson(usersList);
            m_Method.NativeSend(m_HttpContext.Response, retContext);
        }

        /// <summary>
        /// 获取软件模块列表
        /// </summary>
        public async void GetSoftwareListAsync()
        {
            List<SoftwareInfo> softwareList = await StorageHelper.GetInfo(StorageHelper.m_storageObj.softwareInfo);
            string retContext = JsonMapper.ToJson(softwareList);
            m_Method.NativeSend(m_HttpContext.Response, retContext);
        }

        /// <summary>
        /// 登录请求
        /// </summary>
        public void Login()
        {
            string content = m_Method.GetRequestContent(m_HttpContext);
            UserInfo inf = JsonMapper.ToObject<UserInfo>(content);
            inf = StorageHelper.CheckUserLogin(inf);
            string s_inf = JsonMapper.ToJson(inf);
            m_Method.NativeSend(m_HttpContext.Response, s_inf);
        }

        /// <summary>
        /// 注册请求
        /// </summary>
        public void Register()
        {
            string content = m_Method.GetRequestContent(m_HttpContext);
            UserInfo inf = JsonMapper.ToObject<UserInfo>(content);
            inf = StorageHelper.CheckUserLogin(inf);
            string s_inf = JsonMapper.ToJson(inf);
            m_Method.NativeSend(m_HttpContext.Response, s_inf);
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        public void UserExit()
        {
            string content = m_Method.GetRequestContent(m_HttpContext);
            UserInfo inf = JsonMapper.ToObject<UserInfo>(content);
            inf = StorageHelper.ReviseInfoSingle(inf, StorageHelper.m_storageObj.usersInfo, x => x.userName == inf.userName);
            string s_inf = JsonMapper.ToJson(inf);
            m_Method.NativeSend(m_HttpContext.Response, s_inf);
        }

        /// <summary>
        /// 人次增加请求
        /// </summary>
        public void AddNumOfPeople()
        {
            string content = m_Method.GetRequestContent(m_HttpContext);
            NumOfPeopleInfo inf = JsonMapper.ToObject<NumOfPeopleInfo>(content);
            string s_inf = "";

            int index = StorageHelper.m_storageObj.numOfPeoInfo.FindIndex(x => x.moduleName == inf.moduleName);
            if (index != -1) {
                StorageHelper.m_storageObj.numOfPeoInfo[index].count += 1; 
                s_inf = JsonMapper.ToJson(StorageHelper.m_storageObj.numOfPeoInfo[index]);
            }
            else {
                NumOfPeopleInfo nopInfo = new NumOfPeopleInfo();
                nopInfo.moduleName = inf.moduleName;
                nopInfo.count = 1;
                StorageHelper.m_storageObj.numOfPeoInfo.Add(nopInfo);
                s_inf = JsonMapper.ToJson(nopInfo);
            }
            StorageMethod.Memory2HardDisk(FPath.STORAGE_MCNT, StorageHelper.m_storageObj.numOfPeoInfo);
            m_Method.NativeSend(m_HttpContext.Response, s_inf);
        }

        /// <summary>
        /// 人次获取请求
        /// </summary>
        public async void GetNumOfPeopleListAsync()
        {
            List<NumOfPeopleInfo> uopList = await StorageHelper.GetInfo(StorageHelper.m_storageObj.numOfPeoInfo);
            string retContext = JsonMapper.ToJson(uopList);
            m_Method.NativeSend(m_HttpContext.Response, retContext);
        }

        /// <summary>
        /// 不同模块时长统计
        /// </summary>
        public void AddUsrTime()
        {
            string content = m_Method.GetRequestContent(m_HttpContext);
            UsrTimeInfo inf = JsonMapper.ToObject<UsrTimeInfo>(content);
            string s_inf = "";

            int index = StorageHelper.m_storageObj.usrTimeInfo.FindIndex(x => x.moduleName == inf.moduleName && x.usrName == inf.usrName);
            if (index != -1)
            {
                StorageHelper.m_storageObj.usrTimeInfo[index].min += 1;
                s_inf = JsonMapper.ToJson(StorageHelper.m_storageObj.usrTimeInfo[index]);
            }
            else
            {
                UsrTimeInfo usrTimeInfo = new UsrTimeInfo();
                usrTimeInfo.usrName = inf.usrName;
                usrTimeInfo.moduleName = inf.moduleName;
                usrTimeInfo.min = 1;
                StorageHelper.m_storageObj.usrTimeInfo.Add(usrTimeInfo);
                s_inf = JsonMapper.ToJson(usrTimeInfo);
            }
            StorageMethod.Memory2HardDisk(FPath.STORAGE_USTIME, StorageHelper.m_storageObj.usrTimeInfo);
            m_Method.NativeSend(m_HttpContext.Response, s_inf);
        }

        /// <summary>
        /// 获取每个模块的时长列表
        /// </summary>
        public async void GetUsrTimeListAsync()
        {
            List<UsrTimeInfo> utList = await StorageHelper.GetInfo(StorageHelper.m_storageObj.usrTimeInfo);
            string retContext = JsonMapper.ToJson(utList);
            m_Method.NativeSend(m_HttpContext.Response, retContext);
        }

        /// <summary>
        /// 获取每个模块的总时长
        /// </summary>
        public void GetModulesTimeList()
        {
            Dictionary<string, long> modulesDic = new Dictionary<string, long>();
            foreach (var software in StorageHelper.m_storageObj.softwareInfo) { modulesDic.Add(software.Name, 0); }
            foreach (var inf in StorageHelper.m_storageObj.usrTimeInfo) { if (modulesDic.ContainsKey(inf.moduleName)) { modulesDic[inf.moduleName] += inf.min; } }
            string jsStr = JsonMapper.ToJson(modulesDic);
            m_Method.NativeSend(m_HttpContext.Response, jsStr);
        }

        public void DeleteUsrTime()
        {
            string content = m_Method.GetRequestContent(m_HttpContext);
            UsrTimeInfo info = JsonMapper.ToObject<UsrTimeInfo>(content);
            
            int i = StorageHelper.m_storageObj.usrTimeInfo.FindIndex(x => x.usrName == info.usrName && x.moduleName == info.moduleName);
            if (i != -1) StorageHelper.m_storageObj.usrTimeInfo.RemoveAt(i);

            StorageMethod.Memory2HardDisk(FPath.STORAGE_USTIME, StorageHelper.m_storageObj.usrTimeInfo);
            GetUsrTimeListAsync();
        }

        /// <summary>
        /// 其他 固定的规则请求处理
        /// </summary>
        /// <param name="actionName"></param>
        public void Other(string actionName)
        {
            if (actionName == "OPTIONS") { m_Method.OptionsRequestProcess(m_HttpContext); }
            else if (actionName == "POST") { m_Method.PostRequestProcess(m_HttpContext); }
            else if (actionName == "GET") { m_Method.GetRequestProcess(m_HttpContext); }
        }
    }
}
