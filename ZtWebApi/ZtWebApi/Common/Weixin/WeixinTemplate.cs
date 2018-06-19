using DbModels.WeixinModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utility.Log;

namespace ZtWebApi.Common.Weixin
{
    /// <summary>
    /// 微信公众号发模板消息
    /// </summary>
    public class WeixinTemplate
    {
        /// <summary>
        /// 组织发送模板消息内容
        /// </summary>
        /// <param name="weixin">微信公众号信息</param>
        /// <param name="templateCode">模板消息：OPENTM200654400</param>
        /// <param name="openid">用户在公众号下的openid</param>
        /// <param name="data">{{first.DATA}} 商家名称：{{keyword1.DATA}} 商家电话：{{keyword2.DATA}} 订单号：{{keyword3.DATA}} 状态：{{keyword4.DATA}} 总价：{{keyword5.DATA}} {{remark.DATA}}</param>
        /// <returns></returns>
        public static string sendWeixinTemplate(weixin_list weixin, string templateCode, string openid, string data, string seedName, string appcode, string weixin_applet_path)
        {
            //BaseBLL<weixin_template> templateBll = new BaseBLL<weixin_template>();
            //weixin_template Template = templateBll.Find(x => x.template_code == templateCode && x.weixin_id == weixin.id);
            //组织发送的数据
            string[] valueArray = data.Split(';');
            JObject _data = JObject.Parse("{}");
            for (int i = 0; i < valueArray.Length; i++)
            {
                if (i == 0)
                {
                    JObject subObject = new JObject(
                       new JProperty("value", valueArray[i]),
                       new JProperty("color", "#173177")
                    );
                    _data.Add("first", subObject);
                }
                else if (i == valueArray.Length - 1)
                {
                    JObject subObject = new JObject(
                       new JProperty("value", valueArray[i]),
                       new JProperty("color", "#173177")
                    );
                    _data.Add("remark", subObject);
                }
                else
                {
                    JObject subObject = new JObject(
                       new JProperty("value", valueArray[i]),
                       new JProperty("color", "#333")
                    );
                    _data.Add("keyword" + (i), subObject);
                }

            }

            WeixinAPI weixinXApi = new WeixinAPI(weixin.appid, weixin.appsecret, weixin.access_token, weixin.access_token_time.ToString(), weixin.weixin_id);
            JObject postData = JObject.Parse("{}");
            postData.Add("touser", openid);
            postData.Add("template_id", "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            postData.Add("url", "https://tlkjbase.whtlkj.net/Home/TemplateView?orderform_id=123456" + "&openid=" + openid + "&weixin_id=" + weixin.weixin_id + "&appcode=" + appcode + "&seedName=" + seedName + "&weixin_applet_path=" + weixin_applet_path + "&notifyUrl=" + Util.getServerPath());
            postData.Add("data", _data);
            return weixinXApi.sendTemplateByPublic(postData.ToString());
        }

    }
}