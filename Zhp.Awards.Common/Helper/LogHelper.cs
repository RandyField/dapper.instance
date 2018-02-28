using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using log4net.Core;
using System.Diagnostics;
using System.IO;

namespace  Common.Helper
{
    public class LogHelper
    {
        //public static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ILog mLog;

        /// <summary>
        /// 指定配置文件中的name的logger,构造方法
        /// </summary>
        /// <param name="name">配置文件中的logger名</param>
        public LogHelper(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = "RollingFileAppender";

            mLog = LogManager.GetLogger(name);
        }


        public LogHelper(Type type)
        {
            mLog = LogManager.GetLogger(type);
        }


        void WriteLog(Level level, object message, Exception ex)
        {
            StackTrace trace = new StackTrace(true);
            string loggername = typeof(Logger).FullName;
            string logname = typeof(LogHelper).FullName;
            int frameIndex = 0;
            while (frameIndex < trace.FrameCount)
            {
                StackFrame frame = trace.GetFrame(frameIndex);
                MethodBase method = frame.GetMethod();
                if (loggername == method.DeclaringType.FullName || logname == method.DeclaringType.FullName)
                    frameIndex++;
                else
                    break;
            }

            mLog.Logger.Log(trace.GetFrame(frameIndex - 1).GetMethod().DeclaringType, level, message, ex);
        }

        #region Debug

        /// <summary>
        /// 向日志文件中写入调试信息
        /// </summary>
        /// <param name="message">写入的信息内容</param>
        /// <param name="exception">错误类型</param>
        public void Debug(object message, Exception exception)
        {
            if (mLog.IsDebugEnabled)
                WriteLog(Level.Debug, message, exception);
        }

        /// <summary>
        /// 向日志文件中写入调试信息
        /// </summary>
        /// <param name="message">写入的信息内容</param>
        public void Debug(object message)
        {
            if (mLog.IsDebugEnabled)
                WriteLog(Level.Debug, message, null);
        }

        /// <summary>
        /// 向日志文件中写入调试信息
        /// </summary>
        /// <param name="exception">写入的错误类型</param>
        public void Debug(Exception exception)
        {
            if (mLog.IsErrorEnabled)
                WriteLog(Level.Debug, exception.ToString(), null);
        }

        /// <summary>
        /// 向日志文件中写入调试信息
        /// </summary>
        /// <param name="message">写入复合格式字符串信息内容</param>
        /// <param name="args">包含零个或多个要格式化的对象的 System.Object 数组</param>
        public void DebugFormat(string message, params object[] args)
        {
            if (mLog.IsDebugEnabled)
            {
                message = string.Format(message, args);
                WriteLog(Level.Debug, message, null);
            }
        }

        #endregion

        #region Error

        /// <summary>
        /// 向日志文件中写入错误信息
        /// </summary>
        /// <param name="exception">错误类型</param>
        public void Error(Exception exception)
        {
            if (mLog.IsErrorEnabled)
                WriteLog(Level.Error, exception.ToString(), null);
        }

        /// <summary>
        /// 向日志文件中写入错误信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        /// <param name="exception">错误类型</param>
        public void Error(object message, Exception exception)
        {
            if (mLog.IsErrorEnabled)
                WriteLog(Level.Error, message, exception);
        }

        /// <summary>
        /// 向日志文件中写入错误信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        public void Error(object message)
        {
            WriteLog(Level.Error, message, null);
        }

        /// <summary>
        /// 向日志文件中写入错误信息
        /// </summary>
        /// <param name="message">写入复合格式字符串信息内容</param>
        /// <param name="args">包含零个或多个要格式化的对象的 System.Object 数组</param>
        public void ErrorFormat(string message, params object[] args)
        {
            if (mLog.IsErrorEnabled)
            {
                message = string.Format(message, args);
                WriteLog(Level.Error, message, null);
            }
        }

        #endregion

        #region Fatal

        /// <summary>
        /// 向日志文件中写入致命错误信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        /// <param name="exception">错误类型</param>
        public void Fatal(object message, Exception exception)
        {
            if (mLog.IsFatalEnabled)
                WriteLog(Level.Fatal, message, exception);
        }

        /// <summary>
        /// 向日志文件中写入致命错误信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        public void Fatal(object message)
        {
            if (mLog.IsFatalEnabled)
                WriteLog(Level.Fatal, message, null);
        }

        /// <summary>
        /// 向日志文件中写入致命错误信息
        /// </summary>
        /// <param name="exception">错误类型</param>
        public void Fatal(Exception exception)
        {
            if (mLog.IsErrorEnabled)
                WriteLog(Level.Fatal, exception.ToString(), null);
        }

