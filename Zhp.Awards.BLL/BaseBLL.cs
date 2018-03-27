﻿using Common;
using Common.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhp.Awards.Model;

namespace Zhp.Awards.BLL
{
    public class BaseBLL
    {
        public readonly string _key = ConfigurationManager.AppSettings["encryption"];
        /// <summary>
        /// 请求奖品
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public AwardsInfoModel GetAwardsInfo(string activityId)
        {
            AwardsInfoModel awardsInfo = null;
            try
            {
                HttpHelper httpHelper = new HttpHelper();
                string awards = httpHelper.HttpGet(String.Format("http://www.chinazhihuiping.com:89/RedPacketService/GetRedPacketServie?activityId={0}", activityId), "");
                if (string.IsNullOrWhiteSpace(awards))
                {
                    return awardsInfo;
                }
                //反序列化获取奖品信息
                awardsInfo = JsonConvert.DeserializeObject<AwardsInfoModel>(awards);

            }
            catch (Exception ex)
            {
                Logger.Info(string.Format("根据活动id获取奖品信息异常,异常信息：{0},活动ID：{1}", ex.ToString(), activityId));
            }
            return awardsInfo;
        }
    }
}
