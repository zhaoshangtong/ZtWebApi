using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModels.WeixinModels
{
    /// <summary>
    /// 微信公众号
    /// </summary>
    public class weixin_list
    {
        [Key]
        //自增长
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(AutoGenerateField =true,Name ="微信ID")]
        public int weixin_id { get; set; }
        [Required]
        [StringLength(255,ErrorMessage ="微信名字太长",MinimumLength =1)]
        [Display(AutoGenerateField = true, Name = "微信名称")]
        public string weixin_name { get; set; }
        /// <summary>
        /// 微信公众号的账号
        /// </summary>
        [Required]
        [EmailAddress]
        public string email { get; set; }
        /// <summary>
        /// 微信公众号的密码
        /// </summary>
        [StringLength(255)]
        public string password { get; set; }
        [Required]
        [StringLength(255,ErrorMessage ="Appid太长")]
        public string appid { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Appsecret太长")]
        public string appsecret { get; set; }

        [StringLength(255)]
        public string access_token { get; set; }
        /// <summary>
        /// access_token过期时间
        /// </summary>
        public DateTime? access_token_time { get; set; }
        public string  jsapi_ticket { get; set; }
        public DateTime? jsapi_ticket_time { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string pay_mch_id { get; set; }
        /// <summary>
        /// 商户号api key
        /// </summary>
        public string pay_partner_key { get; set; }
        public string mark { get; set; }
        /// <summary>
        /// 关注人数
        /// </summary>
        public int? user_count { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
    }
}
