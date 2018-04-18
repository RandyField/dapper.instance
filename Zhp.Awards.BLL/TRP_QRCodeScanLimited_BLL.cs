using Common.Helper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhp.Awards.DAL;
using Zhp.Awards.IDAL;
using Zhp.Awards.Model;

namespace Zhp.Awards.BLL
{
    public class TRP_QRCodeScanLimited_BLL
    {
        #region 单例模式

        /// <summary>
        /// 定义私有变量记录单例类的唯一实例
        /// </summary>
        private static TRP_QRCodeScanLimited_BLL instance;

        /// <summary>
        /// 线程锁
        /// </summary>
        private static readonly object asyncLock = new object();

        /// <summary>
        /// 私有构造函数，该类无法被实例化
        /// </summary>
        private TRP_QRCodeScanLimited_BLL() { }

        /// <summary>
        /// 公用方法getInstance()提供该类实例的唯一全局访问点
        /// </summary>
        /// <returns></returns>
        public static TRP_QRCodeScanLimited_BLL getInstance()
        {
            //如果实例不存在，则new一个新实例，否则返回已有实例
            if (instance == null)
            {
                instance = new TRP_QRCodeScanLimited_BLL();
            }
            return instance;
        }

        private IBaseDAL idal = DbFactory.Create();

        #endregion

        /// <summary>
        /// 判断二维码是否过期
        /// </summary>
        /// <param name="activityId"></param>
        public bool IsOutofdate(string guid, ref string msg, int times = 1)
        {
            int _qrcodetimes = Convert.ToInt32(ConfigurationManager.AppSettings["qrcodetimes"]);

            if (_qrcodetimes>times)
            {
                times = _qrcodetimes;
            }

            lock (asyncLock)
            {
                bool success = false;

                try
                {
                    DynamicParameters param = new DynamicParameters();

                    if (!string.IsNullOrWhiteSpace(guid))
                    {
                        param.Add("QRRCode", guid);
                        string querysql = @"SELECT * FROM TRP_QRCodeScanLimited WHERE QRRCode=@QRRCode";
                        TRP_QRCodeScanLimited model = idal.FindOne<TRP_QRCodeScanLimited>(querysql, param, false);

                        if (model == null)
                        {
                            TRP_QRCodeScanLimited entity = new TRP_QRCodeScanLimited();
                            entity.LimitedCount = 1;
                            entity.QRRCode = guid;
                            entity.UpdateTime = DateTime.Now;
                            string insertsql = @"INSERT INTO TRP_QRCodeScanLimited
                                          (
                                              [LimitedCount]
                                              ,[QRRCode]
                                              ,[UpdateTime]
                                           )
                                        VALUES
                                           (
                                             @LimitedCount
                                             ,@QRRCode
                                             ,@UpdateTime
                                            )            ";
                            idal.CreateEntity<TRP_QRCodeScanLimited>(insertsql, entity);
                            success = true;
                        }
                        else
                        {
                            if (model.LimitedCount < times)
                            {
                                model.LimitedCount = model.LimitedCount + 1;
                                param.Add("LimitedCount", model.LimitedCount);
                                string updatesql = @"UPDATE TRP_QRCodeScanLimited SET  LimitedCount=@LimitedCount WHERE QRRCode=@QRRCode";
                                idal.ExcuteNonQuery<TRP_QRCodeScanLimited>(updatesql, param, false);
                                success = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = "SERVER_ERROR";
                    Logger.Error(string.Format("判断二维码是否过期异常，异常信息：{0}", ex.ToString()));
                }

                return success;
            }
        }
    }
}
