using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhp.Awards.Model
{
    /// <summary>
    /// 奖品核销Post Model
    /// </summary>
    public class AwardReceivedModel
    {
        public string Openid
        {
            get;
            set;
        }
        public string AwardDetailId
        {
            get;
            set;
        }
        public string Type { get; set; }
        public string AwardName { get; set; }
        public string ActivityId { get; set; }
    }
}
