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
    /// 微信自定义菜单
    /// </summary>
    public class weixin_menus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int weixin_menus_id { get; set; }
        [Required]
        [ForeignKey("weixin_id")]
        public int weixin_id { get; set; }
        [Required]
        public int weixin_menu_parent_id { get; set; }
        [Required]
        [StringLength(255,ErrorMessage ="菜单名过长")]
        public string weixin_menu_name { get; set; }
        [StringLength(255)]
        public string weixin_menu_type { get; set; }
        [StringLength(64)]
        public string weixin_menu_key { get; set; }
        [Url]
        [StringLength(255)]
        public string weixin_menu_url { get; set; }
        [StringLength(255)]
        public string weixin_menu_media_id { get; set; }
        /// <summary>
        /// 对应小程序的appid
        /// </summary>
        [StringLength(255)]
        public string weixin_applet_appid { get; set; }
        /// <summary>
        /// 小程序页面
        /// </summary>
        [StringLength(255)]
        public string weixin_applet_pagepath { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int topsize { get; set; }

        public DateTime? create_time { get; set; }

        public DateTime? update_time { get; set; }
    }
}
