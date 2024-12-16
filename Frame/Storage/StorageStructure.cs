namespace HttpServer.Frame.Storage
{
    /// <summary>
    /// 客户端资源更新前会进行检查请求
    /// 如果客户端的版本码和服务器的不一致则需要更新
    /// </summary>
    [Serializable]
    public class ResourcesInfo : BaseInfo
    {
        public string relaPath; // 相对路径 StreamingAsset/...
        public string version_code; // 版本号
        public bool need_updata; // 是否需要同步更新

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
        public string id; // ID
        public string Name; // 名称
        public string RegisterTime; // 注册时间
        public string TeacherName; // 管理学院人名
    }

    /// <summary>
    /// 用户信息包
    /// </summary>
    [Serializable]
    public class UserInfo : BaseInfo
    {
        public string userName; // 用户名
        public string password; // 密码
        public string Name; // 真实姓名
        public string Gender; // 性别
        public string Age;  // 年龄
        public string Identity; // 身份【学生、教师、管理员】 
        public string idCoder;  // 身份证
        public string Contact;  // 联系方式
        public string UnitName; // 所属单位【班级、专业、学院】
        public bool login = false; // 账号是否激活可登录使用
        public string hint = ""; // 提示信息【登录成功、登录失败等等】
    }

    /// <summary>
    ///  专业信息包
    /// </summary>
    [Serializable]
    public class MajorInfo : BaseInfo
    {
        public string id;  // ID
        public string MajorName; // 专业名称
        public string RegisterTime; // 注册时间
        public string FacultyName; // 所属学院
        public string TeacherName; // 管理老师姓名
    }

    /// <summary>
    ///  班级信息包
    /// </summary>
    [Serializable]
    public class ClassInfo : BaseInfo
    {
        public string id; // ID
        public string Class; // 班级名称
        public string RegisterTime; // 注册时间
        public string Faculty; // 所属学院
        public string Major;  // 所属专业
        public string Teacher; // 管理的老师
        public int Number; // 人数
    }

    /// <summary>
    ///  栏目信息包
    /// </summary>
    [Serializable]
    public class ColumnsInfo : BaseInfo
    {
        public string id; // ID
        public string Name; // 栏目名称
        public string RegisterTime; // 注册时间
    }

    /// <summary>
    ///  课程信息包
    /// </summary>
    [Serializable]
    public class CourseInfo : BaseInfo
    {
        public string id; // ID
        public string CourseName; // 课程名称
        public string Columns; // 所属栏目
        public string Working; // 课程介绍（目前没有使用）
        public string RegisterTime; // 注册时间
    }

    /// <summary>
    ///  考核信息包
    /// </summary>
    [Serializable]
    public class ExamineInfo : BaseInfo
    {
        public string id; // ID
        public string ColumnsName; // 所属栏目
        public string CourseName; // 所属课程
        public string RegisterTime; // 注册时间
        public string TrainingScore; // 设置的实训考核总分数
        public string TheoryTime = "5"; // 理论考核时间（分钟单位）
        public string TrainingTime = "5"; // 实训考核时间（分钟单位）
        public int PNum; // 考核人数
        public int SingleNum; // 单选题数目
        public int MulitNum; // 多选题数目
        public int TOFNum; // 判断题数目
        public bool Status = false; // 是否激活显示在客户端
        public List<SingleChoice> SingleChoices = new List<SingleChoice>(); // 单选题目列表
        public List<MulitChoice> MulitChoices = new List<MulitChoice>(); // 多选题目列表
        public List<TOFChoice> TOFChoices = new List<TOFChoice>(); // 判断题目列表

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
        public string Topic; // 题目内容
        public ItemChoice toA = new ItemChoice();  // A
        public ItemChoice toB = new ItemChoice(); //B
        public ItemChoice toC = new ItemChoice(); //C
        public ItemChoice toD = new ItemChoice(); //D
        public string Answer; // 答案
        public string Score = ""; // 分数

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
        public string Topic; // 题目
        // 选项，因为选项不唯一可能 4个可能6个，所以用List存储
        public List<MulitChoiceItem> Options = new List<MulitChoiceItem>(); // {{"A", "xxxxx", true}, {"B", "xxxxxxx", false}}，
        public string Answer;// 答案
        public string Score = "";// 分数

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
        public string Topic; //题目
        public ItemChoice toA = new ItemChoice(); // A
        public ItemChoice toB = new ItemChoice(); // B
        public string Answer; //答案
        public string Score = "";//分数

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
        public string className; // 所属班级
        public string columnsName;  // 所属栏目
        public string courseName;   // 所属课程
        public string registerTime; // 该次考试的注册时间
        public string userName; // 考试用户名
        public string Name; // 考试人真实姓名
        public string theoryScore;  // 理论分数
        public string trainingScore; // 实训分数
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
        public string moduleName = ""; // 模块名称
        public long count = 0; // 该模块使用人次
    }

    /// <summary>
    /// 时长统计
    /// </summary>
    public class UsrTimeInfo : BaseInfo
    {
        public string usrName = ""; // 用户名
        public string moduleName = ""; // 模块名称
        public long min = 0; // 使用时间(分钟)
    }
}
