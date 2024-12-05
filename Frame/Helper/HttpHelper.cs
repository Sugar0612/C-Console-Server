using HttpServer.Core;
using HttpServer.Frame.Http;
using System.Net;

namespace HttpServer.Frame.Helper
{
    public class HttpHelper
    {
        public void RequestProcess(string actionName, string methodName, HttpListenerContext context)
        {
            string _log = $"RequestProess actionName: {actionName}, methodName: {methodName}\n";
            Console.Write(_log);

            HttpProcess process = new HttpProcess(context);
            if (actionName == "/getUserList" && methodName == "POST") { process.GetUserListAsync(); }
            else if (actionName == "/getSoftwareList" && methodName == "POST") { process.GetSoftwareListAsync(); }
            else if (actionName == "/Login" && methodName == "POST") { process.Login(); }
            else if (actionName == "/Register" && methodName == "POST") { process.Register(); }
            else if (actionName == "/UserExit" && methodName == "POST") { process.UserExit(); }
            else if (actionName == "/NumOfPeople" && methodName == "POST") { process.AddNumOfPeople(); }
            else if (actionName == "/GetNumOfPeopleList" && methodName == "GET") { process.GetNumOfPeopleListAsync(); }
            else if (actionName == "/UsrTime" && methodName == "POST") { process.AddUsrTime(); }
            else if (actionName == "/GetUsrTimeList" && methodName == "GET") { process.GetUsrTimeListAsync(); }
            else if (actionName == "/GetModulesTimeList" && methodName == "GET") { process.GetModulesTimeList(); }
            else { process.Other(methodName); }
        }
    }
}