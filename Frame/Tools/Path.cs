namespace HttpServer.Frame.Tools
{
    internal class PathAPI
    {
        public static string STORAGE_ROOT_PATH = AppDomain.CurrentDomain.BaseDirectory; // Root 路径

        public static string STORAGE_USER = $"{STORAGE_ROOT_PATH}\\UsersInfo.json";
        public static string STORAGE_RESOURCE = $"{STORAGE_ROOT_PATH}\\rsCheck.json";
        public static string STORAGE_FACULTY = $"{STORAGE_ROOT_PATH}\\FacultyInfo.json";
        public static string STORAGE_MAJOR = $"{STORAGE_ROOT_PATH}\\MajorInfo.json";
        public static string STORAGE_CLASS = $"{STORAGE_ROOT_PATH}\\ClassInfo.json";
        public static string STORAGE_COLUMNS = $"{STORAGE_ROOT_PATH}\\ColumnsInfo.json";
        public static string STORAGE_COURSE = $"{STORAGE_ROOT_PATH}\\CourseInfo.json";
        public static string STORAGE_EXAMINE = $"{STORAGE_ROOT_PATH}\\ExamineInfo.json";
        public static string STORAGE_SCORE = $"{STORAGE_ROOT_PATH}\\ScoreInfo.json"; 
    }
}
