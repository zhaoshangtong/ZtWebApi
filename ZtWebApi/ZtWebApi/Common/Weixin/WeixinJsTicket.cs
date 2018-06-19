using DbModels.WeixinModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using Utility.Log;

namespace ZtWebApi.Common.Weixin
{
    /// <summary>
    /// 微信JSAPI
    /// </summary>
    public class WeixinJsTicket
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AccessToken = null;
        public string Ticket = null;
        /// <summary>
        /// token过期时间
        /// </summary>
        public string TicketTime = null;
        public bool rootSuccess = false;
        public string resource = "";
        public string rootErrMsg = "";
        public WeixinJsTicket(string _access_token, string _ticket, string _ticket_time, int _weixin_id)
        {
            //需要查询对应的微信信息
            weixin_list weixin = new weixin_list();

            WeixinAPI api = new WeixinAPI(weixin.appid, weixin.appsecret, weixin.access_token, weixin.access_token_time.ToString(), weixin.weixin_id);
            rootSuccess = true;
            LogHelper.Info("微信公众号accesstoken获取成功：" + api.AccessToken);
            AccessToken = api.AccessToken;
            Ticket = _ticket;
            TicketTime = _ticket_time;
            //检查授权有效期，如果凭证为空，或者已失效，则需要请求授权
            DateTime time = DateTime.Now;
            if (!string.IsNullOrEmpty(_ticket_time))
            {
                time = DateTime.Parse(_ticket_time);
            }
            int comtime = (int)time.Subtract(DateTime.Now).TotalMinutes;

            //凭证在5分钟以内将失效，则重新获取凭证
            if (comtime < 5)
            {
                string s = Util.MethodGET("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + AccessToken + "&type=jsapi", "UTF-8");
                try
                {
                    JObject jo = JObject.Parse(s);
                    Ticket = jo["ticket"].ToString();
                    LogHelper.Info("微信公众号ticket获取成功：" + Ticket);
                    TicketTime = System.DateTime.Now.AddSeconds(double.Parse(jo["expires_in"].ToString())).ToString();

                    //将获取的最新 Jsapi_Ticket 保存到数据库中

                    //if (weixin_applet != null)
                    //{
                    //    weixin_applet.jsapi_ticket = Ticket;
                    //    weixin_applet.jsapi_ticket_time = Convert.ToDateTime(TicketTime);
                    //    //baseBLL.Update(weixin_applet);
                    //    db.Update("weixin", weixin_applet, "id=" + _weixin_applet_id);
                    //    LogHelper.Info("更新微信Ticket：" + Ticket);
                    //}
                }
                catch (Exception e)
                {
                    rootSuccess = false;
                    rootErrMsg = e.Message;
                }
                resource = s;
            }
        }


        #region jsapi
        /// <summary>
        /// jsapi获取签名
        /// </summary>
        /// <param name="url">需要签名的页面的url</param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public weixin_jsapi_config QuerySignature(string url,string appid)
        {
            //需要签名的页面的url
            //string urlstr = Util.getServerPath() + "/Home/TemplateView";
            string urlstr = url;
            //获取最新的jsapi_tiket与accesstoken
            string jsapi = Ticket;
            //取签名的时间戳可以是随机数
            long timestamp = Util.getLongTime();
            //随机字符串(可以写方法随机生成)
            string nonceStr = Sys.getRandomCode(26);
            //签名算法
            string signature = GetSignature(jsapi, nonceStr, timestamp, urlstr);

            return new weixin_jsapi_config() { appId = appid, nonceStr = nonceStr, timestamp = timestamp, signature = signature };
        }
        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="jsapi">ticket</param>
        /// <param name="nonceStr">随机字符串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="urlstr">需要签名的url</param>
        private string GetSignature(string jsapi, string nonceStr, long timestamp, string urlstr)
        {
            StringBuilder str = new StringBuilder();
            string string1 = "jsapi_ticket=" + jsapi + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + urlstr;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(string1, "SHA1").ToLower();
        }
        #endregion

    }
}