using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhp.Awards.Model
{
    public class LCDSearchModel
    {
        /// <summary>
        /// id标识
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string cityName { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string lineName { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string stationName { get; set; }

        /// <summary>
        /// 计算机名
        /// </summary>
        public string pcName { get; set; }

        /// <summary>
        /// 点位
        /// </summary>
        public string position { get; set; }

        /// <summary>
        /// 故障发生时间
        /// </summary>
        public DateTime? brokenTime { get; set; }

        public string brokenTimeStr { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public string errorCode { get; set; }

        /// <summary>
        /// 错误详细信息
        /// </summary>
        public string errorInfo { get; set; }

        /// <summary>
        /// 屏幕类型
        /// </summary>
        public string screenType { get; set; }
    }
}