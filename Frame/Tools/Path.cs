namespace HttpServer.Frame.Tools
{
    internal class FPath
    {
        public static string STORAGE_ROOT_PATH = AppDomain.CurrentDomain.BaseDirectory; // Root 路径

        public static string STORAGE_USER = $"{STORAGE_ROOT_PATH}\\Data\\UsersInfo.json";
        public static string STORAGE_RESOURCE = $"{STORAGE_ROOT_PATH}\\Data\\rsCheck.json";
        public static string STORAGE_FACULTY = $"{STORAGE_ROOT_PATH}\\Data\\FacultyInfo.json";
        public static string STORAGE_MAJOR = $"{STORAGE_ROOT_PATH}\\Data\\MajorInfo.json";
        public static string STORAGE_CLASS = $"{STORAGE_ROOT_PATH}\\Data\\ClassInfo.json";
        public static string STORAGE_COLUMNS = $"{STORAGE_ROOT_PATH}\\Data\\ColumnsInfo.json";
        public static string STORAGE_COURSE = $"{STORAGE_ROOT_PATH}\\Data\\CourseInfo.json";
        public static string STORAGE_EXAMINE = $"{STORAGE_ROOT_PATH}\\Data\\ExamineInfo.json";
        public static string STORAGE_SCORE = $"{STORAGE_ROOT_PATH}\\Data\\ScoreInfo.json";
        public static string STORAGE_SOFTWARE = $"{STORAGE_ROOT_PATH}\\Data\\Software.json";
        public static string IP = $"{STORAGE_ROOT_PATH}\\Data\\IP.txt";
    }
}
