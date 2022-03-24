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
            model.PaidByList = GetPaidBy();
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
                    if(item.PaidDate < DateTime.Now.Date)
                    {
                        InHouse inHouse = worker.InHouseTransactionEntity.GetByID(item.Id);
                        inHouse.CarryForward = item.Balance;
                        worker.InHouseTransactionEntity.Update(inHouse);
                        worker.Save();
                    }

                    InHouseList.Add(new InHouseViewModel
                    {
                        Id = item.Id,
                        TransactionNo = item.TransactionNo,
                        PaidFor = item.PaidFor,
                        PaidBy = item.PaidBy,
                        PaidDate = item.PaidDate,
                        PaidAmt = item.PaidAmt,
                        Balance = item.Balance,
                        CarryForward = item.CarryForward,
                        TransactionType = item.TransactionType
                    });
                }
            }
            return InHouseList;
        }
        public List<SelectListItem> GetPaidBy()
        {
            var query = worker.InHouseTransactionEntity.Get().ToList();

            var list = new List<SelectListItem> { new SelectListItem { Value = null, Text = "" } };
            list.AddRange(query.ToList().Select(C => new SelectListItem
            {
                Value = C.Id.ToString(),
                Text = C.PaidBy
            }));

            return list;
        }
        #endregion

        #region Create InHouse Transaction
        [HttpPost]
        public ActionResult Create(string paidFor, string paidBy, string paidDate, string totalAmount)
        {
            string TransactionNo = "";
            try
            {
                if(ModelState.IsValid)
                {
                    InHouse transaction = new InHouse();
                    transaction.PaidFor = paidFor;
                    transaction.PaidBy = paidBy;
                    transaction.PaidDate = Convert.ToDateTime(paidDate);
                    transaction.PaidAmt = Convert.ToInt32(totalAmount);
                    transaction.TransactionType = "Credit";
                    transaction.Balance = Convert.ToInt32(totalAmount);
                    transaction.CarryForward = 0;
                    transaction.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    transaction.CreatedDate = DateTime.Now.Date;
                    worker.InHouseTransactionEntity.Insert(transaction);
                    worker.Save();

                    var Id = transaction.Id;
                    TransactionNo = Id == 1 ? "TR-1" : "TR-" + Id;

                    var record = worker.InHouseTransactionEntity.Get(x => x.Id == Id).ToList();
                    if (record != null)
                    {
                        transaction.TransactionNo = TransactionNo;
                        worker.Save();
                    }
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
                model.TransactionNo = list.TransactionNo;
                model.PaidBy = list.PaidBy;
                return Json(model, JsonRequestBehavior.AllowGet);               
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MakePayment(InHouseVoucherModel model)
        {
            try
            {
                InHouseVoucher voucher = new InHouseVoucher();
                voucher.TransactionNo = model.TransactionNo;
                voucher.PaymentFrom = model.PaymentFrom;
                voucher.PaymentDate = model.PaymentDate;
                voucher.PaymentAmt = model.PaymentAmt;
                voucher.TransactionType = "Debit";
                voucher.PaymentGivenTo = model.PaymentGivenTo;
                voucher.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                voucher.CreatedDate = DateTime.Now.Date;
                worker.InHouseVoucherEntity.Insert(voucher);
                worker.Save();

                InHouse TransData = worker.InHouseTransactionEntity.GetByID(model.Id);
                var balance = TransData.Balance - voucher.PaymentAmt;
                TransData.Balance = balance;
                worker.InHouseTransactionEntity.Update(TransData);
                worker.Save();

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult InHouseDetails(int Id)
        {
            InHouseViewModel model = new InHouseViewModel();
            model.ViewModel = GetTransactionDetail(Id);
            model.VoucherModel = GetVoucherDetails(Id);
            return PartialView("_InHouseDetails", model);
        }

        public InHouseViewModel GetTransactionDetail(int Id)
        {
            InHouseViewModel inHouses = new InHouseViewModel();
            var list = worker.InHouseTransactionEntity.GetByID(Id);
            if (list != null)
            {
                inHouses.Id = list.Id;
                inHouses.TransactionNo = list.TransactionNo;
                inHouses.PaidFor = list.PaidFor;
                inHouses.PaidBy = list.PaidBy;
                inHouses.PaidDate = list.PaidDate;
                inHouses.PaidAmt = list.PaidAmt;
                inHouses.Balance = list.Balance;
                inHouses.CarryForward = list.CarryForward;
                inHouses.TransactionType = list.TransactionType;
            }
            
            return inHouses;
        }

        public List<InHouseVoucherModel> GetVoucherDetails(int Id)
        {
            List<InHouseVoucherModel> inHouseVouchers = new List<InHouseVoucherModel>();
            var list = worker.InHouseTransactionEntity.GetByID(Id);
            var data = worker.InHouseVoucherEntity.Get(x => x.TransactionNo == list.TransactionNo).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    inHouseVouchers.Add(new InHouseVoucherModel
                    {
                        Id = item.Id,
                        TransactionNo = item.TransactionNo,
                        PaymentFrom = item.PaymentFrom,
                        PaymentDate = item.PaymentDate,
                        PaymentAmt = item.PaymentAmt,
                        TransactionType = item.TransactionType,
                        PaymentGivenTo = item.PaymentGivenTo
                    });
                }
            }
            return inHouseVouchers;
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
        public ActionResult SearchList(string PurchaseName, string StartDate, string EndDate)
        {
            try
            {
                List<InHouseViewModel> model = new List<InHouseViewModel>();
                var SDate = StartDate != "" ? Convert.ToDateTime(StartDate).Date : DateTime.Now;
                var EDate = EndDate != "" ? Convert.ToDateTime(EndDate).Date : DateTime.Now;

                if (PurchaseName != "" && StartDate != "" && EndDate != "")
                {
                    var list = worker.InHouseTransactionEntity.Get(x => x.PaidBy == PurchaseName && x.PaidDate >= SDate && x.PaidDate <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new InHouseViewModel
                        {
                            Id = item.Id,
                            PaidFor = item.PaidFor,
                            PaidBy = item.PaidBy,
                            PaidDate = item.PaidDate,
                            PaidAmt = item.PaidAmt,
                            Balance = item.Balance,
                            CarryForward = item.CarryForward
                        });
                    }
                }
                else if (PurchaseName != "" && StartDate != "" && EndDate == "")
                {
                    var list = worker.InHouseTransactionEntity.Get(x => x.PaidBy == PurchaseName && x.PaidDate == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new InHouseViewModel
                        {
                            Id = item.Id,
                            PaidFor = item.PaidFor,
                            PaidBy = item.PaidBy,
                            PaidDate = item.PaidDate,
                            PaidAmt = item.PaidAmt,
                            Balance = item.Balance,
                            CarryForward = item.CarryForward
                        });
                    }
                }
                else if (PurchaseName != "" && EndDate != "" && StartDate == "")
                {
                    var list = worker.InHouseTransactionEntity.Get(x => x.PaidBy == PurchaseName && x.PaidDate <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new InHouseViewModel
                        {
                            Id = item.Id,
                            PaidFor = item.PaidFor,
                            PaidBy = item.PaidBy,
                            PaidDate = item.PaidDate,
                            PaidAmt = item.PaidAmt,
                            Balance = item.Balance,
                            CarryForward = item.CarryForward
                        });
                    }
                }
                else if (PurchaseName == "" && EndDate != "" && StartDate != "")
                {
                    var list = worker.InHouseTransactionEntity.Get(x => x.PaidDate >= SDate && x.PaidDate <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new InHouseViewModel
                        {
                            Id = item.Id,
                            PaidFor = item.PaidFor,
                            PaidBy = item.PaidBy,
                            PaidDate = item.PaidDate,
                            PaidAmt = item.PaidAmt,
                            Balance = item.Balance,
                            CarryForward = item.CarryForward
                        });
                    }
                }
                else if (PurchaseName != null && StartDate == "" && EndDate == "")
                {
                    var list = worker.InHouseTransactionEntity.Get(x => x.PaidBy == PurchaseName).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new InHouseViewModel
                        {
                            Id = item.Id,
                            PaidFor = item.PaidFor,
                            PaidBy = item.PaidBy,
                            PaidDate = item.PaidDate,
                            PaidAmt = item.PaidAmt,
                            Balance = item.Balance,
                            CarryForward = item.CarryForward
                        });
                    }
                }
                else if (StartDate != "" && PurchaseName == "" && EndDate == "")
                {
                    var list = worker.InHouseTransactionEntity.Get(x => x.PaidDate == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new InHouseViewModel
                        {
                            Id = item.Id,
                            PaidFor = item.PaidFor,
                            PaidBy = item.PaidBy,
                            PaidDate = item.PaidDate,
                            PaidAmt = item.PaidAmt,
                            Balance = item.Balance,
                            CarryForward = item.CarryForward
                        });
                    }
                }
                else if (EndDate != "" && StartDate == "" && PurchaseName == "")
                {
                    var list = worker.InHouseTransactionEntity.Get(x => x.PaidDate == EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new InHouseViewModel
                        {
                            Id = item.Id,
                            PaidFor = item.PaidFor,
                            PaidBy = item.PaidBy,
                            PaidDate = item.PaidDate,
                            PaidAmt = item.PaidAmt,
                            Balance = item.Balance,
                            CarryForward = item.CarryForward
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