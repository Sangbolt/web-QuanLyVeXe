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
    public class AirportController : BaseController
    {
        private Models.VEXE db = new Models.VEXE();
        public ActionResult Index()
        {
            var cities = db.cities.Where(m => m.status == 1).ToList();
            ViewBag.cities = cities;
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ticket ticket = db.tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }
        public ActionResult CreateCities()
        {
            var countries = db.countries.Where(m => m.status == 1).ToList();
            ViewBag.countries = countries;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCities(city cities)
        {

            if (ModelState.IsValid)
            {
                db.cities.Add(cities);
                Message.set_flash("Thêm một tuyến mới thành công", "Thành công");
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Message.set_flash("Thêm một tuyến mới không thành công", "Thất bại");
            return View("Create");
        }
        public ActionResult Status(int id)
        {
            city cities = db.cities.Find(id);
            cities.status = (cities.status == 1) ? 2 : 1;
            db.Entry(cities).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Thay đổi thành công", "Thành công");
            return RedirectToAction("Index");
        }
        public ActionResult trash()
        {
            var list = db.cities.Where(m => m.status == 2).ToList();
            return View("Trash", list);
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult Deltrash(int id)
        {
            city morder = db.cities.Find(id);
            morder.status = 2;
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Xóa thành công", "Thành công");
            return RedirectToAction("Index");
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult Retrash(int id)
        {
            city morder = db.cities.Find(id);
            morder.status = 1;
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Phục hồi thành công", "Thành công");
            return RedirectToAction("trash");
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult deleteTrash(int id)
        {
            city morder = db.cities.Find(id);
            db.cities.Remove(morder);
            db.SaveChanges();
            Message.set_flash("Xóa vĩnh viễn thành công", "Thành công");
            return RedirectToAction("trash");
        }
    }
}
