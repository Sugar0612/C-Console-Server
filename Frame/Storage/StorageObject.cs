using HttpServer.Frame.Helper;
using HttpServer.Frame.Tools;
using LitJson;

namespace HttpServer.Frame.Storage
{
    internal class StorageObject
    {
        public List<ResourcesInfo> rsCheck = new List<ResourcesInfo>();
        public List<UserInfo> usersInfo = new List<UserInfo>();
        public List<FacultyInfo> faculiesInfo = new List<FacultyInfo>();
        public List<MajorInfo> majorInfo = new List<MajorInfo>();
        public List<ClassInfo> classesInfo = new List<ClassInfo>();
        public List<ColumnsInfo> columnsInfo = new List<ColumnsInfo>();
        public List<CourseInfo> courseInfo = new List<CourseInfo>();
        public List<ExamineInfo> examineesInfo = new List<ExamineInfo>();
        public List<ScoreInfo> scoresInfo = new List<ScoreInfo>();

        public void Init()
        {
            HardDisk2Memory(PathAPI.STORAGE_USER, out usersInfo);
            HardDisk2Memory(PathAPI.STORAGE_RESOURCE, out rsCheck);
            HardDisk2Memory(PathAPI.STORAGE_FACULTY, out faculiesInfo);
            HardDisk2Memory(PathAPI.STORAGE_MAJOR, out majorInfo);
            HardDisk2Memory(PathAPI.STORAGE_CLASS, out classesInfo);
            HardDisk2Memory(PathAPI.STORAGE_COLUMNS, out columnsInfo);
            HardDisk2Memory(PathAPI.STORAGE_COURSE, out courseInfo);
            HardDisk2Memory(PathAPI.STORAGE_EXAMINE, out examineesInfo);
            HardDisk2Memory(PathAPI.STORAGE_SCORE, out scoresInfo);
            Memory2HardDisk();
        }

        private void HardDisk2Memory<T>(string filePath, out List<T> targetList)
        {
            string jsonString = FileHelper.ReadTextFile(filePath);
            targetList = JsonMapper.ToObject<List<T>>(jsonString);
        }

        public void Memory2HardDisk()
        {
            string json = JsonMapper.ToJson(usersInfo);
            
            FileHelper.WriteTextFile($"{System.AppDomain.CurrentDomain.BaseDirectory}/UsersInfo.json", json);
        }
    }
}
