using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class EmployeeController : BaseController
    {
        DBWorker worker = new DBWorker();

        #region Employee List
        public ActionResult List()
        {
            return View(GetEmployeeList());
        }
        public List<EmployeeViewModel> GetEmployeeList()
        {
            List<EmployeeViewModel> EmpList = new List<EmployeeViewModel>();
            var list = worker.EmployeeEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    EmpList.Add(new EmployeeViewModel
                    {
                        EmployeeId = item.EmployeeId,
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
        #endregion

        #region Create Employee
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(EmployeeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Employee emp = new Employee();
                    emp.FirstName = model.FirstName;
                    emp.LastName = model.LastName;
                    emp.Email = model.Email;
                    emp.Gender = model.Gender;
                    emp.DOB = model.DOB;
                    emp.BloodGroup = model.BloodGroup;
                    emp.PermanentAddress = model.PermanentAddress;
                    emp.PostalCode = model.PostalCode;
                    emp.PhoneNo = model.PhoneNo;
                    emp.AlternateNumber = model.AlternateNumber;
                    emp.AadharNumber = model.AadharNumber;
                    emp.DateofJoining = model.DateofJoining;
                    emp.AccountNo = model.AccountNo;
                    emp.Salary = model.Salary;
                    emp.Remarks = model.Remarks;
                    emp.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    emp.UpdatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    emp.CreatedDate = DateTime.Now.Date;
                    worker.EmployeeEntity.Insert(emp);
                    worker.Save();
                }
                return RedirectToAction("List");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Edit Employee
        public ActionResult Edit(int Id)
        {
            try
            {
                EmployeeViewModel model = new EmployeeViewModel();
                var user = worker.EmployeeEntity.GetByID(Id);
                model.EmployeeId = user.EmployeeId;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Email = user.Email;
                model.Gender = user.Gender;
                model.DOB = user.DOB;
                model.BloodGroup = user.BloodGroup;
                model.PermanentAddress = user.PermanentAddress;
                model.PostalCode = user.PostalCode;
                model.PhoneNo = user.PhoneNo;
                model.AlternateNumber = user.AlternateNumber;
                model.AadharNumber = user.AadharNumber;
                model.DateofJoining = user.DateofJoining;
                model.AccountNo = user.AccountNo;
                model.Salary = user.Salary;
                model.Remarks = user.Remarks;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(EmployeeViewModel model)
        {
            UpdateEmployee(model);
            return RedirectToAction("List");
        }

        public bool UpdateEmployee(EmployeeViewModel model)
        {
            try
            {
                Employee user = worker.EmployeeEntity.GetByID(model.EmployeeId);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Gender = model.Gender;
                user.DOB = model.DOB;
                user.BloodGroup = model.BloodGroup;
                user.PermanentAddress = model.PermanentAddress;
                user.PostalCode = model.PostalCode;
                user.PhoneNo = model.PhoneNo;
                user.AlternateNumber = model.AlternateNumber;
                user.AadharNumber = model.AadharNumber;
                user.DateofJoining = model.DateofJoining;
                user.AccountNo = model.AccountNo;
                user.Salary = model.Salary;
                user.Remarks = model.Remarks;
                user.UpdatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                user.UpdatedDate = DateTime.Now.Date;
                worker.EmployeeEntity.Update(user);
                worker.Save();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Delete Employee
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = DeleteEmployee(id);
            return RedirectToAction("List"); ;
        }

        public bool DeleteEmployee(int id)
        {

            var result = worker.EmployeeEntity.GetByID(id);
            if (result != null)
            {
                worker.EmployeeEntity.Delete(result);
                worker.Save();
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}