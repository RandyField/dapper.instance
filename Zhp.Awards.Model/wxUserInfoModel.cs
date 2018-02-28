using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhp.Awards.Model
{
    public class wxUserInfoModel
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Headimgurl { get; set; }


        public string user_openid { get; set; }
        public string user_name { get; set; }

    }
}
