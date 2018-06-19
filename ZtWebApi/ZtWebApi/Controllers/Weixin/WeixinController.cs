using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DbModels.EFMssqlCodeFirst;
using DbModels.WeixinModels;
using ZtWebApi.Common.Weixin;
using ZtWebApi.Common;
using Utility.Log;
using System.Collections;
using System.Web;

namespace ZtWebApi.Controllers.Weixin
{
    /// <summary>
    /// 微信
    /// </summary>
    public class WeixinController : ApiController
    {
        private EFCodeFirst db = new EFCodeFirst();

        /// <summary>
        /// 获取微信列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<weixin_list> Getweixin_list()
        {
            return db.weixin_list;
        }

        // GET: api/Weixin/5
        [ResponseType(typeof(weixin_list))]
        public IHttpActionResult Getweixin_list(int id)
        {
            weixin_list weixin_list = db.weixin_list.Find(id);
            if (weixin_list == null)
            {
                return NotFound();
            }

            return Ok(weixin_list);
        }

        // PUT: api/Weixin/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putweixin_list(int id, weixin_list weixin_list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weixin_list.weixin_id)
            {
                return BadRequest();
            }

            db.Entry(weixin_list).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!weixin_listExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Weixin
        [ResponseType(typeof(weixin_list))]
        public IHttpActionResult Postweixin_list(weixin_list weixin_list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.weixin_list.Add(weixin_list);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = weixin_list.weixin_id }, weixin_list);
        }

        // DELETE: api/Weixin/5
        [ResponseType(typeof(weixin_list))]
        public IHttpActionResult Deleteweixin_list(int id)
        {
            weixin_list weixin_list = db.weixin_list.Find(id);
            if (weixin_list == null)
            {
                return NotFound();
            }

            db.weixin_list.Remove(weixin_list);
            db.SaveChanges();

            return Ok(weixin_list);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool weixin_listExists(int id)
        {
            return db.weixin_list.Count(e => e.weixin_id == id) > 0;
        }

    }
}