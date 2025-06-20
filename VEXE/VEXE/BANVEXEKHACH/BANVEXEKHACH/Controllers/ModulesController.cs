﻿using VEXE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VEXE.Controllers
{
    public class ModulesController : Controller
    {
        Models.VEXE db = new Models.VEXE();
        public ActionResult _Header()
        {
            if ((string)Session["userName11"] != "")
            {
                ViewBag.sessionFullname = Session["userName11"];
            }
            return View("_Header");
        }
        public ActionResult _Mainmenu()
        {

            var list = db.menus.Where(m => m.status == 1 && m.parentid == 0).ToList();
            return View("_Mainmenu", list);
        }
        public ActionResult _Footer()
        {
            return View("_Footer");
        }

        public ActionResult Slider()
        {
            return View("Slider");
        }
        public ActionResult LogoSlide()
        {
            return View("LogoSlide");
        }
    }
}