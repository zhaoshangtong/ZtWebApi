using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModels.WeixinModels
{
    /// <summary>
    /// JSAPI支付参数
    /// </summary>
    public class weixin_jsapi_pay
    {
        public long timestamp { get; set; }
        public string nonceStr { get; set; }
        public string package { get; set; }
        public string signType { get; set; }
        public string paySign { get; set; }
        public bool is_pay { get; set; }
    }
}
