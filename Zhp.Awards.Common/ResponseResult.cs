using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhp.Awards.Model;

namespace Zhp.Awards.Common
{
    /// <summary>
    /// http api response result model
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 返回状态码
        /// 成功：SUCCESS
        /// 失败：FAIL
        /// </summary>
        public string return_code { get; set; }

        /// <summary>
        /// 返回信息
        /// 如果有错误，即非空，内容为错误原因
        /// </summary>
        public string return_msg { get; set; }

        /// <summary>
        /// 返回信息
        /// 供前端使用
        /// </summary>
        public PhoneQueryModel return_info { get; set; }
    }
}
