using HttpServer.Frame.Helper;
using HttpServer.Frame.Tools;
using LitJson;

namespace HttpServer.Frame.Storage
{
    internal class StorageObject : StorageMethod
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
        public List<SoftwareInfo> softwareInfo = new List<SoftwareInfo>();
        public List<NumOfPeopleInfo> numOfPeoInfo = new List<NumOfPeopleInfo>();
        public List<UsrTimeInfo> usrTimeInfo = new List<UsrTimeInfo>();

        public void Init()
        {
            HardDisk2Memory(FPath.STORAGE_USER, out usersInfo);
            HardDisk2Memory(FPath.STORAGE_RESOURCE, out rsCheck);
            HardDisk2Memory(FPath.STORAGE_FACULTY, out faculiesInfo);
            HardDisk2Memory(FPath.STORAGE_MAJOR, out majorInfo);
            HardDisk2Memory(FPath.STORAGE_CLASS, out classesInfo);
            HardDisk2Memory(FPath.STORAGE_COLUMNS, out columnsInfo);
            HardDisk2Memory(FPath.STORAGE_COURSE, out courseInfo);
            HardDisk2Memory(FPath.STORAGE_EXAMINE, out examineesInfo);
            HardDisk2Memory(FPath.STORAGE_SCORE, out scoresInfo);
            HardDisk2Memory(FPath.STORAGE_SOFTWARE, out softwareInfo);
            HardDisk2Memory(FPath.STORAGE_MCNT, out numOfPeoInfo);
            HardDisk2Memory(FPath.STORAGE_USTIME, out usrTimeInfo);
        }

        public void Save()
        {
            Memory2HardDisk(FPath.STORAGE_USER, usersInfo);
            Memory2HardDisk(FPath.STORAGE_RESOURCE, rsCheck);
            Memory2HardDisk(FPath.STORAGE_FACULTY, faculiesInfo);
            Memory2HardDisk(FPath.STORAGE_MAJOR, majorInfo);
            Memory2HardDisk(FPath.STORAGE_CLASS, classesInfo);
            Memory2HardDisk(FPath.STORAGE_COLUMNS, columnsInfo);
            Memory2HardDisk(FPath.STORAGE_COURSE, courseInfo);
            Memory2HardDisk(FPath.STORAGE_EXAMINE, examineesInfo);
            Memory2HardDisk(FPath.STORAGE_SCORE, scoresInfo);
            Memory2HardDisk(FPath.STORAGE_SOFTWARE, softwareInfo);
            Memory2HardDisk(FPath.STORAGE_MCNT, numOfPeoInfo);
            Memory2HardDisk(FPath.STORAGE_USTIME, usrTimeInfo);
        }
    }
}
