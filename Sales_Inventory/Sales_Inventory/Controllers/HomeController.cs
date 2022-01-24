using Sales_Inventory.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class HomeController : Controller
    {
        DBWorker worker = new DBWorker();

        public ActionResult Index()
        {
            var EmployeeCount = worker.EmployeeEntity.Get().ToList().Count;
            TempData["EmployeeCount"] = EmployeeCount;
            return View();
        }
    }
}