﻿using System;

namespace CHEER.Service.Base
{
    /// <summary>
    /// 工作项配置
    /// </summary>
    public abstract class ServiceConfig
    {
        #region 子类必需实现的抽象属性

        /// <summary>
        /// 工作项说明
        /// </summary>
        public abstract string Description
        {
            get;
        }

        /// <summary>
        /// 工作项是否开启
        /// </summary>
        public abstract string Enabled
        {
            get;
        }

        /// <summary>
        /// 工作项程序集
        /// </summary>
        public abstract string Assembly
        {
            get;
        }

        /// <summary>
        /// 工作项执行间隔时间
        /// </summary>
        public abstract int Interval
        {
            get;
        }

        #endregion

        #region 扩展属性

        //可扩展

        #endregion
    }
}
