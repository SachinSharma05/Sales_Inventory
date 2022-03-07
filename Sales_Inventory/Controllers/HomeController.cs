using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class HomeController : BaseController
    {
        DBWorker worker = new DBWorker();

        #region Dashboard
        public ActionResult Index()
        {
            try
            {
                var EmployeeCount = worker.EmployeeEntity.Get().Count();
                var PurchaseCount = worker.PurchaseEntity.Get().Count();
                var SalesCount = worker.SaleEntity.Get().Count();
                var PaymentCount = worker.PaymentEntity.Get().Count();
                var PaymentReceiptCount = worker.PaymentReceiptEntity.Get().Count();
                var TotalStock = worker.PurchaseProductEntity.Get().Count();
                var InHouseTransaction = worker.InHouseTransactionEntity.Get().Count();

                TempData["EmployeeCount"] = EmployeeCount;
                TempData["PurchaseCount"] = PurchaseCount;
                TempData["SalesCount"] = SalesCount;
                TempData["PaymentCount"] = PaymentCount;
                TempData["PaymentReceiptCount"] = PaymentReceiptCount;
                TempData["TotalStockCount"] = TotalStock;
                TempData["TotalInHouseTransaction"] = InHouseTransaction;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return View();
        }
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

                foreach (var item in PayCashOut)
                {
                    DayCashModel model = new DayCashModel();
                    model.Name = item.Payment_To;
                    model.PaidDate = item.Payment_Date;
                    model.PaidAmount = item.Paid_Amount;
                    dayCashModel.Add(model);
                    DayCashOut += Convert.ToInt32(item.Paid_Amount);
                }

                foreach (var item in AdvCashOut)
                {
                    DayCashModel model = new DayCashModel();
                    model.Name = item.Advance_To;
                    model.PaidDate = item.Advance_Date;
                    model.PaidAmount = Convert.ToInt32(item.Advance_Amount);
                    dayCashModel.Add(model);
                    DayCashOut += Convert.ToInt32(item.Advance_Amount);
                }

                foreach (var item in MiscCashOut)
                {
                    DayCashModel model = new DayCashModel();
                    model.Name = item.Name;
                    model.PaidDate = item.ExpenseDate;
                    model.PaidAmount = item.ExpenseAmt;
                    dayCashModel.Add(model);
                    DayCashOut += Convert.ToInt32(item.ExpenseAmt);
                }

                ViewBag.DayCashOut = DayCashOut;

                return View(dayCashModel);
            }
            catch(Exception ex)
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
            var query = worker.ProductTypeEntity.Get().ToList().OrderBy(x => x.Product);

            var list = new List<SelectListItem> { new SelectListItem { Value = null, Text = "" } };
            list.AddRange(query.ToList().Select(C => new SelectListItem
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
                        Quantity = item.Quantity,
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
                            Quantity = item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
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
                            Quantity = item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
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
                            Quantity = item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
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
                            Quantity = item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
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
                            Quantity = item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
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
                            Quantity = item.Quantity,
                            CreatedDate = item.CreatedDate
                        });
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
                            Quantity = item.Quantity,
                            CreatedDate = item.CreatedDate
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

        #region Add New Product
        public ActionResult AddNewProduct()
        {
            return PartialView("_AddNewProduct");
        }
        [HttpPost]
        public ActionResult AddNewProduct(string Product)
        {
            try
            {
                ProductType model = new ProductType();
                model.Product = Product;
                model.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                model.CreatedDate = DateTime.Now.Date;
                worker.ProductTypeEntity.Insert(model);
                worker.Save();
                return PartialView("_AddNewProduct");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}