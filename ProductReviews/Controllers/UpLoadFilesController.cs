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
using ProductReviews.Models;
using ProductReviews.JGZX.Entities.Common;
using System.Web;
using System.IO;

namespace ProductReviews.Controllers
{
    public class UpLoadFilesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [AllowAnonymous]
        [Route("api/uploadfiles"),HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
            List<string> filelist = new List<string>();
            try
            {
                //var sysbean = db.Database.SqlQuery<SysConfigBean>("select * from SysConfigBean").FirstOrDefault(p => p.systype == "act_admin_path");
                var sysbean = db.SysConfigBean.Where(p => p.systype == "act_admin_path").FirstOrDefault();
                int cnt = HttpContext.Current.Request.Files.Count;

                for (int i = 0; i < cnt; i++)
                {
                    UpLoadHsty hsty = new UpLoadHsty();
                    HttpPostedFile file = HttpContext.Current.Request.Files[i];

                    Stream fs = file.InputStream;
                    byte[] myfile = new byte[fs.Length];
                    fs.Read(myfile, 0, (int)fs.Length);
                    //System.Guid guid = System.Guid.NewGuid();//Guid类型
                    string strGUID = System.Guid.NewGuid().ToString();//直接返回字符串类型
                    string[] extfiletype = file.FileName.Split('.');
                    string strfName = strGUID + '.' + extfiletype[extfiletype.Length - 1];
                    string savename = strfName;
                    hsty.new_name = savename;
                    hsty.old_name = file.FileName;
                    hsty.create_date = DateTime.Now;

                    if (!Directory.Exists(sysbean.filepath))
                    {
                        Directory.CreateDirectory(sysbean.filepath);
                    }
                    string strPath = sysbean.filepath + strfName;
                    file.SaveAs(strPath);
                    filelist.Add(strfName);

                    response = request.CreateResponse(HttpStatusCode.OK, filelist);

                }
            }
            catch (Exception ex)
            {
                return request.CreateResponse(HttpStatusCode.NotImplemented, ex.Message);
            }
            return response;
        }



        // GET: api/UpLoadFiles
        //public IQueryable<UpLoadFiles> GetUpLoadFiles()
        //{
        //    return db.UpLoadFiles;
        //}

        // GET: api/UpLoadFiles/5
        //[ResponseType(typeof(UpLoadFiles))]
        //public IHttpActionResult GetUpLoadFiles(int id)
        //{
        //    UpLoadFiles upLoadFiles = db.UpLoadFiles.Find(id);
        //    if (upLoadFiles == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(upLoadFiles);
        //}

        // PUT: api/UpLoadFiles/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutUpLoadFiles(int id, UpLoadFiles upLoadFiles)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != upLoadFiles.file_id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(upLoadFiles).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UpLoadFilesExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/UpLoadFiles
        //[ResponseType(typeof(UpLoadFiles))]
        //public IHttpActionResult PostUpLoadFiles(UpLoadFiles upLoadFiles)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.UpLoadFiles.Add(upLoadFiles);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = upLoadFiles.file_id }, upLoadFiles);
        //}

        // DELETE: api/UpLoadFiles/5
        //[ResponseType(typeof(UpLoadFiles))]
        //public IHttpActionResult DeleteUpLoadFiles(int id)
        //{
        //    UpLoadFiles upLoadFiles = db.UpLoadFiles.Find(id);
        //    if (upLoadFiles == null)
        //    {
        //        return NotFound();
        //    }

        //    db.UpLoadFiles.Remove(upLoadFiles);
        //    db.SaveChanges();

        //    return Ok(upLoadFiles);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool UpLoadFilesExists(int id)
        //{
        //    return db.UpLoadFiles.Count(e => e.file_id == id) > 0;
        //}
    }
}