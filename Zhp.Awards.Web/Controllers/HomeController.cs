using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zhp.Awards.BLL;
using Zhp.Awards.Model;

namespace Zhp.Awards.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            //保存用户微信信息
            TRF_WeChatUserInfo entity = new TRF_WeChatUserInfo
            {
                openid = "omJcruMSo_al_vHgI7dQHdixsPEM",
                nickname = "RandyField",
                sex = "1",
                city = "成都",
                province = "四川",
                country = "中国",
                headimgurl = "http://wx.qlogo.cn/mmopen/vi_32/onVkE3YIWibjd3JW8XoHppFLXJg4O2CBibOGVt0Df3vQrDS3TQm6uPfdMG85LPHo5rtn1RMibPemghpicVSfStcc3A/0"
            };
            TRF_WeChatUserInfo_BLL.getInstance().Save(entity);

            //获取奖品领取信息
            var model = await  TRP_AwardReceive_BLL.getInstance().GetByAwardDetailId("308620");

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}