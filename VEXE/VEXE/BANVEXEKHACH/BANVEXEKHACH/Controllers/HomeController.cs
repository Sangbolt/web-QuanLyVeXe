﻿using VEXE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VEXE.Controllers
{
    public class HomeController : Controller
    {
        Models.VEXE db = new Models.VEXE();
        public ActionResult Index()
        {
            DateTime date_now = DateTime.Now;
            string date_now1 = date_now.ToString("MM-dd-yyyy");
            DateTime date_now2 = DateTime.Parse(date_now1);
            ViewBag.dateNow = date_now;
            var cities = db.cities.ToList();
            ViewBag.cities = cities;
            var tickets = db.tickets.Where(m => m.status == 1 && m.departure_date == date_now2).ToList();
            ViewBag.tickets = tickets;
            return View();
        }
    
        public ActionResult flightOfday()
        {
            DateTime date_now = DateTime.Now;
            string date_now1 = date_now.ToString("MM-dd-yyyy");
            DateTime date_now2 = DateTime.Parse(date_now1);
            ViewBag.dateNow = date_now;
            var list = db.tickets.Where(m => m.status == 1 && m.departure_date == date_now2).Take(20).ToList();
            return View("flightOfDay", list);
        }
        
    }
}