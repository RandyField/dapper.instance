using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhp.Awards.Common
{
    /// <summary>
    /// 返回信息，供前端使用
    /// </summary>
   public class ReturnInfo
    {
       /// <summary>
       /// 奖品名称
       /// </summary>
       public string awardname { get; set; }

       /// <summary>
       /// 奖品详情id
       /// </summary>
       public string awarddetailid { get; set; }

       /// <summary>
       /// 奖品图片url
       /// </summary>
       public string awardimgurl { get; set; }

       public WxUserInfo wxuserinfo { get; set; }
    }
}
