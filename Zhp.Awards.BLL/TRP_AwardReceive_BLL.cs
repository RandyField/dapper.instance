using Common.Helper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhp.Awards.DAL;
using Zhp.Awards.Model;
using Zhp.Awards.IDAL;

namespace Zhp.Awards.BLL
{
    /// <summary>
    /// 业务逻辑层
    /// </summary>
    public class TRP_AwardReceive_BLL
    {
        #region 单例模式

        /// <summary>
        /// 定义私有变量记录单例类的唯一实例
        /// </summary>
        private static TRP_AwardReceive_BLL instance;

        /// <summary>
        /// 私有构造函数，该类无法被实例化
        /// </summary>
        private TRP_AwardReceive_BLL() { }

        /// <summary>
        /// 公用方法getInstance()提供该类实例的唯一全局访问点
        /// </summary>
        /// <returns></returns>
        public static TRP_AwardReceive_BLL getInstance()
        {
            //如果实例不存在，则new一个新实例，否则返回已有实例
            if (instance == null)
            {
                instance = new TRP_AwardReceive_BLL();
            }
            return instance;
        }

        #endregion

        /// <summary>
        /// 根据AwardDetailId获取奖品领取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TRP_AwardReceive> GetByAwardDetailId(string id)
        {
            Task<TRP_AwardReceive> model = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    var dapperhelper = DbFactory.Create();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("AwardDetailId", id);
                    string querysql = @"SELECT * FROM TRP_AwardReceive WHERE AwardDetailId=@AwardDetailId";
                    //model = dapperhelper.FindOneAsync<TRP_AwardReceive>(querysql, param, false);
                    model =  dapperhelper.FindOneAsync<TRP_AwardReceive>(querysql, param, false);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("通过奖品id获取奖品领奖信息异常，异常信息:{0},id:{1}", ex.ToString(), id));
            }
            return model;
        }

        /// <summary>
        /// 领奖
        /// </summary>
        /// <param name="id"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public bool ReceivedAwards(string id, string openid)
        {
            bool success = false;
            try
            {
                var dapperhelper = DbFactory.Create();
                DynamicParameters param = new DynamicParameters();
                param.Add("ReceiveTime", DateTime.Now);
                param.Add("AwardDetailId", id);
                param.Add("OpenId", openid);
                string updatesql = @"UPDATE TRP_AwardReceive SET  ReceiveTime=@ReceiveTime WHERE AwardDetailId=@AwardDetailId and OpenId=@OpenId";
                success = dapperhelper.ExcuteNonQuery<TRP_AwardReceive>(updatesql, param, false) > 0;
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("领奖异常，异常信息:{0},id:{1}，Openid:{2}", ex.ToString(), id, openid));
            }
            return success;
        }
    }
}
