using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Zhp.Awards.BLL;
using Zhp.Awards.Model;

namespace Zhp.Awards.Activity.Controllers
{
    public class BaseController : ApiController
    {
        //记录日志
        public void RecordLog(string activityid, string des,string pagedesc, string ip = "127.0.0.1", string url = "test",string img=null)
        {
            TRP_ClientLog entity = new TRP_ClientLog();
            entity.CreateTime = DateTime.Now;
            entity.DeleteMark = false;
            entity.Enable = true;
            entity.PageUrl = url;
            entity.IPAddress = ip;
            entity.ActivityId = Convert.ToInt32(activityid);
            entity.Description = des;
            entity.PageDesc = pagedesc;
            entity.ReceiveImage = img;
            TRP_ClientLog_BLL.getInstance().SaveLog(entity);
        }
    }
}
