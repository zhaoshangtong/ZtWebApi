using DbModels.WeixinModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Utility.Log;
using ZtWebApi.Common;
using ZtWebApi.Common.Weixin;

namespace ZtWebApi.Controllers.Weixin
{
    /// <summary>
    /// 微信支付：先保存订单数据，再统一下单，再调回调接口更新订单数据
    /// </summary>
    public class WeixinPayController : ApiController
    {
        /// <summary>
        /// 统一下单
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="appcode"></param>
        /// <param name="seedName"></param>
        /// <param name="notifyUrl"></param>
        /// <param name="weixin"></param>
        /// <returns></returns>
        public weixin_jsapi_pay GetPayParaData(string openid, string appcode, string seedName, string notifyUrl, weixin_list weixin)
        {
            //微信公众号统一下单 (需要将数据库weixin的信息补全)
            WeixinPay weixinPay = new WeixinPay(System.Web.HttpContext.Current, weixin.appid, weixin.appsecret, weixin.pay_mch_id, weixin.pay_partner_key);
            //string payNumber = Common.Orderform.Orderform.getNewPayNumer(orderform.uid.ToString());
            string payNumber = "";
            //微信支付，统一下单（与微信支付订单一样）
            weixin_unified_order unifiedOrder = new weixin_unified_order();
            //int total_fee = int.Parse(((orderform.money + orderform.freight) * 100).To<double>().ToString("F0"));
            int total_fee = 0;
            unifiedOrder.nonce_str = Sys.getRandomCode(26);
            unifiedOrder.body = seedName + "(" + appcode + "-Applet)";
            unifiedOrder.out_trade_no = payNumber;//商户单号使用支付单号
            unifiedOrder.total_fee = total_fee;
            unifiedOrder.spbill_create_ip = Util.GetUserIp();
            unifiedOrder.notify_url = notifyUrl; //回调
            unifiedOrder.trade_type = "JSAPI";
            unifiedOrder.openid = openid;

            //获得统一下单返回的支付单信息
            weixin_unified_order_return unifiedOrderReturn = weixinPay.UnifiedOrder(unifiedOrder);
            LogHelper.Info("统一下单返回的支付单信息:" + Newtonsoft.Json.JsonConvert.SerializeObject(unifiedOrderReturn));
            if ("SUCCESS".Equals(unifiedOrderReturn.return_code))
            {
                //准备获取支付信息的签名参数
                Hashtable parameter = new Hashtable();
                parameter.Add("appId", unifiedOrderReturn.appid);
                parameter.Add("timeStamp", Util.getLongTime().ToString());
                parameter.Add("nonceStr", unifiedOrderReturn.nonce_str);
                parameter.Add("package", "prepay_id=" + unifiedOrderReturn.prepay_id);
                parameter.Add("signType", "MD5");
                string paysign = weixinPay.Sign(parameter);
                weixin_jsapi_pay weixinPayInfo = new weixin_jsapi_pay()
                {
                    timestamp = Util.getLongTime(),
                    nonceStr = unifiedOrderReturn.nonce_str,
                    package = "prepay_id=" + unifiedOrderReturn.prepay_id,
                    signType = "MD5",
                    paySign = paysign,
                    is_pay = false
                };
                LogHelper.Info("获得了统一下单数据:" + Newtonsoft.Json.JsonConvert.SerializeObject(weixinPayInfo));

                return weixinPayInfo;
            }
            else
            {
                ////删除订单
                //orderform.isdelete = 1;
                //orderform.remarks = "小程序统一下单失败，自动删除";
                //db.Update("orderform", orderform, "id=" + orderform_id);
            }
            return null;
        }


        /// <summary>
        /// 购买回调
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string FastBuyPayReceive()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            weixin_pay_notify payNotify = new weixin_pay_notify();
            try
            {
                payNotify = WeixinPay.getPayNotify(request);
                if ("SUCCESS".Equals(payNotify.result_code))
                {
                    //BaseBLL<pay_record> payRecordBll = new BaseBLL<pay_record>();
                    //pay_record payRecord = payRecordBll.Find(x => x.pay_number == payNotify.out_trade_no);
                    //if (payRecord != null)
                    //{
                    //    //标记支付记录回调成功
                    //    double notify_money = double.Parse(payNotify.total_fee.ToString()) / 100;
                    //payRecord.notify_money = notify_money;
                    //payRecord.pay_success = 1;
                    //payRecord.transaction_id = payNotify.transaction_id;//微信支付订单号
                    //payRecordBll.Update(payRecord);
                    //Orderform.UpdateStatusPayByUnionnumber(payRecord.unionnumber);
                    //}
                    //else
                    //{
                    //    LogHelper.Error("未找到原始支付记录，无法回写支付状态，paynotifyvo.out_trade_no=" + payNotify.out_trade_no + "！");
                    //}
                }
                else
                {
                    LogHelper.Error("return_code = " + payNotify.result_code);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
            string ret = "<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>";
            LogHelper.Info("FastBuyPayReceive return:" + ret);
            HttpContext.Current.Response.Write(ret);
            HttpContext.Current.Response.End();
            return ret;
        }

    }
}
