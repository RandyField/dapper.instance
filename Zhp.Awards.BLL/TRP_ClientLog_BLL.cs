using Common.Helper;
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
    public class TRP_ClientLog_BLL
    {
        #region 单例模式

        /// <summary>
        /// 定义私有变量记录单例类的唯一实例
        /// </summary>
        private static TRP_ClientLog_BLL instance;

        /// <summary>
        /// 私有构造函数，该类无法被实例化
        /// </summary>
        private TRP_ClientLog_BLL() { }

        /// <summary>
        /// 公用方法getInstance()提供该类实例的唯一全局访问点
        /// </summary>
        /// <returns></returns>
        public static TRP_ClientLog_BLL getInstance()
        {
            //如果实例不存在，则new一个新实例，否则返回已有实例
            if (instance == null)
            {
                instance = new TRP_ClientLog_BLL();
            }
            return instance;
        }

        private IBaseDAL idal = DbFactory.Create();

        #endregion

        /// <summary>
        /// 保存数据库日志
        /// </summary>
        /// <param name="model"></param>
        public void SaveLog(TRP_ClientLog model)
        {
            try
            {
                string insertsql = @"INSERT INTO [TRP_ClientLog]
                                          (
                                              [CreateTime]
                                              ,[Description]
                                              ,[DeleteMark]
                                              ,[Enable]
                                              ,[IPAddress]
                                              ,[PageDesc]
                                              ,[PageUrl]
                                              ,[ActivityId]
                                              ,[ReceiveImage]
                                           )
                                        VALUES
                                           (
                                               @CreateTime
                                              ,@Description
                                              ,@DeleteMark
                                              ,@Enable
                                              ,@IPAddress
                                              ,@PageDesc
                                              ,@PageUrl
                                              ,@ActivityId
                                              ,@ReceiveImage
                                            )            ";
                idal.CreateEntity<TRP_ClientLog>(insertsql, model);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("写数据库日志异常，异常信息:{0}", ex.ToString()));
            }
        }
    }
}
