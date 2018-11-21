using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHEER.PresentationLayer.Models
{
    public class AuthModel
    {
    }

    public class deviceInfo
    {
        public string device_num { get; set; } = "1";
        public string op_type { get; set; } = "0";
        public string product_id { get; set; }
        public List<deviceConfigure> device_list = new List<deviceConfigure>();
    }

    public class deviceConfigure
    {
        public string id { get; set; }
        public string mac { get; set; }
        public string connect_protocol { get; set; } = "4";
        public string auth_key { get; set; } = "8c82365499da40648de0eb705e4229dc";
        public string close_strategy { get; set; } = "1";
        public string conn_strategy { get; set; } = "1";
        public string crypt_method { get; set; } = "1";
        public string auth_ver { get; set; } = "1";
        public string manu_mac_pos { get; set; } = "-1";

        public string ser_mac_pos { get; set; } = "-2";
        public string ble_simple_protocol { get; set; } = "0";
    }

    // 设备授权
    public class deviceAuthorization
    {
        public List<resp> resp = new List<resp>();
    }

    public class resp
    {
        public base_info base_Info { get; set; } = new base_info();
        public string errcode { get; set; }
        public string errmsg { get; set; }
    }

    public class base_info
    {
        public string device_type { get; set; }
        public string device_id { get; set; }
    }
}