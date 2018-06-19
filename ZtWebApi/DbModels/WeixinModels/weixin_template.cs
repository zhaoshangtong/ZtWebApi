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
    /// 微信模板
    /// </summary>
    public class weixin_template
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int weixin_template_id { get; set; }
        [ForeignKey("weixin_id")]
        public weixin_list weixin { get; set; }
        public Nullable<int> weixin_id { get; set; }
        ///<summary>
        ///微信模版编号  
        ///</summary>
        public string template_code { get; set; }
        ///<summary>
        ///模板ID  
        ///</summary>
        public string tempid { get; set; }

        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
    }
}
