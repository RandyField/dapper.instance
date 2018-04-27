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
    public class TRF_WeChatUserInfo_BLL
    {
        #region 单例模式

        /// <summary>
        /// 定义私有变量记录单例类的唯一实例
        /// </summary>
        private static TRF_WeChatUserInfo_BLL instance;

        /// <summary>
        /// 私有构造函数，该类无法被实例化
        /// </summary>
        private TRF_WeChatUserInfo_BLL() { }

        /// <summary>
        /// 线程锁
        /// </summary>
        private static object asyncLock = new object();

        /// <summary>
        /// 公用方法getInstance()提供该类实例的唯一全局访问点
        /// </summary>
        /// <returns></returns>
        public static TRF_WeChatUserInfo_BLL getInstance()
        {
            //如果实例不存在，则new一个新实例，否则返回已有实例
            if (instance == null)
            {
                instance = new TRF_WeChatUserInfo_BLL();
            }
            return instance;
        }

        private IBaseDAL idal = DbFactory.Create();

        #endregion


        /// <summary>
        /// 保存微信用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Save(TRF_WeChatUserInfo model)
        {
            bool success = false;
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("openid", model.openid);
                string querysql = @"SELECT * FROM TRF_WeChatUserInfo WHERE openid=@openid";
                TRF_WeChatUserInfo queryresult = idal.FindOne<TRF_WeChatUserInfo>(querysql, param, false);
                if (queryresult == null)
                {
                    string insertsql = @"INSERT INTO TRF_WeChatUserInfo
                                          (
                                              openid
                                              ,nickname
                                              ,sex
                                              ,language
                                              ,city
                                              ,province
                                              ,country
                                              ,headimgurl
                                              ,privilege
                                           )
                                        VALUES
                                           (
                                             @openid
                                              ,@nickname
                                              ,@sex
                                              ,@language
                                              ,@city
                                              ,@province
                                              ,@country
                                              ,@headimgurl
                                              ,@privilege
                                            )            ";
                    success = idal.CreateEntity<TRF_WeChatUserInfo>(insertsql, model);
                }
                else
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("保存用户微信信息异常，异常信息:{0}", ex.ToString()));
            }
            return success;

        }
    }
}
