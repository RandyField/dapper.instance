using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhp.Awards.Model
{
    public class SYS_MENU_MODEL 
    {
        #region Field
        private decimal _id;             // 菜单标识
        private decimal _parentid;       // 父级菜单标识
        private string _navigateurl; // 链接地址
        private string _name;        // 菜单名称
        private decimal _mid;            // 所属模块
        private decimal sortcode;        // 排序
        private decimal _state;          // 状态 1：正常 0：停用
        private string _target;      // 打开方式
        private string _ico;         // 图标地址
        private decimal _levelnum;    //菜单级别
        #endregion
    }
}
