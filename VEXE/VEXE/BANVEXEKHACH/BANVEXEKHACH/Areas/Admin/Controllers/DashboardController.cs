using VEXE.Common;
using VEXE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace VEXE.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private Models.VEXE db = new Models.VEXE();

        public DashboardController()
        {
        }

        public ActionResult Index()
        {
            // Tổng doanh thu
            var revenue = db.ordersdetails
                .Include(x => x.ticket)
                .Include(x => x.order)
                .Sum(x => x.quantity * x.ticket.price);
            ViewBag.Revenue = string.Format("{0:0,0}", revenue ?? 0);

            // Tổng số tuyến xe
            var totalRoutes = db.tickets.Count();
            ViewBag.TotalRoutes = string.Format("{0:0,0}", totalRoutes);

            // Tổng số lượng vé đặt
            var totalOrders = db.orders.Count();
            ViewBag.TotalOrders = string.Format("{0:0,0}", totalOrders);


            return View();
        }


        public ActionResult usersession()
        {
            var session = (Userlogin)Session[Common.CommonConstants.USER_SESSION];
            return View("_adminSession", session);
        }
    }
}
