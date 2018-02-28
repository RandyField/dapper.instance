using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhp.Awards.Model
{
    public class SYS_LOG_MODEL 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public decimal ID
        {
            get;
            set;
        }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName
        {
            get;
            set;
        }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 操作说明
        /// 主要描述做什么事情
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 描述当前异常的消息
        /// </summary>
        public string ErrorMsg
        {
            get;
            set;
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 获取调用堆栈上直接帧的字符串表示形式
        /// </summary>
        public string StackTrace
        {
            get;
            set;
        }

        /// <summary>
        /// 写入时间
        /// </summary>
        public DateTime CreateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 日志类型 1：业务系统 2：接口 3：服务
        /// </summary>
        public int LogType
        {
            get;
            set;
        }

        /// <summary>
        /// 日志级别 1：普通 2：验证 0：其他
        /// </summary>
        public int LevelNum
        {
            get;
            set;
        }

        /// <summary>
        /// 操作部门编号
        /// </summary>
        public string DepartmentNo
        {
            get;
            set;
        }
    }
}
