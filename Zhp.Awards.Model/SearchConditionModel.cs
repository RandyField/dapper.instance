using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhp.Awards.Model
{
    public class SearchConditionModel
    {
        #region Field
        private Hashtable conditionTable = new Hashtable();  //查询条件
        #endregion

        #region Property
        /// <summary>
        /// 查询条件列表
        /// </summary>
        public Hashtable ConditionTable
        {
            get { return this.conditionTable; }
        }
        #endregion

        #region Constructor

        #endregion
    }
}
