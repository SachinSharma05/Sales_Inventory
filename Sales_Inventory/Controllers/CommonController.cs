using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class CommonController : BaseController
    {
        #region
        DBWorker worker = new DBWorker();
        #endregion

        #region Day Cash Book
        public ActionResult DayCashBook()
        {
            try
            {
                List<DayCashModel> dayCashModel = new List<DayCashModel>();
                int DayCashIn = 0;
                int DayCashOut = 0;

                var Date = DateTime.Now.Date;

                var PayCashOut = worker.PaymentEntity.Get(x => x.Payment_Date == Date);
                var AdvCashOut = worker.AdvanceEntity.Get(x => x.Advance_Date == Date);
                var MiscCashOut = worker.MiscExpensesEntity.Get(x => x.ExpenseDate == Date);

                var PayCashIn = worker.PaymentReceiptEntity.Get(x => x.ReceivedDate == Date);

                foreach (var item in PayCashOut)
                {
                    DayCashModel model = new DayCashModel();
                    model.Name = item.Payment_To;
                    model.PaidDate = item.Payment_Date;
                    model.PaidAmount = (decimal)item.Paid_Amount;
                    model.Action = "Payment Done";
                    dayCashModel.Add(model);
                    DayCashOut += Convert.ToInt32(item.Paid_Amount);
                }

                foreach (var item in AdvCashOut)
                {
                    DayCashModel model = new DayCashModel();
                    model.Name = item.Advance_To;
                    model.PaidDate = item.Advance_Date;
                    model.PaidAmount = Convert.ToInt32(item.Advance_Amount);
                    model.Action = "Advance Given";
                    dayCashModel.Add(model);
                    DayCashOut += Convert.ToInt32(item.Advance_Amount);
                }

                foreach (var item in MiscCashOut)
                {
                    DayCashModel model = new DayCashModel();
                    model.Name = item.Name;
                    model.PaidDate = item.ExpenseDate;
                    model.PaidAmount = (decimal)item.ExpenseAmt;
                    model.Action = "Miscellaneous Expense";
                    dayCashModel.Add(model);
                    DayCashOut += Convert.ToInt32(item.ExpenseAmt);
                }

                foreach (var item in PayCashIn)
                {
                    DayCashModel model = new DayCashModel();
                    model.Name = item.ReceivedFrom;
                    model.PaidDate = item.ReceivedDate;
                    model.PaidAmount = (decimal)item.PaidAmount;
                    model.Action = "Payment Received";
                    dayCashModel.Add(model);
                    DayCashIn += Convert.ToInt32(item.PaidAmount);
                }

                ViewBag.DayCashOut = DayCashOut;
                ViewBag.DayCashIn = DayCashIn;
                ViewBag.DayFinalAmount = DayCashOut - DayCashIn;

                return View(dayCashModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Total Stock
        public ActionResult TotalStockList()
        {
            Purchase_Products model = new Purchase_Products();
            model.ProductName = GetProductTypeList();
            model.List = GetStockList();
            return View(model);
        }
        public List<SelectListItem> GetProductTypeList()
        {
            var query = worker.ProductTypeEntity.Get().GroupBy(x => x.Product).Select(g => g.First()).OrderBy(x => x.Product);
            var newList = query.ToList();
            var list = new List<SelectListItem> { new SelectListItem { Value = null, Text = "" } };
            list.AddRange(newList.ToList().Select(C => new SelectListItem
            {
                Value = C.Id.ToString(),
                Text = C.Product
            }));

            ViewBag.ProductList = list;

            return list;
        }
        public List<Purchase_Products> GetStockList()
        {
            List<Purchase_Products> StockList = new List<Purchase_Products>();
            var list = worker.PurchaseProductEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    StockList.Add(new Purchase_Products
                    {
                        Id = item.Id,
                        ItemName = item.ItemName,
                        Quantity = (decimal)item.Quantity,
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            return StockList;
        }
        public ActionResult SearchList(string PurchaseName, string StartDate, string EndDate)
        {
            try
            {
                decimal TotalStock = 0;
                List<Purchase_Products> model = new List<Purchase_Products>();
                var SDate = StartDate != "" ? Convert.ToDateTime(StartDate).Date : DateTime.Now;
                var EDate = EndDate != "" ? Convert.ToDateTime(EndDate).Date : DateTime.Now;

                if (PurchaseName != "" && StartDate != "" && EndDate != "")
                {
                    var list = worker.PurchaseProductEntity.Get(x => x.ItemName == PurchaseName && x.CreatedDate >= SDate && x.CreatedDate <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new Purchase_Products
                        {
                            Id = item.Id,
                            ItemName = item.ItemName,
                            Quantity = (decimal)item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
                        TotalStock += (decimal)item.Quantity;
                    }
                }
                else if (PurchaseName != "" && StartDate != "" && EndDate == "")
                {
                    var list = worker.PurchaseProductEntity.Get(x => x.ItemName == PurchaseName && x.CreatedDate == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new Purchase_Products
                        {
                            Id = item.Id,
                            ItemName = item.ItemName,
                            Quantity = (decimal)item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
                        TotalStock += (decimal)item.Quantity;
                    }
                }
                else if (PurchaseName != "" && EndDate != "" && StartDate == "")
                {
                    var list = worker.PurchaseProductEntity.Get(x => x.ItemName == PurchaseName && x.CreatedDate == EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new Purchase_Products
                        {
                            Id = item.Id,
                            ItemName = item.ItemName,
                            Quantity = (decimal)item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
                        TotalStock += (decimal)item.Quantity;
                    }
                }
                else if (StartDate != "" && EndDate != "" && PurchaseName == "")
                {
                    var list = worker.PurchaseProductEntity.Get(x => x.CreatedDate >= SDate && x.CreatedDate <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new Purchase_Products
                        {
                            Id = item.Id,
                            ItemName = item.ItemName,
                            Quantity = (decimal)item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
                        TotalStock += (decimal)item.Quantity;
                    }
                }
                else if (PurchaseName != null && StartDate == "" && EndDate == "")
                {
                    var list = worker.PurchaseProductEntity.Get(x => x.ItemName == PurchaseName).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new Purchase_Products
                        {
                            Id = item.Id,
                            ItemName = item.ItemName,
                            Quantity = (decimal)item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
                        TotalStock += (decimal)item.Quantity;
                    }
                }
                else if (StartDate != "" && PurchaseName == "" && EndDate == "")
                {
                    var list = worker.PurchaseProductEntity.Get(x => x.CreatedDate == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new Purchase_Products
                        {
                            Id = item.Id,
                            ItemName = item.ItemName,
                            Quantity = (decimal)item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
                        TotalStock += (decimal)item.Quantity;
                    }
                }
                else if (EndDate != "" && StartDate == "" && PurchaseName == "")
                {
                    var list = worker.PurchaseProductEntity.Get(x => x.CreatedDate <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new Purchase_Products
                        {
                            Id = item.Id,
                            ItemName = item.ItemName,
                            Quantity = (decimal)item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
                        TotalStock += (decimal)item.Quantity;
                    }
                }
                ViewBag.TotalStock = TotalStock;
                return PartialView("_SearchList", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Add New Product
        [HttpPost]
        public ActionResult AddNewProduct(string Product)
        {
            try
            {
                if(Product != null)
                {
                    ProductType model = new ProductType();
                    model.Product = Product;
                    model.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    model.CreatedDate = DateTime.Now.Date;
                    worker.ProductTypeEntity.Insert(model);
                    worker.Save();
                }
               return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Credit Transaction
        public ActionResult CreditList()
        {
            AddNewCredits model = new AddNewCredits();
            model.creditList = GetCreditList();
            return View(model);
        }
        public List<AddNewCredits> GetCreditList()
        {
            List<AddNewCredits> CreditList = new List<AddNewCredits>();
            var list = worker.AddNewCreditEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    CreditList.Add(new AddNewCredits
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Amount = item.Amount,
                        Date = item.Date,
                        Type = item.Type,
                        Remarks = item.Remarks,
                        CreatedOn = item.CreatedOn
                    });
                }
            }
            return CreditList;
        }

        [HttpPost]
        public ActionResult AddNewCredit(string Name, string Amount, string CreditDate, string CreditType, string Remarks)
        {
            try
            {
                AddNewCredit credit = new AddNewCredit();
                credit.Name = Name;
                credit.Amount = Convert.ToInt32(Amount);
                credit.Date = Convert.ToDateTime(CreditDate);
                credit.Type = CreditType;
                credit.Remarks = Remarks;
                credit.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                credit.CreatedOn = DateTime.Now.Date;
                worker.AddNewCreditEntity.Insert(credit);
                worker.Save();
                return RedirectToAction("CreditList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult DeleteCredit(int id)
        {
            var model = Delete(id);
            return RedirectToAction("CreditList"); ;
        }
        public bool Delete(int id)
        {

            var result = worker.AddNewCreditEntity.GetByID(id);
            if (result != null)
            {
                worker.AddNewCreditEntity.Delete(result);
                worker.Save();
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}