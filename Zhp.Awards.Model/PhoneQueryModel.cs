using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhp.Awards.Model
{
    public class PhoneQueryModel
    {
        public Nullable<int> ActivityId { get; set; }
        public Nullable<int> AwardDetailId { get; set; }
        public string AwardName { get; set; }
        public string ReceiveImage { get; set; }
        public Nullable<System.DateTime> ReceiveTime { get; set; }
        public Nullable<System.DateTime> SubmitTime { get; set; }
        public string Phone { get; set; }
        public string OpenId { get; set; }
    }
}
