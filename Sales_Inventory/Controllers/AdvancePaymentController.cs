using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class AdvancePaymentController : BaseController
    {
        DBWorker worker = new DBWorker();

        // GET: AdvancePayment

        #region Advance List
        public ActionResult List()
        {
            AdvanceViewModel model = new AdvanceViewModel();
            model.AdvanceTo = GetAdvanceToList();
            model.List = GetAdvanceList();
            return View(model);
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

        public List<SelectListItem> GetAdvanceToList()
        {
            var query = worker.AdvanceEntity.Get().ToList();

            var list = new List<SelectListItem> { new SelectListItem { Value = null, Text = "" } };
            list.AddRange(query.ToList().Select(C => new SelectListItem
            {
                Value = C.Id.ToString(),
                Text = C.Advance_To
            }).Distinct());

            ViewBag.ProductList = list;

            return list;
        }
        #endregion

        #region Create Advance
        public ActionResult Create()
        {
            return View("_Create");
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

        #region Search List
        public ActionResult SearchList(string PurchaseName, string StartDate, string EndDate)
        {
            try
            {
                List<AdvanceViewModel> model = new List<AdvanceViewModel>();
                var SDate = StartDate != "" ? Convert.ToDateTime(StartDate).Date : DateTime.Now;
                var EDate = EndDate != "" ? Convert.ToDateTime(EndDate).Date : DateTime.Now;

                if (PurchaseName != "" && StartDate != "" && EndDate != "")
                {
                    var list = worker.AdvanceEntity.Get(x => x.Advance_To == PurchaseName && x.Advance_Date >= SDate && x.Advance_Date <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new AdvanceViewModel
                        {
                            Id = item.Id,
                            Advance_To = item.Advance_To,
                            Advance_Date = item.Advance_Date,
                            Advance_Amount = item.Advance_Amount,
                            Advance_Against = item.Advance_Against
                        });
                    }
                }
                else if (PurchaseName != "" && StartDate != "" && EndDate == "")
                {
                    var list = worker.AdvanceEntity.Get(x => x.Advance_To == PurchaseName && x.Advance_Date == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new AdvanceViewModel
                        {
                            Id = item.Id,
                            Advance_To = item.Advance_To,
                            Advance_Date = item.Advance_Date,
                            Advance_Amount = item.Advance_Amount,
                            Advance_Against = item.Advance_Against
                        });
                    }
                }
                else if (PurchaseName != "" && EndDate != "" && StartDate == "")
                {
                    var list = worker.AdvanceEntity.Get(x => x.Advance_To == PurchaseName && x.Advance_Date == EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new AdvanceViewModel
                        {
                            Id = item.Id,
                            Advance_To = item.Advance_To,
                            Advance_Date = item.Advance_Date,
                            Advance_Amount = item.Advance_Amount,
                            Advance_Against = item.Advance_Against
                        });
                    }
                }
                else if (StartDate != "" && EndDate != "" && PurchaseName == "")
                {
                    var list = worker.AdvanceEntity.Get(x => x.Advance_Date >= SDate && x.Advance_Date <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new AdvanceViewModel
                        {
                            Id = item.Id,
                            Advance_To = item.Advance_To,
                            Advance_Date = item.Advance_Date,
                            Advance_Amount = item.Advance_Amount,
                            Advance_Against = item.Advance_Against
                        });
                    }
                }
                else if (PurchaseName != null && StartDate == "" && EndDate == "")
                {
                    var list = worker.AdvanceEntity.Get(x => x.Advance_To == PurchaseName).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new AdvanceViewModel
                        {
                            Id = item.Id,
                            Advance_To = item.Advance_To,
                            Advance_Date = item.Advance_Date,
                            Advance_Amount = item.Advance_Amount,
                            Advance_Against = item.Advance_Against
                        });
                    }
                }
                else if (StartDate != "" && PurchaseName == "" && EndDate == "")
                {
                    var list = worker.AdvanceEntity.Get(x => x.Advance_Date == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new AdvanceViewModel
                        {
                            Id = item.Id,
                            Advance_To = item.Advance_To,
                            Advance_Date = item.Advance_Date,
                            Advance_Amount = item.Advance_Amount,
                            Advance_Against = item.Advance_Against
                        });
                    }
                }
                else if (EndDate != "" && StartDate == "" && PurchaseName == "")
                {
                    var list = worker.AdvanceEntity.Get(x => x.Advance_Date <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new AdvanceViewModel
                        {
                            Id = item.Id,
                            Advance_To = item.Advance_To,
                            Advance_Date = item.Advance_Date,
                            Advance_Amount = item.Advance_Amount,
                            Advance_Against = item.Advance_Against
                        });
                    }
                }

                return PartialView("_SearchList", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}