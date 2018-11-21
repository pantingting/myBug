using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CHEER.PresentationLayer.CommonUse
{
    public static class EnumExtensions
    {
        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }
    }

    public class Enumeration
    {
        /// <summary>
        /// 订单状态
        /// </summary>
        public enum orderStatus
        {
            /// <summary>
            /// 已提交|待接单
            /// </summary>
            PENDING = 0,
            /// <summary>
            /// 配送中
            /// </summary>
            DISTRIBUTION = 1,
            /// <summary>
            /// 已完成
            /// </summary>
            COMPLETED = 2,
            /// <summary>
            /// 已取消
            /// </summary>
            CANCELED = 3
        }

        /// <summary>
        /// 订单支付状态
        /// </summary>
        public enum payStatus
        {
            /// <summary>
            /// 已支付
            /// </summary>
            PAID = 1,
            /// <summary>
            /// 未支付
            /// </summary>
            NOTPAID = 0
        }
    }
}