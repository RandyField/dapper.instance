using Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
        public ResponseResult IsOutofdate(dynamic data)
        {
            string guid = data.guid;
            TRP_QRCodeScanLimited_BLL bll = TRP_QRCodeScanLimited_BLL.getInstance();
            string msg = "";

            //返回实体
            ResponseResult result = new ResponseResult();

            if (bll.IsOutofdate(guid, ref msg))
            {
                result.return_code = "SUCCESS";
                result.return_msg = msg;
            }
            else
            {
                result.return_code = "FAIL";
                result.return_msg = msg;
            }  
            return result;
        }

        /// <summary>
        /// 计数(点击计数)
        /// </summary>
        /// <param name="data"></param>
        [Route("click/count/{activityid}")]
        [HttpGet]
        public ResponseResult ClickCount(string activityid)
        {
            TRP_OpenCount_BLL bll = TRP_OpenCount_BLL.getInstance();
            string msg = "";

            //返回实体
            ResponseResult result = new ResponseResult();

            if (bll.QrOpenCount(activityid, ref msg))
            {
                result.return_code = "SUCCESS";
                result.return_msg = msg;
            }
            else
            {
                result.return_code = "FAIL";
                result.return_msg = msg;
            }
            return result;
        }

        /// <summary>
        /// 扫码详细计数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("scan/detail/count")]
        [HttpPost]
        public ResponseResult ScanDetailCount(dynamic data)
        {
            string activityid = data.activityid;
            string activityname = data.activityname;
            string url = data.url;

            TRP_ScanCount_BLL bll = TRP_ScanCount_BLL.getInstance();
            string msg = "";

            //返回实体
            ResponseResult result = new ResponseResult();

            if (bll.QrScanCount(activityid, ref msg,activityname,url))
            {
                result.return_code = "SUCCESS";
                result.return_msg = msg;
            }
            else
            {
                result.return_code = "FAIL";
                result.return_msg = msg;
            }
            return result;

        }
    }
}
