using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Untility;

namespace  Common
{
    public static class ParseErrorCode
    {
        public static string GetErrorInfo(string code)
        {
            try
            {
                code = code.Trim();
                string info = "";
                string screenState = "";
                string screenRow = "";
                string screenColumn = "";
                string screenDetailState = "";
                string content = "";
                if (code.Length == 5)
                {
                    screenState = code[0].ToString();                
                    if (screenState=="8")
                    {
                        screenRow = code[1].ToString();
                        screenColumn = code[2].ToString();
                        screenDetailState = code[3].ToString() + code[4].ToString();
                        //屏幕详细状态
                        switch (screenDetailState)
                        {
                            case "10":
                                content = "没有返回数据";
                                break;
                            case "20":
                                content = "背光灯未开";
                                break;
                            case "30":
                                content = "无信号源";
                                break;
                            case "41":
                                content = "信号源错误,信号源应为DVI,实际为VGA";
                                break;
                            case "42":
                                content = "信号源错误,信号源应为DVI,实际为HDMI";
                                break;
                            //case "43":
                            //    info = "有信号，信号是DVI";
                            //    break;
                            case "44":
                                content = "信号源错误,信号源应为DVI,实际为VIDEO01";
                                break;
                            case "45":
                                content = "信号源错误,信号源应为DVI,实际为VIDEO02";
                                break;
                            case "46":
                                content = "信号源错误,信号源应为DVI,实际为VIDEO03";
                                break;
                            case "47":
                                content = "信号源错误,信号源应为DVI,实际为VIDEO04";
                                break;
                            case "48":
                                content = "信号源错误,信号源应为DVI,实际为SV";
                                break;
                            default:
                                content = "未定义的状态码";
                                break;
                        }
                    }
                    else
                    {
                        info = "屏幕状态码有误,无法解析";
                        return info;
                    }               
                }
                else
                {
                    info = "屏幕状态码有误";
                    return info;
                }

                if (code == "80000" || code == "80043")
                {
                    info = "屏幕正常";                               
                }
                else
                {
                    info = string.Format("{0}行{1}列故障，故障信息：{2}", screenRow, screenColumn, content);
                }
                return info;
            }
            catch (Exception ex)
            {
                WriteLog.WriteErrorLogToFile(string.Format("解析错误码异常-异常信息：{0},{1}", ex.Message.ToString(), DateTime.Now.ToString("yyyy-MM-DD HH:mm:ss")));
                throw ex;
            }


        }
    }
}
