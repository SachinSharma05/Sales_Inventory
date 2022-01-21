using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Sales_Inventory.DAL;
using Sales_Inventory.Models;

namespace Sales_Inventory.Controllers
{
    public class AccountController : Controller
    {
        #region Variable
        DBWorker worker = new DBWorker();
        #endregion

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                UserViewModel user = VerifyLogin(model.Email, model.Password);
                if(user != null)
                {
                    if(user.IsActive)
                    {
                        Session["UserId"] = user.UserId;
                        Session["Email"] = user.Email;
                        Session["Fullname"] = user.Fullname;
                        Session["RoleId"] = user.RoleId;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your account is inactive. Please contact to administrator.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User Name or Password is Invalid.");
                }
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public UserViewModel VerifyLogin(string Email, string Password)
        {
            UserViewModel model = new UserViewModel();
            if(Email != "" && Password != "")
            {
                var user = worker.UserEntity.Get(x => x.Email == Email && x.Password == Password).FirstOrDefault();
                model.Email = user.Email;
                model.Fullname = user.FullName;
                model.UserId = user.UserId;
                model.RoleId = user.RoleId;
                model.IsActive = user.IsActive;
            }
            return model;
        }
    }
}