        /// <summary>
        /// 向日志文件中写入致命错误信息
        /// </summary>
        /// <param name="message">写入复合格式字符串信息内容</param>
        /// <param name="args">包含零个或多个要格式化的对象的 System.Object 数组</param>
        public void FatalFormat(string message, params object[] args)
        {
            if (mLog.IsFatalEnabled)
            {
                message = string.Format(message, args);
                WriteLog(Level.Fatal, message, null);
            }
        }

        #endregion

        #region Info

        /// <summary>
        /// 向日志文件中写入一般信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        /// <param name="exception">错误类型</param>
        public void Info(object message, Exception exception)
        {
            if (mLog.IsInfoEnabled)
                WriteLog(Level.Info, message, exception);
        }

        /// <summary>
        /// 向日志文件中写入一般信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        public void Info(object message)
        {
            if (mLog.IsInfoEnabled)
                WriteLog(Level.Info, message, null);
        }

        /// <summary>
        /// 向日志文件中写入一般信息
        /// </summary>
        /// <param name="exception">错误类型</param>
        public void Info(Exception exception)
        {
            if (mLog.IsErrorEnabled)
                WriteLog(Level.Info, exception.ToString(), null);
        }

        /// <summary>
        /// 向日志文件中写入一般信息
        /// </summary>
        /// <param name="message">写入复合格式字符串信息内容</param>
        /// <param name="args">包含零个或多个要格式化的对象的 System.Object 数组</param>
        public void InfoFormat(string message, params object[] args)
        {
            if (mLog.IsInfoEnabled)
            {
                message = string.Format(message, args);
                WriteLog(Level.Info, message, null);
            }
        }

        #endregion

        #region Warn

        /// <summary>
        /// 向日志文件中写入警告信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        /// <param name="exception">错误类型</param>
        public void Warn(object message, Exception exception)
        {
            if (mLog.IsWarnEnabled)
                WriteLog(Level.Warn, message, exception);
        }

        /// <summary>
        /// 向日志文件中写入警告信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        public void Warn(object message)
        {
            if (mLog.IsWarnEnabled)
                WriteLog(Level.Warn, message, null);
        }

        /// <summary>
        /// 向日志文件中写入警告信息
        /// </summary>
        /// <param name="exception">错误类型</param>
        public void Warn(Exception exception)
        {
            if (mLog.IsErrorEnabled)
                WriteLog(Level.Warn, exception.ToString(), null);
        }

        /// <summary>
        /// 向日志文件中写入警告信息
        /// </summary>
        /// <param name="message">写入复合格式字符串信息内容</param>
        /// <param name="args">包含零个或多个要格式化的对象的 System.Object 数组</param>
        public void WarnFormat(string message, params object[] args)
        {
            if (mLog.IsWarnEnabled)
            {
                message = string.Format(message, args);
                WriteLog(Level.Warn, message, null);
            }
        }

        #endregion
    }

    /// <summary>
    /// 写入日志，提供了具体实现类的快捷操作方法
    /// </summary>
    public static class Logger
    {
        static Dictionary<object, LogHelper> sLoggerManager = new Dictionary<object, LogHelper>();

        static Logger()
        {

           // public static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            //读取log4net的配置信息，必须添加才能正常写入日志信息
            log4net.Config.XmlConfigurator.Configure(new FileInfo(System.Web.HttpContext.Current.Server.MapPath(@"~/Log4Net.config")));
        }

        public static LogHelper GetLogger(string name)
        {
            if (sLoggerManager.Keys.Contains(name) == false)
                sLoggerManager[name] = new LogHelper(name);
            return sLoggerManager[name];
        }

        public static LogHelper GetLogger(Type type)
        {
            if (sLoggerManager.Keys.Contains(type) == false)
                sLoggerManager[type] = new LogHelper(type);
            return sLoggerManager[type];
        }

        #region Debug

        /// <summary>
        /// 向日志文件中写入调试信息
        /// </summary>
        /// <param name="message">写入的信息内容</param>
        /// <param name="exception">错误类型</param>
        public static void Debug(object message, Exception exception)
        {
            Logger.GetLogger("Debug").Debug(message, exception);
        }

        /// <summary>
        /// 向日志文件中写入调试信息
        /// </summary>
        /// <param name="message">写入的信息内容</param>
        public static void Debug(object message)
        {
            Logger.GetLogger("Debug").Debug(message);
        }

        /// <summary>
        /// 向日志文件中写入调试信息
        /// </summary>
        /// <param name="exception">写入的错误类型</param>
        public static void Debug(Exception exception)
        {
            Logger.GetLogger("Debug").Debug(exception);
        }

