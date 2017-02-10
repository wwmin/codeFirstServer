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
        [Route("api/uploadfiles"), HttpPost]
        public HttpResponseMessage filesUpload(HttpRequestMessage request)
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


        [AllowAnonymous]
        [Route("api/uploadFilesWithSmallImg"), HttpPost]
        public HttpResponseMessage filesUploadBigSmallImg(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
            List<string> filelist = new List<string>();
            try
            {
                List<UpLoadFilesBigSmall> UpLoadFilesBigSmallList = new List<UpLoadFilesBigSmall>();
                int cnt = System.Web.HttpContext.Current.Request.Files.Count;
                for (int i = 0; i < cnt; i++)
                {
                    UpLoadFilesBigSmall UpLoadFilesBigSmall = new UpLoadFilesBigSmall();
                    HttpPostedFile mFile = System.Web.HttpContext.Current.Request.Files[i];
                    if (mFile == null || mFile.ContentLength == 0) { response = request.CreateResponse(HttpStatusCode.OK, new { }); continue; }
                    #region 获取文件信息
                    Stream fs = mFile.InputStream;
                    byte[] myFile = new byte[fs.Length];
                    fs.Read(myFile, 0, (int)fs.Length);

                    string[] mFileExtNameArray = mFile.FileName.Split('.');
                    string mFileExtName = mFileExtNameArray[mFileExtNameArray.Length-1];
                    string mCurrentFileName = mFile.FileName;//上传时文件名称
                    string mSysFileName = System.Guid.NewGuid().ToString().ToUpper().Replace("-", "");//生成guid全大写去掉  "-"  后的文件名，没有扩展名
                    string mSysFileFullName = mSysFileName + '.' + mFileExtName;//生成系统的文件名称 
                    #endregion
                    #region 生成文件名称以及建立文件夹
                    string mRootSaveFolder = "/upload";
                    //按时间进行分组
                    string mDateFolder = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                    //文件扩展名
                    string mFileExtentName = mFileExtNameArray[mFileExtNameArray.Length - 1];
                    //大图存放的图片路径
                    string mBigFileFolder = "/big/" + mDateFolder + "/";
                    //网站路径地址
                    string url = System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port+mRootSaveFolder;
                    //网站本地路径
                    string savePath = System.Web.HttpContext.Current.Server.MapPath(mRootSaveFolder);
                    //大图片存放的文件夹路径
                    string mSaveBigFileFolder = savePath + mBigFileFolder;
                    //创建文件夹
                    if (!Directory.Exists(mSaveBigFileFolder))
                    {
                        Directory.CreateDirectory(mSaveBigFileFolder);
                    }
                    #endregion
                    #region 保存文件,如果为图片类数据,则生成缩略图

                    List<string> mExtNameArray = new List<string>();//图片类型数组
                    mExtNameArray.Add("jpg");
                    mExtNameArray.Add("png");
                    mExtNameArray.Add("bmp");
                    mExtNameArray.Add("jpeg");
                    mExtNameArray.Add("gif");
                    //是否为图片
                    var mIsPic = mExtNameArray.Where(p => p == mFileExtentName.ToLower()).FirstOrDefault() == null ? false : true;
                    var mFileUrl = mSaveBigFileFolder + mSysFileFullName;
                    if (mIsPic)
                    {
                        var mSmallImage = ImageHelper.GetThumbnail(ImageHelper.BytesToImage(myFile),200,150);
                        //小图片存放的图片路径
                        string mSmallFileFolder = "/samll/" + mDateFolder + "/";
                        //小图片存放的文件夹路径
                        string mSaveSmallFileFolder = savePath + mSmallFileFolder;
                        if (!Directory.Exists(mSaveSmallFileFolder))
                        {
                            Directory.CreateDirectory(mSaveSmallFileFolder);
                        }
                        mSmallImage.Save(mSaveSmallFileFolder + mSysFileFullName);

                        //写入实例
                        UpLoadFilesBigSmall.F_FileSmallURL = mSmallFileFolder + mSysFileFullName;
                    }
                    else
                    {
                        UpLoadFilesBigSmall.F_FileSmallURL = "";
                    }
                    mFile.SaveAs(mFileUrl);
                    #endregion
                    #region 添加数据到数据库
                    UpLoadFilesBigSmall.F_FileCurrentName = mCurrentFileName;
                    UpLoadFilesBigSmall.F_FileExtName = mFileExtentName;
                    UpLoadFilesBigSmall.F_FileSysName = mSysFileName;
                    UpLoadFilesBigSmall.F_FileURL = mBigFileFolder + mSysFileFullName;

                    UpLoadFilesBigSmallList.Add(UpLoadFilesBigSmall);
                    #endregion
                    db.UpLoadFilesBigSmall.Add(UpLoadFilesBigSmall);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return request.CreateErrorResponse(HttpStatusCode.NotImplemented,ex.Message);
                    }
                }
                response = request.CreateResponse(HttpStatusCode.OK, UpLoadFilesBigSmallList.ToList());
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.NotImplemented, ex.Message);
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