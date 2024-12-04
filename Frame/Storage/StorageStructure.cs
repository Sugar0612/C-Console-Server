namespace HttpServer.Frame.Storage
{
    /// <summary>
    /// 客户端资源更新前会进行检查请求
    /// 如果客户端的版本码和服务器的不一致则需要更新
    /// </summary>
    [Serializable]
    public class ResourcesInfo : BaseInfo
    {
        public string relaPath;
        public string version_code;
        public bool need_updata;

        public ResourcesInfo() { }

        public ResourcesInfo(ResourcesInfo clone)
        {
            relaPath = clone.relaPath;
            version_code = clone.version_code;
            need_updata = clone.need_updata;
        }
    }

    public class BaseInfo { }

    /// <summary>
    ///  学院信息包
    /// </summary>
    [Serializable]
    public class FacultyInfo : BaseInfo
    {
        public string id;
        public string Name;
        public string RegisterTime;
        public string TeacherName;
    }

    /// <summary>
    /// 用户信息包
    /// </summary>
    [Serializable]
    public class UserInfo : BaseInfo
    {
        public string userName;
        public string password;
        public string Name;
        public string Gender;
        public string Age;
        public string Identity;
        public string idCoder;
        public string Contact;
        public string UnitName;
        public bool login = false; // 账号是否激活可登录使用
        public string hint = "";
    }

    /// <summary>
    ///  专业信息包
    /// </summary>
    [Serializable]
    public class MajorInfo : BaseInfo
    {
        public string id;
        public string MajorName;
        public string RegisterTime;
        public string FacultyName;
        public string TeacherName;
    }

    /// <summary>
    ///  班级信息包
    /// </summary>
    [Serializable]
    public class ClassInfo : BaseInfo
    {
        public string id;
        public string Class;
        public string RegisterTime;
        public string Faculty;
        public string Major;
        public string Teacher;
        public int Number;
    }

    /// <summary>
    ///  栏目信息包
    /// </summary>
    [Serializable]
    public class ColumnsInfo : BaseInfo
    {
        public string id;
        public string Name;
        public string RegisterTime;
    }

    /// <summary>
    ///  课程信息包
    /// </summary>
    [Serializable]
    public class CourseInfo : BaseInfo
    {
        public string id;
        public string CourseName;
        public string Columns;
        public string Working;
        public string RegisterTime;
    }

    /// <summary>
    ///  考核信息包
    /// </summary>
    [Serializable]
    public class ExamineInfo : BaseInfo
    {
        public string id;
        public string ColumnsName;
        public string CourseName;
        public string RegisterTime;
        public string TrainingScore;
        public string TheoryTime = "5"; // 分钟
        public string TrainingTime = "5"; // 分钟   
        public int PNum;
        public int SingleNum;
        public int MulitNum;
        public int TOFNum;
        public bool Status = false;
        public List<SingleChoice> SingleChoices = new List<SingleChoice>();
        public List<MulitChoice> MulitChoices = new List<MulitChoice>();
        public List<TOFChoice> TOFChoices = new List<TOFChoice>();

        public ExamineInfo() { }
        public ExamineInfo Clone()
        {
            ExamineInfo inf = new ExamineInfo();
            inf.id = id;
            inf.ColumnsName = ColumnsName;
            inf.CourseName = CourseName;
            inf.RegisterTime = RegisterTime;
            inf.TrainingScore = TrainingScore;
            inf.PNum = PNum;
            inf.SingleNum = SingleNum;
            inf.MulitNum = MulitNum;
            inf.TOFNum = TOFNum;
            inf.TheoryTime = TheoryTime;
            inf.TrainingTime = TrainingTime;
            inf.Status = Status;
            foreach (var Option in SingleChoices) { inf.SingleChoices.Add(Option.Clone()); }
            foreach (var Option in MulitChoices) { inf.MulitChoices.Add(Option.Clone()); }
            foreach (var Option in TOFChoices) { inf.TOFChoices.Add(Option.Clone()); }
            return inf;
        }
    }

    /// <summary>
    /// 单选题包
    /// </summary>
    [Serializable]
    public class SingleChoice
    {
        public string Topic;
        public ItemChoice toA = new ItemChoice();
        public ItemChoice toB = new ItemChoice();
        public ItemChoice toC = new ItemChoice();
        public ItemChoice toD = new ItemChoice();
        public string Answer;
        public string Score = "";

        public SingleChoice Clone()
        {
            SingleChoice single = new SingleChoice();
            single.Topic = Topic;
            single.toA = toA.Clone();
            single.toB = toB.Clone();
            single.toC = toB.Clone();
            single.toD = toB.Clone();
            single.Answer = Answer;
            single.Score = Score;
            return single;
        }
    }

    /// <summary>
    /// 多选
    /// </summary>
    [Serializable]
    public class MulitChoice
    {
        public string Topic;
        public List<MulitChoiceItem> Options = new List<MulitChoiceItem>(); // {{"A", "xxxxx", true}, {"B", "xxxxxxx", false}}
        public string Answer;
        public string Score = "";

        public MulitChoice Clone()
        {
            MulitChoice mulit = new MulitChoice();
            mulit.Topic = Topic;
            foreach (var Option in Options) { mulit.Options.Add(Option.Clone()); }
            mulit.Answer = Answer;
            mulit.Score = Score;
            return mulit;
        }
    }

    /// <summary>
    /// 判断题
    /// </summary>
    [Serializable]
    public class TOFChoice
    {
        public string Topic;
        public ItemChoice toA = new ItemChoice();
        public ItemChoice toB = new ItemChoice();
        public string Answer;
        public string Score = "";

        public TOFChoice Clone()
        {
            TOFChoice tof = new TOFChoice();
            tof.Topic = Topic;
            tof.toA = toA.Clone();
            tof.toB = toB.Clone();
            tof.Answer = Answer;
            tof.Score = Score;
            return tof;
        }
    }

    /// <summary>
    /// 理论模式中 一个选项的信息
    /// </summary>
    [Serializable]
    public class ItemChoice
    {
        public string m_content = "";
        public bool m_isOn = false;

        public ItemChoice() { }

        public ItemChoice(string content, bool ison)
        {
            m_content = content;
            m_isOn = ison;
        }

        public ItemChoice Clone()
        {
            ItemChoice item = new ItemChoice();
            item.m_content = m_content;
            item.m_isOn = m_isOn;
            return item;
        }
    }

    /// <summary>
    /// 因为服务器端没有办法 序列化 字典类型，所以为了保存多选题的选项，需要自定义一个类
    /// </summary>
    [Serializable]
    public class MulitChoiceItem
    {
        public string Serial = "A";
        public string Content = "";
        public bool isOn = false;

        public MulitChoiceItem() { }

        public MulitChoiceItem Clone()
        {
            MulitChoiceItem item = new MulitChoiceItem();
            item.Serial = Serial;
            item.Content = Content;
            item.isOn = isOn;
            return item;
        }
    }

    public class BaseChoice { }

    /// <summary>
    /// 成绩管理信息
    /// </summary>
    [Serializable]
    public class ScoreInfo : BaseInfo
    {
        public string className;
        public string columnsName;
        public string courseName;
        public string registerTime; // 该次考试的注册时间
        public string userName;
        public string Name;
        public string theoryScore;
        public string trainingScore;
        public bool theoryFinished; //本次理论考试是否完成
        public bool trainingFinished; //本次实训考试是否完成

        public ScoreInfo Clone()
        {
            ScoreInfo inf = new ScoreInfo()
            {
                className = className,
                columnsName = columnsName,
                courseName = courseName,
                registerTime = registerTime,
                userName = userName,
                Name = Name,
                theoryScore = theoryScore,
                trainingScore = trainingScore,
                theoryFinished = theoryFinished,
                trainingFinished = trainingFinished,
            };
            return inf;
        }
    }

    /// <summary>
    /// 软件信息
    /// </summary>
    public class SoftwareInfo : BaseInfo
    {
        public string id = ""; // 项目唯一id
        public string Name = ""; // 软件名
        public string Cover = ""; // 封面路径
        public string QRcode = ""; // 二维码路径
        public string ProjPath = ""; // 软件路径
    }

    /// <summary>
    /// 人次统计
    /// </summary>
    public class NumOfPeopleInfo : BaseInfo
    {
        public string moduleName = "";
        public long count = 0;
    }

    /// <summary>
    /// 时长统计
    /// </summary>
    public class UsrTimeInfo : BaseInfo
    {
        public string usrName = "";
        public string moduleName = "";
        public long min = 0;
    }
}
