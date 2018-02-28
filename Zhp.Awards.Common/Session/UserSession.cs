
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhp.Awards.Model;


namespace  Common
{
    public class UserSession
    {
        #region Field
        private List<SYS_MENU_MODEL> _menulist;  // 用户所拥有菜单集合
        //private SYS_MECHANISM_MODEL _mechanism;  //部门信息
        private SYS_ROLE_MODEL _role;            //角色信息 
        private Dictionary<string, List<string>> _permissions = new Dictionary<string, List<string>>();// 权限信息
        public SYS_USER_MODEL userInfo { get; set; }
        #endregion
    }
}
