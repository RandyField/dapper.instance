using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Enum
{
    public enum ActivityStatus
    {
        /// <summary>
        /// 奖品已扫
        /// </summary>
        HasScan,

        /// <summary>
        /// 已参加
        /// </summary>
        HasAttend,

        /// <summary>
        /// 当场活动第一次参加
        /// </summary>
        FirstTime,

        /// <summary>
        /// 未知
        /// </summary>
        Unknown
    }
}
