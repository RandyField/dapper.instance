using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zhp.Awards.Common;

namespace Zhp.Awards.Activity.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : BaseApiController
    {

        /// <summary>
        /// rsa测试
        /// </summary>
        /// <param name="data"></param>
        [Route("rsa")]
        [HttpGet]
        public string rsatest()
        {
            RSAhelper helper = new RSAhelper();
            helper.GenerateKeys();

            //F:\\RSA\ [RSA.Pub, RSA.Private]
            string str1 = "";
            string str2 = "";
            str1 = helper.Encrypt("Randy", "F:\\RSA\\RSA.Pub");
             str2 = helper.Decrypt(str1, "F:\\RSA\\RSA.Private");
             return str2;
        }
    }
}
