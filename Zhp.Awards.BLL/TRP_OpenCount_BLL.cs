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
    public class TRP_OpenCount_BLL
    {
        #region 单例模式

        /// <summary>
        /// 定义私有变量记录单例类的唯一实例
        /// </summary>
        private static TRP_OpenCount_BLL instance;

        /// <summary>
        /// 线程锁
        /// </summary>
        private static readonly object asyncLock = new object();

        /// <summary>
        /// 私有构造函数，该类无法被实例化
        /// </summary>
        private TRP_OpenCount_BLL() { }

        /// <summary>
        /// 公用方法getInstance()提供该类实例的唯一全局访问点
        /// </summary>
        /// <returns></returns>
        public static TRP_OpenCount_BLL getInstance()
        {
            //如果实例不存在，则new一个新实例，否则返回已有实例
            if (instance == null)
            {
                instance = new TRP_OpenCount_BLL();
            }
            return instance;
        }

        private IBaseDAL idal = DbFactory.Create();

        #endregion

        /// <summary>
        /// 计数
        /// </summary>
        /// <param name="activityId"></param>
        public bool QrOpenCount(string activityId,ref string msg)
        {
            lock (asyncLock)
            {
                bool success = false;
                try
                {
                    DynamicParameters param = new DynamicParameters();
                    if (!string.IsNullOrWhiteSpace(activityId))
                    {                   
                        param.Add("ActivityId", activityId);
                        string querysql = @"SELECT * FROM TRP_OpenCount WHERE ActivityId=@ActivityId";
                        TRP_OpenCount model = idal.FindOne<TRP_OpenCount>(querysql, param, false);
                        if (model == null)
                        {
                            TRP_OpenCount entity = new TRP_OpenCount();
                            entity.ActivityId = activityId;
                            entity.Count = 1;
                            string insertsql = @"INSERT INTO TRP_OpenCount
                                          (
                                              [Count]
                                              ,[ActivityId]
                                           )
                                        VALUES
                                           (
                                             @Count
                                             ,@ActivityId
                                            )            ";
                            idal.CreateEntity<TRP_OpenCount>(insertsql, entity);
                            success = true;
                        }
                        else
                        {
                            model.Count = model.Count + 1;
                            param.Add("Count", model.Count);
                            param.Add("ActivityId", activityId);
                            string updatesql = @"UPDATE TRP_OpenCount SET  Count=@Count WHERE ActivityId=@ActivityId";
                            idal.ExcuteNonQuery<TRP_OpenCount>(updatesql, param, false);
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = "SERVER_ERROR";
                    Logger.Error(string.Format("点击计数异常，异常信息：{0}", ex.ToString()));
                }

                return success;
            }
        }
    }
}
