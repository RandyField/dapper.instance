using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Untility
{
    /// <summary>
    /// jsonHeler
    /// </summary>
    public static class JsonHelper
    {
        public static T DeserializeJson<T>(string jsonStr)
        {
            try
            {           
                T model = JsonConvert.DeserializeObject<T>(jsonStr);
                return model;
            }
            catch (Exception ex)
            {
                WriteLog.WriteErrorLogToFile(string.Format("json序列化异常-异常信息：{0},{1}", ex.Message.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),true);                  
                throw ex;
            }
            
        }  
    }
}
