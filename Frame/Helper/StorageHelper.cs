using Cysharp.Threading.Tasks;
using HttpServer.Frame.Storage;
using HttpServer.Frame.Tools;

namespace HttpServer.Frame.Helper
{
    internal class StorageHelper
    {
        public static StorageObject m_storageObj = new StorageObject();

        public void LoadHardDisk2Memory()
        {
            m_storageObj = new StorageObject();
            m_storageObj.Init();
        }

        public void SaveHardDisk()
        {
            m_storageObj.Save();
        }

        /// <summary>
        /// 检查客户端的资源版本是否时最新的
        /// </summary>
        /// <param name="cli_info"></param>
        public static ResourcesInfo GetThisInfoPkg(ResourcesInfo cli_info)
        {
            return m_storageObj.rsCheck.Find((x) =>
            {
                return (x.relaPath == cli_info.relaPath);
            });
        }

        /// <summary>
        /// 保存这个文件的版本信息
        /// </summary>
        /// <param name="relative"></param>
        public static async void UpdateThisFileInfo(string relative)
        {
            string[] st = relative.Split("\\");
            string id = st[0];
            string moudleName = st[1];

            int idx = m_storageObj.rsCheck.FindIndex((x) => { return x.relaPath == relative; });
            if (idx != -1)
            {
                m_storageObj.rsCheck.RemoveAt(idx);
            }

            ResourcesInfo ri = new ResourcesInfo();
            ri.relaPath = relative;
            ri.version_code = Tools.Tools.SpawnRandomCode();
            m_storageObj.rsCheck.Add(ri);
            m_storageObj.Save();
        }


        /// <summary>
        /// 检查用户登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static UserInfo CheckUserLogin(UserInfo info)
        {
            UserInfo usrInfo = info;
            int account_idx = m_storageObj.usersInfo.FindIndex(x => x.userName == info.userName);
            if (account_idx != -1)
            {
                int pwd_idx = m_storageObj.usersInfo.FindIndex(x => x.userName == info.userName && x.password == info.password);
                if (pwd_idx != -1 && m_storageObj.usersInfo[pwd_idx].login == true)
                {
                    usrInfo.Name = m_storageObj.usersInfo[pwd_idx].Name;
                    usrInfo.Gender = m_storageObj.usersInfo[pwd_idx].Gender;
                    usrInfo.Age = m_storageObj.usersInfo[pwd_idx].Age;
                    usrInfo.Identity = m_storageObj.usersInfo[pwd_idx].Identity;
                    usrInfo.idCoder = m_storageObj.usersInfo[pwd_idx].idCoder;
                    usrInfo.Contact = m_storageObj.usersInfo[pwd_idx].Contact;
                    usrInfo.UnitName = m_storageObj.usersInfo[pwd_idx].UnitName;
                    usrInfo.hint = "登录成功";
                }
                else if (pwd_idx != -1 && m_storageObj.usersInfo[pwd_idx].login == false)
                {
                    usrInfo.hint = "账号未激活";
                }
                else
                {
                    usrInfo.hint = "密码错误";
                }
            }
            else
            {
                usrInfo.hint = "用户名不存在";
            }
            return usrInfo;
        }

        /// <summary>
        /// 注册请求
        /// </summary>
        /// <param name="inf"></param>
        /// <returns></returns>
        public async static UniTask<UserInfo> Register(UserInfo inf)
        {
            if (m_storageObj.usersInfo.Find(x => x.userName == inf.userName) == null)
            {
                inf.hint = "注册成功!";
                inf.login = false;
                m_storageObj.usersInfo.Add(inf);
                m_storageObj.Save();
            }
            else
            {
                //如果注册失败清空inf中的数据
                inf.userName = "";
                inf.password = "";
                inf.hint = "该用户名存在!";
            }

            return inf;
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async UniTask<List<T>> GetInfo<T>(List<T> storInfo) where T : BaseInfo
        {
            List<T> info = new List<T>();

            foreach (T inf in storInfo)
            {
                info.Add(inf);
            }
            return info;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static async UniTask<List<T>> AddInfo<T>(T inf, List<T> storInfo, Predicate<T> match = default) where T : BaseInfo
        {
            if (storInfo.Find(match) == null)
            {
                storInfo.Add(inf);
            }
            m_storageObj.Save();
            return storInfo;
        }

        public static async UniTask<List<T>> AddInfo<T>(List<T> l_inf, List<T> storInfo) where T : UserInfo
        {
            foreach (var inf in l_inf)
            {
                await Register(inf);
            }
            m_storageObj.Save();
            return storInfo;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static async UniTask<List<T>> ReviseInfo<T>(T inf, List<T> storInfo, Predicate<T> match) where T : BaseInfo
        {
            int index = storInfo.FindIndex(match);
            Console.WriteLine($"reviseInfo: {index}");
            if (index != -1)
            {
                storInfo[index] = inf;
                return storInfo;
            }
            return new List<T>();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ReviseInfoSingle<T>(T inf, List<T> storInfo, Predicate<T> match) where T : BaseInfo, new()
        {
            int index = storInfo.FindIndex(match);
            if (index != -1)
            {
                storInfo[index] = inf;
                m_storageObj.Save();
                return storInfo[index];
            }
            return new T();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static async UniTask<List<T>> DeleteInfo<T>(List<T> storInfo, Predicate<T> match) where T : BaseInfo
        {
            int idx = storInfo.FindIndex(match);
            if (idx != -1)
            {
                storInfo.RemoveAt(idx);
                return storInfo;
            }
            m_storageObj.Save();
            return new List<T>();
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storInf"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static List<T> SearchInf<T>(List<T> storInf, Predicate<T> match) where T : BaseInfo
        {
            List<T> list = new List<T>();
            list = storInf.FindAll(match);
            return list;
        }
    }
}
