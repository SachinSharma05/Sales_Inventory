using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class InHouseController : BaseController
    {
        #region Variable
        DBWorker worker = new DBWorker();
        #endregion

        #region InHouse Transaction List
        public ActionResult List()
        {
            InHouseViewModel model = new InHouseViewModel();
            model.List = GetInHouseList();
            return View(model);
        }
        public List<InHouseViewModel> GetInHouseList()
        {
            List<InHouseViewModel> InHouseList = new List<InHouseViewModel>();
            var list = worker.InHouseTransactionEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    InHouseList.Add(new InHouseViewModel
                    {
                        Id = item.Id,
                        PaidBy = item.PaidBy,
                        PaidTo = item.PaidTo,
                        PaidDate = item.PaidDate,
                        TotalAmount = item.TotalAmount,
                        PaidAmount = item.PaidAmount,
                        ReturnAmtReceived = item.ReturnAmtReceived,
                        BalanceAmt = item.BalanceAmt
                    });
                }
            }
            return InHouseList;
        }
        #endregion

        #region Create InHouse Transaction
        [HttpPost]
        public ActionResult Create(string paidTo, string paidBy, string paidDate, string totalAmount, string paidAmount, string returnAmtReceived, string balanceAmt)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    InHouse transaction = new InHouse();
                    transaction.PaidBy = paidBy;
                    transaction.PaidTo = paidTo;
                    transaction.PaidDate = Convert.ToDateTime(paidDate);
                    transaction.TotalAmount = Convert.ToInt32(totalAmount);
                    transaction.PaidAmount = Convert.ToInt32(paidAmount);
                    transaction.ReturnAmtReceived = Convert.ToInt32(returnAmtReceived);
                    transaction.BalanceAmt = Convert.ToInt32(balanceAmt);
                    transaction.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    transaction.CreatedDate = DateTime.Now.Date;
                    worker.InHouseTransactionEntity.Insert(transaction);
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

        #region Edit InHouse Transaction
        public JsonResult Edit(int Id)
        {
            try
            {
                InHouseViewModel model = new InHouseViewModel();
                var list = worker.InHouseTransactionEntity.GetByID(Id);
                model.Id = list.Id;
                model.PaidBy = list.PaidBy;
                model.PaidTo = list.PaidTo;
                model.PaidDate = list.PaidDate;
                model.TotalAmount = list.TotalAmount;
                model.PaidAmount = list.PaidAmount;
                model.ReturnAmtReceived = list.ReturnAmtReceived;
                model.BalanceAmt = list.BalanceAmt;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Update(InHouseViewModel model)
        {
            try
            {
                InHouse data = worker.InHouseTransactionEntity.GetByID(model.Id);
                data.ReturnAmtReceived = model.ReturnAmtReceived;
                data.BalanceAmt = model.BalanceAmt;
                data.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                data.CreatedDate = DateTime.Now.Date;
                worker.InHouseTransactionEntity.Update(data);
                worker.Save();

                return RedirectToAction("list");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete InHouse Transaction
        [HttpGet]
        public ActionResult Delete(int id)
        {
            DeleteInHouseTransaction(id);
            return RedirectToAction("List");
        }

        public bool DeleteInHouseTransaction(int id)
        {
            var trans = worker.InHouseTransactionEntity.GetByID(id);
            if (trans != null)
            {
                worker.InHouseTransactionEntity.Delete(trans);
                worker.Save();
                return true;
            }
            else
                return false;
        }
        #endregion

        #region Search List
        public ActionResult SearchList(string StartDate, string EndDate)
        {
            try
            {
                List<InHouseViewModel> model = new List<InHouseViewModel>();
                var SDate = StartDate != "" ? Convert.ToDateTime(StartDate).Date : DateTime.Now;
                var EDate = EndDate != "" ? Convert.ToDateTime(EndDate).Date : DateTime.Now;

                if (StartDate != "" && EndDate != "")
                {
                    var list = worker.InHouseTransactionEntity.Get(x => x.PaidDate >= SDate && x.PaidDate <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new InHouseViewModel
                        {
                            Id = item.Id,
                            PaidBy = item.PaidBy,
                            PaidTo = item.PaidTo,
                            PaidDate = item.PaidDate,
                            TotalAmount = item.TotalAmount,
                            PaidAmount = item.PaidAmount,
                            ReturnAmtReceived = item.ReturnAmtReceived,
                            BalanceAmt = item.BalanceAmt
                        });
                    }
                }
                else if (StartDate != "" && EndDate == "")
                {
                    var list = worker.InHouseTransactionEntity.Get(x => x.PaidDate == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new InHouseViewModel
                        {
                            Id = item.Id,
                            PaidBy = item.PaidBy,
                            PaidTo = item.PaidTo,
                            PaidDate = item.PaidDate,
                            TotalAmount = item.TotalAmount,
                            PaidAmount = item.PaidAmount,
                            ReturnAmtReceived = item.ReturnAmtReceived,
                            BalanceAmt = item.BalanceAmt
                        });
                    }
                }
                else if (EndDate != "" && StartDate == "")
                {
                    var list = worker.InHouseTransactionEntity.Get(x => x.PaidDate == EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new InHouseViewModel
                        {
                            Id = item.Id,
                            PaidBy = item.PaidBy,
                            PaidTo = item.PaidTo,
                            PaidDate = item.PaidDate,
                            TotalAmount = item.TotalAmount,
                            PaidAmount = item.PaidAmount,
                            ReturnAmtReceived = item.ReturnAmtReceived,
                            BalanceAmt = item.BalanceAmt
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