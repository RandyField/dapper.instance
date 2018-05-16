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
        /// 线程锁
        /// </summary>
        private static readonly object asyncLock = new object();

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
        /// 通过电话号码和活动id发奖
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="activityid"></param>
        /// <param name="return_code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public PhoneQueryModel GetAwards(string phone, string activityid, ref string return_code, ref string msg)
        {
            lock (asyncLock)
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

                        if (string.IsNullOrWhiteSpace(awardsModel.Class))
                        {
                            Initialize(activityid);
                            //奖品初始化

                            awardsModel = GetAwardsInfo(activityid);
                        }

                        if (awardsModel != null
                            && string.IsNullOrWhiteSpace(awardsModel.Class))
                        {
                            return_code = "NO_AWARDS";
                        }
                        else
                        {
                            //解密奖品详情id
                            var detailid = DESEncrypt.Decrypt(awardsModel.id, _key);

                            //领奖
                            model = SavePhone(phone, activityid, detailid);
                        }
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
                    return_code = "FAIL";
                    msg = "SERVER_ERROR";
                    Logger.Error(string.Format("通过电话号码和活动id检索异常，异常信息:{0},phone:{1}，活动id:{2}", ex.ToString(), phone, activityid));
                }
                return model;
            }
        }

        /// <summary>
        /// 获取flash前端已经请求过的奖品
        /// </summary>
        /// <param name="activityid"></param>
        /// <param name="awardid"></param>
        /// <param name="return_code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public PhoneQueryModel GetAward(string activityid, string awardid, ref string return_code, ref string msg)
        {
            PhoneQueryModel model = null;
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("ActivityId", activityid);
                param.Add("AwardDetailId", awardid);

                string querysql = @"SELECT A.ActivityId,A.AwardDetailId,C.AwardName,C.ReceiveImage,    
                                    A.ReceiveTime,A.SubmitTime,A.Phone,A.OpenId   
                                    FROM TRP_AwardReceive A LEFT JOIN  TRP_Award B ON A.AwardId=B.Id   
                                    LEFT JOIN TRP_AwardUrl C ON B.AwardName=C.AwardName     
                                                WHERE A.ActivityId=@ActivityId and A.AwardDetailId=@AwardDetailId";

                model = idal.FindOne<PhoneQueryModel>(querysql, param, false);

                return_code = "SUCCESS";

                //奖品已回收
                if (model == null)
                {
                    return_code = "HAD_RECYCLE";
                }

                //奖品还存在
                else
                {
                    //奖品已输入手机号码
                    if (model.Phone != null)
                    {
                        //奖品已领取
                        return_code = "HAD_RECEIVED";
                    }
                    else
                    {
                        //领取时间不为空  二维码已扫
                        if (model.SubmitTime != null)
                        {
                            return_code = "HAD_RECEIVED";
                        }
                        //领取时间为空  二维码置为已扫
                        else
                        {
                            param.Add("SubmitTime", DateTime.Now);

                            string updatesql = @"UPDATE TRP_AwardReceive SET  SubmitTime=@SubmitTime WHERE AwardDetailId=@AwardDetailId and ActivityId=@ActivityId";

                            idal.ExcuteNonQuery<TRP_AwardReceive>(updatesql, param, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return_code = "FAIL";
                msg = "SERVER_ERROR";
                Logger.Error(string.Format("通过活动id与奖品id检索异常，异常信息:{0},活动id:{1}，奖品id:{2}", ex.ToString(), activityid, awardid));
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

        /// <summary>
        /// 通过手机号码领取
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="activityid"></param>
        /// <param name="awardid"></param>
        /// <returns></returns>
        public PhoneQueryModel SavePhone(string phone, string activityid, string awardid, ref string return_code, ref string msg)
        {
            PhoneQueryModel model = null;
            bool success = false;
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("ActivityId", activityid);
                

                string querysql = @"SELECT A.ActivityId,A.AwardDetailId,C.AwardName,C.ReceiveImage,    
                                    A.ReceiveTime,A.SubmitTime,A.Phone,A.OpenId   
                                    FROM TRP_AwardReceive A LEFT JOIN  TRP_Award B ON A.AwardId=B.Id   
                                    LEFT JOIN TRP_AwardUrl C ON B.AwardName=C.AwardName     
                                                    WHERE A.ActivityId=@ActivityId and A.Phone=@Phone";


                param.Add("Phone", phone);
               
                //根据活动id与手机号码查询
                model = idal.FindOne<PhoneQueryModel>(querysql, param, false);

                param.Add("AwardDetailId", awardid);
                if (model != null)
                {
                    if (model.Phone == phone)
                    {
                        return_code = "ATTEND";
                    }
                    else
                    {
                        return_code = "HAD_RECEIVE";
                    }
                }

                     
                //第一次参加
                else
                {
                    string updatesql = @"UPDATE TRP_AwardReceive SET  Phone=@Phone WHERE AwardDetailId=@AwardDetailId and ActivityId=@ActivityId";
                    success = idal.ExcuteNonQuery<TRP_AwardReceive>(updatesql, param, false) > 0;
                    //根据活动id与手机号码查询
                    model = idal.FindOne<PhoneQueryModel>(querysql, param, false);
                    return_code = "SUCCESS";
                }


            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("根据手机号码领取奖品，异常信息:{0},phone:{1}，活动id:{2}", ex.ToString(), phone, activityid));
            }
            return model;
        }

        /// <summary>
        /// 判断奖品名称是否重复
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsExistAwardName(string name, ref string msg)
        {
            bool exist = false;
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("AwardName", name.Trim());


                string querysql = @"SELECT [id]
                                              ,[AwardName]
                                              ,[ReceiveImage] FROM [TRP_AwardUrl]    
                                     WHERE AwardName=@AwardName";

                TRP_AwardUrl model = idal.FindOne<TRP_AwardUrl>(querysql, param, false);
                if (model != null)
                {
                    if (model.AwardName.Trim() == name.Trim())
                    {
                        exist = true;
                    }
                }

                msg = "";
            }
            catch (Exception ex)
            {
                msg = "SERVER_ERROR";
                Logger.Error(string.Format("判断奖品名称是否存在异常，异常信息:{0}", ex.ToString()));
            }

            return exist;
        }


        /// <summary>
        /// 保存奖品名称 奖品路径
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="return_code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SaveImg(string name, string url, ref string return_code, ref string msg)
        {
            bool successs = false;
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("AwardName", name.Trim());
                param.Add("ReceiveImage", url.Trim());

                string updatesql = @"INSERT INTO TRP_AwardUrl (AwardName,ReceiveImage) VALUES  (@AwardName,@ReceiveImage)";
                int i = idal.ExcuteNonQuery<TRP_AwardUrl>(updatesql, param, false);

                if (i > 0)
                {
                    successs = true;
                    return_code = "SUCCESS";
                }
                else
                {
                    return_code = "FAIL";
                }

                msg = "";
            }
            catch (Exception ex)
            {
                return_code = "FAIL";
                msg = "SERVER_ERROR";
                Logger.Error(string.Format("判断奖品名称是否存在异常，异常信息:{0}", ex.ToString()));
            }

            return successs;
        }
    }
}
