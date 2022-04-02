using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class PaymentController : BaseController
    {
        DBWorker worker = new DBWorker();

        // GET: Payment
        #region Payment List
        public ActionResult List()
        {
            PaymentModel model = new PaymentModel();
            model.PaymentName = GetPaymentName();
            model.List = GetPaymentList();
            return View(model);
        }
        public List<PaymentModel> GetPaymentList()
        {
            List<PaymentModel> PaymentList = new List<PaymentModel>();
            var list = worker.PaymentEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    PaymentList.Add(new PaymentModel
                    {
                        Id = item.Id,
                        Purchase_No = item.Purchase_No,
                        Payment_To = item.Payment_To,
                        Payment_Date = item.Payment_Date,
                        Payment_Type = item.Payment_Type,
                        Paid_Amount = (decimal)item.Paid_Amount,
                        Balance = (decimal)item.Balance
                    });
                }
            }
            ViewBag.PaymentListModel = worker.PurchaseEntity.Get(x => x.Balance > 0).ToList();
            return PaymentList;
        }
        public List<SelectListItem> GetPaymentName()
        {
            var query = worker.PaymentEntity.Get().ToList();

            var list = new List<SelectListItem> { new SelectListItem { Value = null, Text = "" } };
            list.AddRange(query.ToList().Select(C => new SelectListItem
            {
                Value = C.Id.ToString(),
                Text = C.Payment_To
            }));

            return list;
        }
        #endregion

        #region Create Payment
        public ActionResult Create(int Id)
        {
            PaymentModel model = new PaymentModel();
            model.Id = Id;
            var data = worker.PurchaseEntity.GetByID(model.Id);
            model.Total_Payment_Amount = (decimal)(data.Balance != null ? data.Balance : data.GrossTotal);
            model.Purchase_No = data.Purchase_No;
            model.Payment_To = data.Purchase_From;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(PaymentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Payment payment = new Payment();
                    payment.Purchase_No = model.Purchase_No;
                    payment.Payment_To = model.Payment_To;
                    payment.Payment_Date = model.Payment_Date;
                    payment.Payment_Type = model.Payment_Type;
                    payment.Total_Payment_Amount = model.Total_Payment_Amount;
                    payment.Contact_No = model.Contact_No;
                    payment.Paid_Amount = model.Paid_Amount;
                    payment.Bank_Name = model.Bank_Name;
                    payment.Account_No = model.Account_No;
                    payment.Account_Holder_Name = model.Account_Holder_Name;
                    payment.UPI_Id = model.UPI_Id;
                    payment.Cheque_No = model.Cheque_No;
                    payment.Cheque_Date = model.Cheque_Date;
                    payment.Name_On_Cheque = model.Name_On_Cheque;
                    payment.Balance = model.Balance;
                    payment.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    payment.CreatedDate = DateTime.Now.Date;
                    worker.PaymentEntity.Insert(payment);
                    worker.Save();

                    Purchase purchase = worker.PurchaseEntity.Get(x => x.Purchase_No == model.Purchase_No).FirstOrDefault();
                    purchase.Balance = model.Balance;
                    worker.PurchaseEntity.Update(purchase);
                    worker.Save();
                }
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Edit Payment
        public ActionResult Edit(int Id)
        {
            try
            {
                PaymentModel model = new PaymentModel();
                var payment = worker.PaymentEntity.GetByID(Id);
                model.Purchase_No = payment.Purchase_No;
                model.Payment_To = payment.Payment_To;
                model.Payment_Date = payment.Payment_Date;
                model.Payment_Type = payment.Payment_Type;
                model.Total_Payment_Amount = (decimal)payment.Total_Payment_Amount;
                model.Contact_No = payment.Contact_No;
                model.Paid_Amount = (decimal)payment.Paid_Amount;
                model.Bank_Name = payment.Bank_Name;
                model.Account_No = payment.Account_No;
                model.Account_Holder_Name = payment.Account_Holder_Name;
                model.UPI_Id = payment.UPI_Id;
                model.Cheque_No = payment.Cheque_No;
                model.Cheque_Date = payment.Cheque_Date;
                model.Name_On_Cheque = payment.Name_On_Cheque;
                model.Balance = (decimal)payment.Balance;
                return View(model);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(PaymentModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Payment payment = worker.PaymentEntity.GetByID(model.Id);
                    payment.Purchase_No = model.Purchase_No;
                    payment.Payment_To = model.Payment_To;
                    payment.Payment_Date = model.Payment_Date;
                    payment.Payment_Type = model.Payment_Type;
                    payment.Total_Payment_Amount = model.Total_Payment_Amount;
                    payment.Contact_No = model.Contact_No;
                    payment.Paid_Amount = model.Paid_Amount;
                    payment.Bank_Name = model.Bank_Name;
                    payment.Account_No = model.Account_No;
                    payment.Account_Holder_Name = model.Account_Holder_Name;
                    payment.UPI_Id = model.UPI_Id;
                    payment.Cheque_No = model.Cheque_No;
                    payment.Cheque_Date = model.Cheque_Date;
                    payment.Name_On_Cheque = model.Name_On_Cheque;
                    payment.Balance = model.Balance;
                    worker.PaymentEntity.Update(payment);
                    worker.Save();

                    Purchase purchase = worker.PurchaseEntity.Get(x => x.Purchase_No == model.Purchase_No).FirstOrDefault();
                    purchase.Balance = model.Balance;
                    worker.PurchaseEntity.Update(purchase);
                    worker.Save();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return RedirectToAction("List");
        }
        #endregion

        #region Delete Payment
        [HttpGet]
        public ActionResult Delete(int id)
        {
            DeletePayment(id);
            return RedirectToAction("List");
        }

        public bool DeletePayment(int id)
        {
            var payment = worker.PaymentEntity.GetByID(id);
            if (payment != null)
            {
                worker.PaymentEntity.Delete(payment);
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
                List<PaymentModel> model = new List<PaymentModel>();
                var SDate = StartDate != "" ? Convert.ToDateTime(StartDate).Date : DateTime.Now;
                var EDate = EndDate != "" ? Convert.ToDateTime(EndDate).Date : DateTime.Now;

                if (PurchaseName != "" && StartDate != "" && EndDate != "")
                {
                    var list = worker.PaymentEntity.Get(x => x.Payment_To == PurchaseName && x.Payment_Date >= SDate && x.Payment_Date <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentModel
                        {
                            Id = item.Id,
                            Purchase_No = item.Purchase_No,
                            Payment_To = item.Payment_To,
                            Payment_Date = item.Payment_Date,
                            Payment_Type = item.Payment_Type,
                            Paid_Amount = (decimal)item.Paid_Amount,
                            Balance = (decimal)item.Balance
                        });
                    }
                }
                else if (PurchaseName != "" && StartDate != "" && EndDate == "")
                {
                    var list = worker.PaymentEntity.Get(x => x.Payment_To == PurchaseName && x.Payment_Date == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentModel
                        {
                            Id = item.Id,
                            Purchase_No = item.Purchase_No,
                            Payment_To = item.Payment_To,
                            Payment_Date = item.Payment_Date,
                            Payment_Type = item.Payment_Type,
                            Paid_Amount = (decimal)item.Paid_Amount,
                            Balance = (decimal)item.Balance
                        });
                    }
                }
                else if (PurchaseName != "" && EndDate != "" && StartDate == "")
                {
                    var list = worker.PaymentEntity.Get(x => x.Payment_To == PurchaseName && x.Payment_Date == EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentModel
                        {
                            Id = item.Id,
                            Purchase_No = item.Purchase_No,
                            Payment_To = item.Payment_To,
                            Payment_Date = item.Payment_Date,
                            Payment_Type = item.Payment_Type,
                            Paid_Amount = (decimal)item.Paid_Amount,
                            Balance = (decimal)item.Balance
                        });
                    }
                }
                else if (StartDate != "" && EndDate != "" && PurchaseName == "")
                {
                    var list = worker.PaymentEntity.Get(x => x.Payment_Date == SDate && x.Payment_Date == EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentModel
                        {
                            Id = item.Id,
                            Purchase_No = item.Purchase_No,
                            Payment_To = item.Payment_To,
                            Payment_Date = item.Payment_Date,
                            Payment_Type = item.Payment_Type,
                            Paid_Amount = (decimal)item.Paid_Amount,
                            Balance = (decimal)item.Balance
                        });
                    }
                }
                else if (PurchaseName != null && StartDate == "" && EndDate == "")
                {
                    var list = worker.PaymentEntity.Get(x => x.Payment_To == PurchaseName).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentModel
                        {
                            Id = item.Id,
                            Purchase_No = item.Purchase_No,
                            Payment_To = item.Payment_To,
                            Payment_Date = item.Payment_Date,
                            Payment_Type = item.Payment_Type,
                            Paid_Amount = (decimal)item.Paid_Amount,
                            Balance = (decimal)item.Balance
                        });
                    }
                }
                else if (StartDate != "" && PurchaseName == "" && EndDate == "")
                {
                    var list = worker.PaymentEntity.Get(x => x.Payment_Date == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentModel
                        {
                            Id = item.Id,
                            Purchase_No = item.Purchase_No,
                            Payment_To = item.Payment_To,
                            Payment_Date = item.Payment_Date,
                            Payment_Type = item.Payment_Type,
                            Paid_Amount = (decimal)item.Paid_Amount,
                            Balance = (decimal)item.Balance
                        });
                    }
                }
                else if (EndDate != "" && StartDate == "" && PurchaseName == "")
                {
                    var list = worker.PaymentEntity.Get(x => x.Payment_Date <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new PaymentModel
                        {
                            Id = item.Id,
                            Purchase_No = item.Purchase_No,
                            Payment_To = item.Payment_To,
                            Payment_Date = item.Payment_Date,
                            Payment_Type = item.Payment_Type,
                            Paid_Amount = (decimal)item.Paid_Amount,
                            Balance = (decimal)item.Balance
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