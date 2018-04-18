﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Zhp.Awards.BLL;
using Zhp.Awards.Common;
using Zhp.Awards.Model;

namespace Zhp.Awards.Activity.Controllers
{
    [RoutePrefix("api/award")]
    public class AwardController : BaseController
    {
        [Route("phone")]
        [HttpPost]
        public ResponseResult GetAwardByPhone([FromBody]JObject data)
        {
            string msg = "";

            //返回实体
            ResponseResult result = new ResponseResult();

            if (data["phone"] != null && data["activityid"] != null)
            {
                TRP_AwardReceive_BLL bll = TRP_AwardReceive_BLL.getInstance();
                string phone = data["phone"].ToString();
                string activityid = data["activityid"].ToString();
                string return_code = "";


                //1.根据手机号码和活动id判断是否参与情况
                PhoneQueryModel model = bll.GetAwards(phone, activityid, ref return_code, ref msg);
                if (return_code == "FIRST_TIME")
                {
                    result.return_code = return_code;
                    result.return_info = model;
                    RecordLog(activityid, string.Format("【首次参与】用户输入手机号码:{0}，获取奖品，时间:{1}", phone, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        "输入手机号码，首次参与", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.Url.AbsoluteUri, model.ReceiveImage);
                }

                // 1.1第一次参加，则请求奖品，填充手机号码

                // 1.2已经参加，奖品已领取
                else if (return_code == "ATTENDED")
                {
                    result.return_code = return_code;
                    result.return_info = null;
                    RecordLog(activityid, string.Format("【已领取】用户输入相同手机号码:{0}，奖品已领取，提示已参加活动，时间:{1}", phone, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                       "输入相同手机号码，再次参与,提示已参加活动", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.Url.AbsoluteUri, model.ReceiveImage);
                }

                // 1.3已经参加，奖品还未领取
                else if (return_code == "WAIT_RECEIVE")
                {
                    result.return_code = return_code;
                    result.return_info = model;
                    RecordLog(activityid, string.Format("【再次参与】用户输入相同手机号码:{0}，获取未领取奖品，时间:{1}", phone, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                      "输入相同手机号码，再次参与,获取未领取奖品", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.Url.AbsoluteUri, model.ReceiveImage);
                }
            }
            else
            {
                result.return_code = "ERROR";
                result.return_msg = "参数不完整";
                result.return_info = null;
            }
            result.return_msg = msg;
            return result;
        }

        [Route("name/is/exist")]
        [HttpPost]
        /// <summary>
        /// 判断奖品名称是否重复
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResponseResult IsExistAwardName(dynamic data)
        {
            TRP_AwardReceive_BLL bll = TRP_AwardReceive_BLL.getInstance();
            string awardname = data.awardname;
            string return_code = "";
            string msg = "";

            //返回实体
            ResponseResult result = new ResponseResult();

            //判断是否存在奖品名称
            if (bll.IsExistAwardName(awardname, ref msg))
            {
                return_code = "EXIST";
            }
            result.return_code = return_code;
            result.return_msg = msg;
            result.return_info = null;
            return result;
        }

        [Route("upload")]
        [HttpPost]
        /// <summary>
        /// 判断奖品名称是否重复
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResponseResult Uploadimg(dynamic data)
        {
            TRP_AwardReceive_BLL bll = TRP_AwardReceive_BLL.getInstance();
            string awardname = data.awardname;
            string url = data.url;
            string return_code = "";
            string msg = "";

            //返回实体
            ResponseResult result = new ResponseResult();

            //判断是否存在奖品名称
            bll.SaveImg(awardname, url, ref return_code, ref msg);

            result.return_code = return_code;
            result.return_msg = msg;
            result.return_info = null;
            return result;
        }
    }
}
