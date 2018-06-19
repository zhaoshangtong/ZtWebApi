using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Utility;
using Utility.Log;

namespace ZtWebApi.Common.Weixin
{
    /// <summary>
    /// 微信公众号操作类
    /// </summary>
    public class WeixinAPI
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId = null;
        /// <summary>
        /// AppSecret
        /// </summary>                               
        public string AppSecret = null;
        /// <summary>
        /// AccessToken
        /// </summary>                                      
        public string AccessToken = null;
        /// <summary>
        /// token过期时间
        /// </summary>
        public string AccessTokenTime = null;
        /// <summary>
        /// 
        /// </summary>
        public bool rootSuccess = false;
        /// <summary>
        /// 
        /// </summary>
        public string resource = "";
        /// <summary>
        /// 
        /// </summary>
        public string rootErrMsg = "";
        /// <summary>
        /// 
        /// </summary>
        public static HttpContext httpcontext = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_appid"></param>
        /// <param name="_appsecret"></param>
        public WeixinAPI(string _appid, string _appsecret)
        {
            AppId = _appid;
            AppSecret = _appsecret;

        }
        /// <summary>
        /// 获取并更新微信公众号的access_token
        /// </summary>
        /// <param name="_appid"></param>
        /// <param name="_appsecret"></param>
        /// <param name="_access_token"></param>
        /// <param name="_access_token_time"></param>
        /// <param name="_weixin_id"></param>
        public WeixinAPI(string _appid, string _appsecret, string _access_token, string _access_token_time, int _weixin_id)
        {
            rootSuccess = true;
            AppId = _appid;
            AppSecret = _appsecret;
            AccessToken = _access_token;
            AccessTokenTime = _access_token_time;
            //检查授权有效期，如果凭证为空，或者已失效，则需要请求授权
            DateTime time = DateTime.Now;
            if (!string.IsNullOrEmpty(_access_token_time))
            {
                time = DateTime.Parse(_access_token_time);
            }
            int comtime = (int)time.Subtract(DateTime.Now).TotalMinutes;
            //凭证在5分钟以内将失效，则重新获取凭证
            if (comtime < 5)
            {
                string s = Util.MethodGET("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppId + "&secret=" + AppSecret, "UTF-8");
                try
                {
                    JObject jo = JObject.Parse(s);
                    AccessToken = jo["access_token"].ToString();
                    AccessTokenTime = System.DateTime.Now.AddSeconds(double.Parse(jo["expires_in"].ToString())).ToString();

                    //将获取的最新 AccessToken 保存到数据库中
                    #region

                    #endregion

                }
                catch (Exception e)
                {
                    rootSuccess = false;
                    rootErrMsg = e.Message;
                }
                resource = s;

            }


        }


        /// <summary>
        /// code 换取 session_key
        /// </summary>
        public string codeToSession(string jscode)
        {
            string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + AppId + "&secret=" + AppSecret + "&js_code=" + jscode + "&grant_type=authorization_code";
            // return MethodGET(url, "UTF-8");
            return HttpHelper.HttpGet(url, "");
        }



        /// <summary>
        ///采用https协议访问网络
        ///</summary>
        ///<param name="URL">url地址</param>
        ///<param name="strPostdata">发送的数据</param>
        /// <param name="strEncoding"></param>
        /// <param name="filename"></param>
        /// <param name="filefolder"></param>
        public static void MethodJsonPOST(string URL, string strPostdata, string strEncoding, string filename, string filefolder)
        {
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath(filefolder), filename);
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            Encoding encoding = System.Text.Encoding.GetEncoding(strEncoding);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "post";
            request.ContentType = "application/json;charset=" + strEncoding.ToUpper();
            byte[] buffer = encoding.GetBytes(strPostdata);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream sm = response.GetResponseStream();
            System.Drawing.Image img = System.Drawing.Image.FromStream(sm);
            img.Save(filePath);
            sm.Flush();
            sm.Close();
        }



       /// <summary>
       /// 发送模板消息
       /// </summary>
       /// <param name="postData">模板消息参数</param>
       /// <returns></returns>
        public string sendTemplateByPublic(string postData)
        {
            LogHelper.Info("sendTemplate:" + postData);
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + AccessToken;
            return HttpHelper.HttpPost(url, postData);
        }
    }
}