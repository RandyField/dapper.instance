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
using Common;

namespace Zhp.Awards.BLL
{
    /// <summary>
    /// 业务逻辑层
    /// </summary>
    public class TRP_AwardReceive_BLL : BaseBLL
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

        private IBaseDAL idal = DbFactory.Create();

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
                    model = dapperhelper.FindOneAsync<TRP_AwardReceive>(querysql, param, false);
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

        /// <summary>
        /// 通过电话号码和活动id 检索
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="activityid"></param>
        /// <param name="return_code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public PhoneQueryModel IsAttend(string phone, string activityid, ref string return_code, ref string msg)
        {
            PhoneQueryModel model = null;
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("ActivityId", activityid);
                param.Add("Phone", phone);

                string querysql = @"SELECT A.ActivityId,A.AwardDetailId,C.AwardName,C.ReceiveImage,    
                                    A.ReceiveTime,A.SubmitTime,A.Phone,A.OpenId   
                                    FROM TRP_AwardReceive A LEFT JOIN  TRP_Award B ON A.AwardId=B.Id   
                                    LEFT JOIN TRP_AwardUrl C ON B.AwardName=C.AwardName     
                                                    WHERE A.ActivityId=@ActivityId and A.Phone=@Phone";

                model = idal.FindOne<PhoneQueryModel>(querysql, param, false);

                //首次参加活动
                if (model == null)
                {
                    return_code = "FIRST_TIME";

                    //请求奖品
                    AwardsInfoModel awardsModel = GetAwardsInfo(activityid);

                    //解密奖品详情id
                    var detailid = DESEncrypt.Decrypt(awardsModel.id, _key);

                    //领奖
                    model=SavePhone(phone, activityid, detailid);
                }
                else
                {
                    //已参加活动，但是还未领取
                    if (model.ReceiveTime == null)
                    {
                        return_code = "WAIT_RECEIVE";
                    }

                    //已参加活动，且已领取
                    else
                    {
                        return_code = "ATTENDED";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "SERVER_ERROR";
                Logger.Error(string.Format("通过电话号码和活动id检索异常，异常信息:{0},phone:{1}，活动id:{2}", ex.ToString(), phone, activityid));
            }
            return model;
        }

        /// <summary>
        /// 通过手机号码领取
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="activityid"></param>
        /// <param name="awardid"></param>
        /// <returns></returns>
        public PhoneQueryModel SavePhone(string phone, string activityid, string awardid)
        {
            PhoneQueryModel model = null;
            bool success = false;
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("ActivityId", activityid);
                param.Add("AwardDetailId", awardid);

                string querysql = @"SELECT A.ActivityId,A.AwardDetailId,C.AwardName,C.ReceiveImage,    
                                    A.ReceiveTime,A.SubmitTime,A.Phone,A.OpenId   
                                    FROM TRP_AwardReceive A LEFT JOIN  TRP_Award B ON A.AwardId=B.Id   
                                    LEFT JOIN TRP_AwardUrl C ON B.AwardName=C.AwardName     
                                                    WHERE A.ActivityId=@ActivityId and A.Phone=@Phone";


                param.Add("Phone", phone);
                param.Add("SubmitTime", DateTime.Now);

                string updatesql = @"UPDATE TRP_AwardReceive SET  Phone=@Phone,SubmitTime=@SubmitTime WHERE AwardDetailId=@AwardDetailId and ActivityId=@ActivityId";

                success = idal.ExcuteNonQuery<TRP_AwardReceive>(updatesql, param, false) > 0;



                model = idal.FindOne<PhoneQueryModel>(querysql, param, false);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("根据手机号码领取奖品，异常信息:{0},phone:{1}，活动id:{2}", ex.ToString(), phone, activityid));
            }
            return model;
        }
    }
}
