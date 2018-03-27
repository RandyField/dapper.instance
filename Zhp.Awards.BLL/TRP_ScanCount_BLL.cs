using Common.Helper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhp.Awards.DAL;
using Zhp.Awards.IDAL;
using Zhp.Awards.Model;

namespace Zhp.Awards.BLL
{
    public class TRP_ScanCount_BLL
    {

        #region 单例模式

        /// <summary>
        /// 定义私有变量记录单例类的唯一实例
        /// </summary>
        private static TRP_ScanCount_BLL instance;

        /// <summary>
        /// 线程锁
        /// </summary>
        private static readonly object asyncLock = new object();

        /// <summary>
        /// 私有构造函数，该类无法被实例化
        /// </summary>
        private TRP_ScanCount_BLL() { }

        /// <summary>
        /// 公用方法getInstance()提供该类实例的唯一全局访问点
        /// </summary>
        /// <returns></returns>
        public static TRP_ScanCount_BLL getInstance()
        {
            //如果实例不存在，则new一个新实例，否则返回已有实例
            if (instance == null)
            {
                instance = new TRP_ScanCount_BLL();
            }
            return instance;
        }

        private IBaseDAL idal = DbFactory.Create();

        #endregion

        /// <summary>
        /// 红包扫码计数
        /// </summary>
        /// <param name="activityid"></param>
        /// <param name="msg"></param>
        /// <param name="activityname"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool QrScanCount(string activityid, ref string msg, string activityname = "undefine ActivityName", string url = "undefine url")
        {
            lock (asyncLock)
            {
                bool success = false;

                try
                {
                    DynamicParameters param = new DynamicParameters();

                    if (!string.IsNullOrWhiteSpace(activityid))
                    {
                        param.Add("ActivityId", activityid);
                        param.Add("ActivityName", activityname);
                        string querysql = @"SELECT * FROM TRP_ScanCount WHERE ActivityId=@ActivityId and ActivityName=@ActivityName";
                        TRP_ScanCount model = idal.FindOne<TRP_ScanCount>(querysql, param, false);

                        if (model == null)
                        {
                            TRP_ScanCount entity = new TRP_ScanCount();
                            entity.Count = 1;
                            entity.ActivityName = activityname;
                            entity.Url = url;
                            entity.ActivityId = activityid;
                            entity.UpdateTime = DateTime.Now;
                            string insertsql = @"INSERT INTO TRP_ScanCount
                                          (
                                            [Count]
                                          ,[ActivityId]
                                          ,[ActivityName]
                                          ,[Url]
                                          ,[UpdateTime]
                                           )
                                        VALUES
                                           (
                                             @Count
                                             ,@ActivityId
                                             ,@ActivityName
                                             ,@Url
                                             ,@UpdateTime
                                            )            ";
                          success=  idal.CreateEntity<TRP_ScanCount>(insertsql, entity);
                        }
                        else
                        {
                            model.Count = model.Count + 1;
                            param.Add("Count", model.Count);
                            string updatesql = @"UPDATE TRP_ScanCount SET  Count=@Count WHERE ActivityId=@ActivityId and ActivityName=@ActivityName";
                            idal.ExcuteNonQuery<TRP_ScanCount>(updatesql, param, false);
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = "SERVER_ERROR";
                    Logger.Error(string.Format("红包扫码计数异常，异常信息：{0}", ex.ToString()));
                }

                return success;
            }
        }
    }
}
