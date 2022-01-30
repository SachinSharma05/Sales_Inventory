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
            return View(GetAdvanceList());
        }
        public List<AdvanceViewModel> GetAdvanceList()
        {
            List<AdvanceViewModel> AdvanceList = new List<AdvanceViewModel>();
            var list = worker.AdvanceEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    AdvanceList.Add(new AdvanceViewModel
                    {
                        Id = item.Id,
                        Advance_To = item.Advance_To,
                        Advance_Date = item.Advance_Date,
                        Advance_Amount = item.Advance_Amount,
                        Advance_Against = item.Advance_Against
                    });
                }
            }
            return AdvanceList;
        }
        #endregion

        #region Create Advance
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AdvanceViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Advance advance = new Advance();
                    advance.Advance_To = model.Advance_To;
                    advance.Advance_Date = model.Advance_Date;
                    advance.Advance_Amount = model.Advance_Amount;
                    advance.Advance_Against = model.Advance_Against;
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