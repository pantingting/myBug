
using CHEER.PresentationLayer.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CHEER.PresentationLayer.Logic
{
    public class HelperDelegate
    {
        /// <summary>
        /// 空的Delegate,使用 () => 即可
        /// </summary>
        public delegate void EmptyDelegate();

        /// <summary>
        /// String的Delegate,使用 str => 即可
        /// </summary>
        public delegate void StringDelegate(string str);

        /// <summary>
        /// Exception的Delegate,使用 ex => 即可
        /// </summary>
        public delegate void ExceptionDelegate(Exception ex);

        /// <summary>
        /// PersistBroker的Delegate,使用 _broker => 即可
        /// </summary>
        public delegate void BrokerDelegate(PersistBroker broker);

        /// <summary>
        /// DataTable的Delegate,使用 dt => 即可
        /// </summary>
        public delegate void DataTableDelegate(DataTable dt);

        /// <summary>
        /// DataSet的Delegate,使用 ds => 即可
        /// </summary>
        public delegate void DataSetDelegate(DataSet ds);
    }
}