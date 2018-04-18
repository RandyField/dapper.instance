using Common.Helper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;
using Zhp.Awards.BLL;
using Zhp.Awards.Common;

namespace Zhp.Awards.Activity.Controllers
{
    [RoutePrefix("api/qrcode")]
    public class QrcodeController : ApiController
    {
        ////// GET: api/Qrcode
        ////public IEnumerable<string> Get()
        ////{
        ////    return new string[] { "value1", "value2" };
        ////}

        //[Route("api/qrcode/guid={guid}&a={name}&b={id}")]
        //[HttpGet]
        //// GET: api/Qrcode/5
        //public string Get(int guid, string name, string id)
        //{
        //    return "value";
        //}

        //[Route("api/qrcode/test/test")]
        //[HttpPost]
        //// POST: api/Qrcode
        //public void Post(dynamic guid)
        //{
        //}


        /// <summary>
        /// 判断二维码是否过期
        /// </summary>
        /// <param name="guid"></param>
        [Route("is/outofdate")]
        [HttpPost]
        public ResponseResult IsOutofdate([FromBody]JObject data)
        {
            string msg = "";

            //返回实体
            ResponseResult result = new ResponseResult();

            if (data["guid"] != null)
            {
                string guid = data["guid"].ToString();

                TRP_QRCodeScanLimited_BLL bll = TRP_QRCodeScanLimited_BLL.getInstance();

                if (bll.IsOutofdate(guid, ref msg))
                {
                    result.return_code = "SUCCESS";
                }
                else
                {
                    result.return_code = "FAIL";
                }
            }
            else
            {
                result.return_code = "FAIL";

                //guid为空
                msg = "参数不完整";
            }
            result.return_msg = msg;
            return result;
        }

        /// <summary>
        /// 计数(点击计数)
        /// </summary>
        /// <param name="data"></param>
        [Route("click/count/{activityid}")]
        [HttpGet]
        public HttpResponseMessage ClickCount(string activityid)
        {
            string msg = "";

            //返回实体
            ResponseResult result = new ResponseResult();

            TRP_OpenCount_BLL bll = TRP_OpenCount_BLL.getInstance();


            if (bll.QrOpenCount(activityid, ref msg))
            {
                result.return_code = "SUCCESS";
            }
            else
            {
                result.return_code = "FAIL";
            }

            result.return_msg = msg;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string str = serializer.Serialize(result);
            return new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
        }

        /// <summary>
        /// 扫码详细计数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("scan/detail/count")]
        [HttpPost]
        public ResponseResult ScanDetailCount([FromBody]JObject data)
        {
            string msg = "";

            //返回实体
            ResponseResult result = new ResponseResult();

            if (data["activityid"] != null
                    && data["activityname"] != null
                    && data["url"] != null)
            {
                string activityid = data["activityid"].ToString();
                string activityname = data["activityname"].ToString();
                string url = data["url"].ToString();

                TRP_ScanCount_BLL bll = TRP_ScanCount_BLL.getInstance();


                if (bll.QrScanCount(activityid, ref msg, activityname, url))
                {
                    result.return_code = "SUCCESS";
                }
                else
                {
                    result.return_code = "FAIL";           
                }
            }
            else
            {
                result.return_code = "FAIL";
                msg = "参数不完整";
            }

            result.return_msg = msg;
            return result;

        }
    }
}
