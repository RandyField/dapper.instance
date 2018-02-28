
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Zhp.Awards.Model;


namespace  Common
{

    public class AccountInfo
    {
        #region Property
        /// <summary>
        /// 获取用户session
        /// 超时或未登录返回null
        /// </summary>
        public static UserSession UserInfo
        {
            get { return HttpContext.Current.Session["UserInfo"] as UserSession; }
            set { HttpContext.Current.Session.Add("UserInfo", value); }
        }
        #endregion

        /// <summary>
        /// 设置用户Session
        /// </summary>
        /// <param name="user">用户信息</param>
        public static void SetUserSession(SYS_USER_MODEL user)
        {
            if (user == null)
            {
                UserInfo = null;
                return;
            }

            UserSession session = new UserSession();
            session.userInfo = user;

            //session.MechanismList = GetMechanismList(user);
            //session.MechanismList = GetSubMechanismList(user.SSBMBH);
            //SYS_MECHANISM_MODEL m = CacheDictionary.MechanismList.Find(li => li.BMBH == user.SSBMBH);
            //session.MechanismList.Add(m);

            //设置用户角色信息
            //session.Role = SYS_ROLE_BLL.Instance.Dal.FindByID(user.ROLEID);

            //List<SYS_MENU_MODEL> mList = new List<SYS_MENU_MODEL>();

            //设置用户权限信息
            //session.Permissions = GetUserPermissions(session.Role, user.LOGINNAME, ref mList);

            //if (user.LOGINNAME.Trim().ToUpper() == "ADMIN")
            //{
            //    session.MenuList = mList;
            //}
            //else
            //{
            //    //设置用户菜单信息
            //    session.MenuList = GetMenuList(string.Join(",", session.Permissions.Keys), user.LOGINNAME).OrderBy(li => li.SORTCODE).ToList();
            //}

            //设置用户快捷菜单
            //session.QuickMenu = GetUserQuickMenu(user.LOGINNAME);

            UserInfo = session;
        }
    }
}
