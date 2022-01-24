using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class EmployeeController : Controller
    {
        DBWorker worker = new DBWorker();

        // GET: Employee

        public ActionResult List()
        {
            return View(GetEmployeeList());
        }

        public List<EmployeeViewModel> GetEmployeeList()
        {
            List<EmployeeViewModel> EmpList = new List<EmployeeViewModel>();
            var list = worker.EmployeeEntity.Get().ToList();
            if(list.Count > 0)
            {
                foreach (var item in list)
                {
                    EmpList.Add(new EmployeeViewModel
                    {
                        FullName = item.FullName,
                        AadharNumber = item.AadharNumber,
                        Gender = item.Gender,
                        DateofJoining = item.DateofJoining,
                        PhoneNo = item.PhoneNo,
                        PermanentAddress = item.PermanentAddress
                    });
                }
            }
            return EmpList;
        }
    }
}