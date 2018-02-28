using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    /// <summary>
    /// 动态类，每个实例使用单独session
    /// </summary>
    public class HttpHelper
    {
        public CookieContainer cookie = new CookieContainer();
        /// <summary>
        /// post请求返回html
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        /// 
    
        
        public string HttpPost(string Url, string postDataStr)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";

            //设置参数的编码格式，解决中文乱码
            byte[] byteArray = Encoding.UTF8.GetBytes(postDataStr);

            //设置request的MIME类型及内容长度
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            HttpWebResponse response = null;

            //打开request字符流
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            try
            {
                this.SetCertificatePolicy();

                //返回请求数据
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (System.Exception ex)
            {
            }
            if (response != null)
            {
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            else
            {
                return "error"; //post请求返回为空
            }
        }


        public string HttpPostForm(string Url, string postDataStr)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";

            //设置参数的编码格式，解决中文乱码
            byte[] byteArray = Encoding.UTF8.GetBytes(postDataStr);

            //设置request的MIME类型及内容长度
            //request.ContentType = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            HttpWebResponse response = null;

            //打开request字符流
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            try
            {
                this.SetCertificatePolicy();
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (System.Exception ex)
            {
            }
            if (response != null)
            {
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            else
            {
                return "error"; //post请求返回为空
            }
        }
        /// <summary>
        /// get请求获取返回的html
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string HttpGet(string Url, string Querydata)
        {
            string retString = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                //初始化
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (Querydata == "" ? "" : "?") + Querydata);

                //请求的方法
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.CookieContainer = cookie;
                this.SetCertificatePolicy();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();     
            }
            catch (System.Exception ex)
            {
                Common.Helper.Logger.Info(string.Format("请求被中止：未能创建SSL/TLS安全通道。信息{0},retString:{1}", ex.ToString(), retString));   
            }
            return retString;
        }
        /// <summary>
        /// 获得响应中的图像
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Stream GetResponseImage(string url)
        {
            Stream resst = null;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.KeepAlive = true;
                req.Method = "GET";
                req.AllowAutoRedirect = true;
                req.CookieContainer = cookie;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.Timeout = 50000;
                Encoding myEncoding = Encoding.GetEncoding("UTF-8");
                this.SetCertificatePolicy();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                resst = res.GetResponseStream();
                return resst;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 正则获取匹配的第一个值
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public string getStringByRegex(string html, string pattern)
        {
            Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matchs = re.Matches(html);
            if (matchs.Count > 0)
            {
                return matchs[0].Groups[1].Value;
            }
            else
                return "";
        }
        /// <summary>
        /// 正则验证返回的response是否正确
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public bool verifyResponseHtml(string html, string pattern)
        {
            Regex re = new Regex(pattern);
            return re.IsMatch(html);
        }
        //注册证书验证回调事件，在请求之前注册
        private void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback
                       += RemoteCertificateValidate;
        }
        /// <summary>  
        /// 远程证书验证，固定返回true 
        /// </summary>  
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert,
            X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
    }
}