using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Web;

namespace Untility
{
    /// <summary>
    ///  XML解析帮助类
    /// </summary>
    public static class XmlParseHelper
    {
        /// <summary>
        /// exe中寻找xml路径
        /// </summary>
        public static string xmlPath = ConfigurationManager.AppSettings["xmlRoute"];

        /// <summary>
        /// 设置xml路径
        /// </summary>
        /// <param name="pathConfigName">xml路径在配置文件中</param>
        /// <param name="isWeb">是否是web项目</param>
        public static void SetPath(string pathConfigName, bool isWeb)
        {
            if (isWeb)
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["xmlRoute"]))
                {
                    xmlPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["xmlRoute"]);
                }
            }
            else
            {
                xmlPath = HttpContext.Current.Server.MapPath(xmlPath);
            }
        }

        /// <summary>
        /// 静态构造方法
        /// web中寻找xml路径
        /// </summary>
        //static XmlParseHelper()
        //{
        //    if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["xmlRoute"]))
        //    {
        //        xmlPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["xmlRoute"]);              
        //    }
        //    else
        //    {
        //        xmlPath = HttpContext.Current.Server.MapPath(xmlPath);
        //    }
        //}


        /// <summary>
        /// 解析xml节点 获取所有二级节点值
        /// </summary>
        /// <param name="firstNodeName">一级节点名</param>
        /// <param name="secondNodeName">二级节点名</param>
        /// <returns></returns>
        public static List<string> GetNodeList(string firstNodeName, string secondNodeName)
        {
            List<string> list = null;

            //定义并从xml文件中加载节点（根节点）
            XElement rootNode = XElement.Load(xmlPath);

            //查询语句：获取根节点下子节点(此时子节点可以跨层次：孙节点、重孙节点)
            IEnumerable<XElement> targetNodes = from target in rootNode.Descendants(firstNodeName).Descendants(secondNodeName)
                                                select target;
            if (targetNodes.Count() > 0)
            {
                list = new List<string>();
                foreach (XElement node in targetNodes)
                {
                    list.Add(node.Value);
                }
            }

            return list;
        }


        /// <summary>
        /// 解析xml3级节点下的值
        /// </summary>
        /// <param name="firstNodeName"></param>
        /// <param name="secondNodeName"></param>
        /// <param name="thirdNodeName"></param>
        /// <returns></returns>
        public static List<string> GetNodeList(string firstNodeName, string secondNodeName, string thirdNodeName)
        {
            List<string> list = null;

            //定义并从xml文件中加载节点（根节点）
            XElement rootNode = XElement.Load(xmlPath);

            //查询语句：获取根节点下子节点(此时子节点可以跨层次：孙节点、重孙节点)
            IEnumerable<XElement> targetNodes = from target in rootNode.Descendants(firstNodeName).Descendants(secondNodeName).Descendants(thirdNodeName)
                                                select target;
            if (targetNodes.Count() > 0)
            {
                list = new List<string>();
                foreach (XElement node in targetNodes)
                {
                    list.Add(node.Value);
                }
            }

            return list;
        }


        /// <summary>
        /// 解析xml节点 获取一个二级节点值
        /// </summary>
        /// <param name="firstNodeName"></param>
        /// <param name="secondNodeName"></param>
        /// <returns></returns>
        public static string GetSingNode(string firstNodeName, string secondNodeName)
        {
            string value = "";
            //定义并从xml文件中加载节点（根节点）
            XElement rootNode = XElement.Load(xmlPath);

            //查询语句：获取根节点下子节点(此时子节点可以跨层次：孙节点、重孙节点)
            IEnumerable<XElement> targetNodes = from target in rootNode.Descendants(firstNodeName).Descendants(secondNodeName)
                                                select target;
            if (targetNodes.Count() > 0)
            {
                XElement node = (XElement)targetNodes.First();
                value = node.Value;
            }

            return value;
        }

        /// <summary>
        /// 解析xml节点 获取一个1级节点值
        /// </summary>
        /// <param name="firstNodeName"></param>
        /// <returns></returns>
        public static string GetSingNode(string firstNodeName)
        {
            string value = "";
            //定义并从xml文件中加载节点（根节点）
            XElement rootNode = XElement.Load(xmlPath);

            //查询语句：获取根节点下子节点(此时子节点可以跨层次：孙节点、重孙节点)
            IEnumerable<XElement> targetNodes = from target in rootNode.Descendants(firstNodeName)
                                                select target;
            if (targetNodes.Count() > 0)
            {
                XElement node = (XElement)targetNodes.First();
                value = node.Value;
            }

            return value;
        }
    }
}
