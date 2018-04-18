using Common.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Zhp.Awards.Activity.Controllers
{
    public class UploadImgController : Controller
    {
        // GET: UploadImg
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

            if (files.Count == 0) 
            {
                return Json("Faild", JsonRequestBehavior.AllowGet);
            }

            MD5 md5Hasher = new MD5CryptoServiceProvider();

            /*计算指定Stream对象的哈希值*/
            byte[] arrbytHashValue = md5Hasher.ComputeHash(files[0].InputStream);

            /*由以连字符分隔的十六进制对构成的String，其中每一对表示value中对应的元素；例如“F-2C-4A”*/
            string strHashData = System.BitConverter.ToString(arrbytHashValue).Replace("-", "");

            //文件扩展名
            string FileEextension = Path.GetExtension(files[0].FileName);

            //上传日期
            string uploadDate = DateTime.Now.ToString("yyyyMMdd");

            //相对路径
            string virtualPath = string.Format("/Upload/img/{0}/{1}{2}", uploadDate, strHashData, FileEextension);

            //Logger.Error(string.Format("相对路径地址:{0}", virtualPath));

            //服务器绝对路径
            string fullFileName = Server.MapPath(Request.ApplicationPath + virtualPath);

            //Logger.Error(string.Format("服务器绝对路径:{0}", fullFileName));

            //服务器URI
            string server = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + Request.ApplicationPath;

            //Logger.Error(string.Format("服务器URI:{0}", server));
            //img URI
            string imguri =server+virtualPath;

            //Logger.Error(string.Format("img URI:{0}", imguri));

            //创建文件夹，保存文件
            string path = Path.GetDirectoryName(fullFileName);

            Directory.CreateDirectory(path);
            
            if (!System.IO.File.Exists(fullFileName))
            {
                files[0].SaveAs(fullFileName);
            }

            //文件名称
            string fileName = files[0].FileName.Substring(files[0].FileName.LastIndexOf("\\") + 1, files[0].FileName.Length - files[0].FileName.LastIndexOf("\\") - 1);

            //文件大小
            string fileSize = GetFileSize(files[0].ContentLength);

            return Json(new { FileName = fileName, FilePath = virtualPath, FileSize = fileSize,FileUri=imguri }, "text/html", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string GetFileSize(long bytes)
        {
            long kblength = 1024;
            long mbLength = 1024 * 1024;
            if (bytes < kblength)
                return bytes.ToString() + "B";
            if (bytes < mbLength)
                return decimal.Round(decimal.Divide(bytes, kblength), 2).ToString() + "KB";
            else
                return decimal.Round(decimal.Divide(bytes, mbLength), 2).ToString() + "MB";
        }
    }
}