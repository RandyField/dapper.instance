using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Zhp.Awards.Activity.Controllers
{
    public class QrcodeController : ApiController
    {
        //// GET: api/Qrcode
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        [Route("api/qrcode?guid={guid}&a={name}&b={id}")]
        [HttpGet]
        // GET: api/Qrcode/5
        public string Get(int guid,string name,string id)
        {
            return "value";
        }

        [Route("api/qrcode/test/test")]
        [HttpPost]
        // POST: api/Qrcode
        public void Post(dynamic guid)
        {

        }

        // PUT: api/Qrcode/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Qrcode/5
        public void Delete(int id)
        {
        }
    }
}
