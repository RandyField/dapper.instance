using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhp.Awards.Model
{
    public class SYS_ROLE_MODEL
    {
        #region Field
        private decimal _id;  //主键标识
        private string _name; //角色名称
        private string _permissions;//权限集合
        private decimal _levelnum;//角色层级
        private decimal _type;//角色类型
        private decimal _state;//状态
        private string _operater;//操作人员账号
        private DateTime _operationTime;//操作时间 
        #endregion
    }
}
