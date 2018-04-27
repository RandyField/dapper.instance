using Common;
using Common.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zhp.Awards.Common.Helper;
using Zhp.Awards.Model;
using System.Net.Http.Headers;
using System.Web;

namespace Zhp.Awards.Activity.Controllers
{
    public class BaseApiController : ApiController
    {
        [Route("getimg")]
        public HttpResponseMessage GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();

            //获取验证码
            string code = vCode.CreateValidateCode(4);

            HttpContext.Current.Session["ValidateCode"] = code;

            //获取验证码图片
            byte[] bytes = vCode.CreateValidateGraphic(code);

            //设置响应
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes)
                //或者  
                //Content = new StreamContent(stream)  
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            return resp;
        }
    }
}
