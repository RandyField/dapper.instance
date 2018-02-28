using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Common.Enum
{
    public enum LoginStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 锁定
        /// </summary>
        Lock,
        /// <summary>
        /// 停用
        /// </summary>
        Disable,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 首次登录
        /// </summary>
        FirstLogin,
        /// <summary>
        /// 过期
        /// </summary>
        Expire,
        /// <summary>
        /// 其他
        /// </summary>
        Other,
        /// <summary>
        /// 不允许访问
        /// </summary>
        NoAccess
    }
}
