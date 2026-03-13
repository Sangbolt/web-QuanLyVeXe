using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VEXE.Common;
using VEXE.Models;

namespace VEXE.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        private Models.VEXE db = new Models.VEXE();

        // Danh sách bài viết
        public ActionResult Index()
        {
            var list = db.posts.Where(m => m.status > 0).OrderByDescending(m => m.ID).ToList();
            return View(list);
        }

        // Chi tiết bài viết
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post mpost = db.posts.Find(id);
            if (mpost == null)
            {
                return HttpNotFound();
            }
            return View(mpost);
        }

        // Tạo bài viết mới (GET)
        public ActionResult Create()
        {
            ViewBag.listTopic = db.topics.Where(m => m.status == 1).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(post mpost)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file;
                var namecateDb = db.topics.Where(m => m.ID == mpost.topid).First();
                string slug = Mystring.ToSlug(mpost.title.ToString());
                string namecate = Mystring.ToStringNospace(namecateDb.name);
                file = Request.Files["img"];
                string filename = file.FileName.ToString();
                string ExtensionFile = Mystring.GetFileExtension(filename);
                string namefilenew = namecate + "/" + slug + "." + ExtensionFile;
                var path = Path.Combine(Server.MapPath("~/public/images/post/"), namefilenew);
                var folder = Server.MapPath("~/public/images/post/" + namecate);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                file.SaveAs(path);
                mpost.img = namefilenew;
                mpost.slug = slug;
                mpost.type = "Post";
                mpost.created_at = DateTime.Now;
                mpost.updated_at = DateTime.Now;
                mpost.created_by = int.Parse(Session["Admin_id"].ToString());
                mpost.updated_by = int.Parse(Session["Admin_id"].ToString());
                db.posts.Add(mpost);
                db.SaveChanges();
                //create Link

                db.SaveChanges();
                Message.set_flash("Thêm bài viết thành công", "Thành công");
                return RedirectToAction("Index");
            }
            ViewBag.listTopic = db.topics.Where(m => m.status != 0).ToList();
            Message.set_flash("Thêm bài viết thất bại", "Thất bại");
            return View(mpost);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post mpost = db.posts.Find(id);
            if (mpost == null)
            {
                return HttpNotFound();
            }
            ViewBag.listTopic = db.topics.Where(m => m.status != 0).ToList();
            return View(mpost);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(post mpost)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file;
                string slug = Mystring.ToSlug(mpost.title.ToString());
                file = Request.Files["img"];
                string filename = file.FileName.ToString();
                if (filename.Equals("") == false)
                {
                    var namecateDb = db.topics.Where(m => m.ID == mpost.topid).First();
                    string namecate = Mystring.ToStringNospace(namecateDb.name);
                    string ExtensionFile = Mystring.GetFileExtension(filename);
                    string namefilenew = namecate + "/" + slug + "." + ExtensionFile;
                    var path = Path.Combine(Server.MapPath("~/public/images/post"), namefilenew);
                    var folder = Server.MapPath("~/public/images/post/" + namecate);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    file.SaveAs(path);
                    mpost.img = namefilenew;
                }
                mpost.slug = slug;
                mpost.updated_at = DateTime.Now;
                mpost.updated_by = int.Parse(Session["Admin_id"].ToString());
                db.Entry(mpost).State = EntityState.Modified;
                db.SaveChanges();
                Message.set_flash("Cập nhật bài viết thành công", "Thành công");

                return RedirectToAction("Index");
            }
            ViewBag.listTopic = db.topics.Where(m => m.status != 0).ToList();
            Message.set_flash("Cập nhật bài viết thất bại", "Thất bại");
            return View(mpost);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post mpost = db.posts.Find(id);
            if (mpost == null)
            {
                return HttpNotFound();
            }
            return View(mpost);
        }
        public ActionResult Status(int id)
        {
            post mpost = db.posts.Find(id);
            mpost.status = (mpost.status == 1) ? 2 : 1;
            mpost.updated_at = DateTime.Now;
            mpost.updated_by = int.Parse(Session["Admin_id"].ToString());
            db.Entry(mpost).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Thay đổi trạng thái thành công", "Thành công");
            return RedirectToAction("Index");
        }
        public ActionResult trash()
        {
            var list = db.posts.Where(m => m.status == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult Deltrash(int id)
        {
            post mpost = db.posts.Find(id);
            mpost.status = 0;
            mpost.updated_at = DateTime.Now;
            mpost.updated_by = int.Parse(Session["Admin_id"].ToString());
            db.Entry(mpost).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Xóa bài viết thành công", "Thành công");
            return RedirectToAction("Index");
        }
        public ActionResult Retrash(int id)
        {
            post mpost = db.posts.Find(id);
            mpost.status = 2;
            mpost.updated_at = DateTime.Now;
            mpost.updated_by = int.Parse(Session["Admin_id"].ToString());
            db.Entry(mpost).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Phục hồi bài viết thành công", "Thành công");
            return RedirectToAction("trash");
        }
        public ActionResult deleteTrash(int id)
        {
            post mpost = db.posts.Find(id);
            db.posts.Remove(mpost);
            db.SaveChanges();
            Message.set_flash("Xóa vĩnh viễn bài viết thành công", "Thành công");
            return RedirectToAction("trash");
        }
    }
}
