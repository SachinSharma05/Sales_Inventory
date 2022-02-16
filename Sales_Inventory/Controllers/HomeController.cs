using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class HomeController : Controller
    {
        DBWorker worker = new DBWorker();

        public ActionResult Index()
        {
            var EmployeeCount = worker.EmployeeEntity.Get().ToList().Count;
            var PurchaseCount = worker.PurchaseEntity.Get().ToList().Count;
            var SalesCount = worker.SaleEntity.Get().ToList().Count;
            var PaymentCount = worker.PaymentEntity.Get().ToList().Count;

            CommonViewModel commonViewModel = new CommonViewModel();
            commonViewModel.stockViewModels = StockList();
            commonViewModel.purchaseViewModels = BalanceList();

            TempData["EmployeeCount"] = EmployeeCount;
            TempData["PurchaseCount"] = PurchaseCount;
            TempData["SalesCount"] = SalesCount;
            TempData["PaymentCount"] = PaymentCount;

            return View(commonViewModel);
        }

        public List<StockViewModel> StockList()
        {
            List<StockViewModel> stockList = new List<StockViewModel>();
            var StockCount = worker.StockEntity.Get().ToList();
            if (StockCount.Count > 0)
            {
                foreach (var item in StockCount)
                {
                    stockList.Add(new StockViewModel
                    {
                        Id = item.Id,
                        Product = item.Product,
                        TotalQuantity = item.TotalQuantity
                    });
                }
            }
            return stockList;
        }

        public List<PurchaseViewModel> BalanceList()
        {
            List<PurchaseViewModel> balanceList = new List<PurchaseViewModel>();
            var balanceCount = worker.PurchaseEntity.Get(x => x.Balance > 0).ToList();
            if (balanceCount.Count > 0)
            {
                foreach (var item in balanceCount)
                {
                    balanceList.Add(new PurchaseViewModel
                    {
                        Id = item.Id,
                        Purchase_No = item.Purchase_No,
                        Purchase_From = item.Purchase_From,
                        Balance = item.Balance
                    });
                }
            }
            return balanceList;
        }

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

        //public ActionResult Stock()
        //{
        //    var result = worker.StockEntity.context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Stock]");

        //    var StockCount = worker.PurchaseProductEntity.Get().GroupBy(x => x.ItemName).Select(x => new StockViewModel { Product = x.Key, TotalQuantity = x.Sum(y => y.Quantity) }).ToList();
        //    foreach (var item in StockCount)
        //    {
        //        Stock stock = new Stock();
        //        stock.Product = item.Product;
        //        stock.TotalQuantity = item.TotalQuantity;
        //        worker.StockEntity.Insert(stock);
        //        worker.Save();
        //    }

        //    return RedirectToAction("List");
        //}
    }
}