        /// <summary>
        /// 向日志文件中写入调试信息
        /// </summary>
        /// <param name="message">写入复合格式字符串信息内容</param>
        /// <param name="args">包含零个或多个要格式化的对象的 System.Object 数组</param>
        public static void DebugFormat(string message, params object[] args)
        {
            Logger.GetLogger("Debug").DebugFormat(message, args);
        }

        #endregion

        #region Error

        /// <summary>
        /// 向日志文件中写入错误信息
        /// </summary>
        /// <param name="exception">错误类型</param>
        public static void Error(Exception exception)
        {
            Logger.GetLogger("Error").Error(exception);
        }

        /// <summary>
        /// 向日志文件中写入错误信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        /// <param name="exception">错误类型</param>
        public static void Error(object message, Exception exception)
        {
            Logger.GetLogger("Error").Error(message, exception);
        }

        /// <summary>
        /// 向日志文件中写入错误信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        public static void Error(object message)
        {
            Logger.GetLogger("Error").Error(message);
        }

        /// <summary>
        /// 向日志文件中写入错误信息
        /// </summary>
        /// <param name="message">写入复合格式字符串信息内容</param>
        /// <param name="args">包含零个或多个要格式化的对象的 System.Object 数组</param>
        public static void ErrorFormat(string message, params object[] args)
        {
            Logger.GetLogger("Error").ErrorFormat(message, args);
        }

        #endregion

        #region Fatal

        /// <summary>
        /// 向日志文件中写入致命错误信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        /// <param name="exception">错误类型</param>
        public static void Fatal(object message, Exception exception)
        {
            Logger.GetLogger("Fatal").Fatal(message, exception);
        }

        /// <summary>
        /// 向日志文件中写入致命错误信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        public static void Fatal(object message)
        {
            Logger.GetLogger("Fatal").Fatal(message);
        }

        /// <summary>
        /// 向日志文件中写入致命错误信息
        /// </summary>
        /// <param name="exception">错误类型</param>
        public static void Fatal(Exception exception)
        {
            Logger.GetLogger("Fatal").Fatal(exception);
        }

        /// <summary>
        /// 向日志文件中写入致命错误信息
        /// </summary>
        /// <param name="message">写入复合格式字符串信息内容</param>
        /// <param name="args">包含零个或多个要格式化的对象的 System.Object 数组</param>
        public static void FatalFormat(string message, params object[] args)
        {
            Logger.GetLogger("Fatal").FatalFormat(message, args);
        }

        #endregion

        #region Info

        /// <summary>
        /// 向日志文件中写入一般信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        /// <param name="exception">错误类型</param>
        public static void Info(object message, Exception exception)
        {
            Logger.GetLogger("Info").Info(message, exception);
        }

        /// <summary>
        /// 向日志文件中写入一般信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        public static void Info(object message)
        {
            Logger.GetLogger("Info").Info(message);
        }

        /// <summary>
        /// 向日志文件中写入一般信息
        /// </summary>
        /// <param name="exception">错误类型</param>
        public static void Info(Exception exception)
        {
            Logger.GetLogger("Info").Info(exception);
        }

        /// <summary>
        /// 向日志文件中写入一般信息
        /// </summary>
        /// <param name="message">写入复合格式字符串信息内容</param>
        /// <param name="args">包含零个或多个要格式化的对象的 System.Object 数组</param>
        public static void InfoFormat(string message, params object[] args)
        {
            Logger.GetLogger("Info").InfoFormat(message, args);
        }

        #endregion


        /// <summary>
        /// 向日志文件中写入警告信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        /// <param name="exception">错误类型</param>
        public static void Warn(object message, Exception exception)
        {
            Logger.GetLogger("Warn").Warn(message, exception);
        }

        /// <summary>
        /// 向日志文件中写入警告信息
        /// </summary>
        /// <param name="message">写入的内容</param>
        public static void Warn(object message)
        {
            Logger.GetLogger("Warn").Warn(message);
        }

        /// <summary>
        /// 向日志文件中写入警告信息
        /// </summary>
        /// <param name="exception">错误类型</param>
        public static void Warn(Exception exception)
        {
            Logger.GetLogger("Warn").Warn(exception);
        }

        /// <summary>
        /// 向日志文件中写入警告信息
        /// </summary>
        /// <param name="message">写入复合格式字符串信息内容</param>
        /// <param name="args">包含零个或多个要格式化的对象的 System.Object 数组</param>
        public static void WarnFormat(string message, params object[] args)
        {
            Logger.GetLogger("Warn").WarnFormat(message, args);
        }
    }
}

