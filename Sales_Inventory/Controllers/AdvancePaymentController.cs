using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class AdvancePaymentController : Controller
    {
        DBWorker worker = new DBWorker();

        // GET: AdvancePayment

        #region Advance List
        public ActionResult List()
        {
            var AdvanceList = worker.AdvanceEntity.Get().ToList();
            ViewBag.ListAdvance = AdvanceList;
            return View();
        }
        #endregion

        #region Create Advance
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string AdvanceTo, string AdvanceAmt, string AdvanceDate, string AdvanceAgainst)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Advance advance = new Advance();
                    advance.Advance_To = AdvanceTo;
                    advance.Advance_Date = Convert.ToDateTime(AdvanceDate);
                    advance.Advance_Amount = AdvanceAmt;
                    advance.Advance_Against = AdvanceAgainst;
                    advance.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    advance.CreatedDate = DateTime.Now.Date;
                    worker.AdvanceEntity.Insert(advance);
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

        #region Edit Advance
        public ActionResult Edit(int Id)
        {
            try
            {
                AdvanceViewModel model = new AdvanceViewModel();
                var user = worker.AdvanceEntity.GetByID(Id);
                model.Advance_To = user.Advance_To;
                model.Advance_Date = user.Advance_Date;
                model.Advance_Amount = user.Advance_Amount;
                model.Advance_Against = user.Advance_Against;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(AdvanceViewModel model)
        {
            if (ModelState.IsValid)
            {
                UpdateAdvance(model);
            }
            return RedirectToAction("List");
        }

        public bool UpdateAdvance(AdvanceViewModel model)
        {
            try
            {
                Advance adv = worker.AdvanceEntity.GetByID(model.Id);
                adv.Advance_To = model.Advance_To;
                adv.Advance_Date = model.Advance_Date;
                adv.Advance_Amount = model.Advance_Amount;
                adv.Advance_Against = model.Advance_Against;
                worker.AdvanceEntity.Update(adv);
                worker.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Delete Advance
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = DeleteAdvance(id);
            return RedirectToAction("List"); ;
        }

        public bool DeleteAdvance(int id)
        {

            var result = worker.AdvanceEntity.GetByID(id);
            if (result != null)
            {
                worker.AdvanceEntity.Delete(result);
                worker.Save();
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}