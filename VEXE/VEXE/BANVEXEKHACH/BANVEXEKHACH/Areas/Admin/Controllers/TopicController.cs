using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VEXE.Common;
using VEXE.Models;

namespace VEXE.Areas.Admin.Controllers
{
    public class TopicController : BaseController
    {
        private Models.VEXE db = new Models.VEXE();

        // GET: Admin/Topic
        public ActionResult Index()
        {

            var list = db.topics.Where(m => m.status != 0).OrderByDescending(m => m.ID).ToList();
            return View(list);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            topic mtopic = db.topics.Find(id);
            if (mtopic == null)
            {
                return HttpNotFound();
            }
            return View(mtopic);
        }

        public ActionResult Create()
        {
            ViewBag.listtopic = db.topics.Where(m => m.status != 0).ToList();
            return View();
        }

        // POST: Admin/Topic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(topic mtopic)
        {
            if (ModelState.IsValid)
            {
                string slug = Mystring.ToSlug(mtopic.name.ToString());
                mtopic.slug = slug;
                mtopic.created_at = DateTime.Now;
                mtopic.updated_at = DateTime.Now;
                mtopic.created_by = int.Parse(Session["Admin_id"].ToString());
                mtopic.updated_by = int.Parse(Session["Admin_id"].ToString());
                db.topics.Add(mtopic);
                db.SaveChanges();
                Message.set_flash("Thành công", "Thành công");
                return RedirectToAction("Index");
            }
            Message.set_flash("Thất bại", "Thất bại");
            ViewBag.listtopic = db.topics.Where(m => m.status != 0).ToList();
            return View(mtopic);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            topic mtopic = db.topics.Find(id);
            if (mtopic == null)
            {
                return HttpNotFound();
            }
            ViewBag.listtopic = db.topics.Where(m => m.status != 0).ToList();
            return View(mtopic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(topic mtopic)
        {
            if (ModelState.IsValid)
            {
                string slug = Mystring.ToSlug(mtopic.name.ToString());


                mtopic.updated_at = DateTime.Now;
                mtopic.updated_by = int.Parse(Session["Admin_id"].ToString());
                db.Entry(mtopic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listtopic = db.topics.Where(m => m.status != 0).ToList();
            return View(mtopic);
        }

        public ActionResult Status(int id)
        {
            topic mtopic = db.topics.Find(id);
            mtopic.status = (mtopic.status == 1) ? 2 : 1;
            mtopic.updated_at = DateTime.Now;
            mtopic.updated_by = int.Parse(Session["Admin_id"].ToString());
            db.Entry(mtopic).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Thay đổi trạng thái thành công", "Thành công");
            return RedirectToAction("Index");
        }
        //trash
        public ActionResult trash()
        {
            var list = db.topics.Where(m => m.status == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult Deltrash(int id)
        {
            topic mtopic = db.topics.Find(id);
            mtopic.status = 0;
            mtopic.updated_at = DateTime.Now;
            mtopic.updated_by = int.Parse(Session["Admin_id"].ToString());
            db.Entry(mtopic).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Xóa chủ đề thành công", "Thành công");
            return RedirectToAction("Index");
        }

        public ActionResult Retrash(int id)
        {
            topic mtopic = db.topics.Find(id);
            mtopic.status = 2;
            mtopic.updated_at = DateTime.Now;
            mtopic.updated_by = int.Parse(Session["Admin_id"].ToString());
            db.Entry(mtopic).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Phục hồi thành công", "Thành công");
            return RedirectToAction("trash");
        }
        public ActionResult deleteTrash(int id)
        {
            topic mtopic = db.topics.Find(id);
            db.topics.Remove(mtopic);
            db.SaveChanges();
            Message.set_flash("Xóa vĩnh viễn chủ đề thành công", "Thành công");
            return RedirectToAction("trash");
        }
    }
}
