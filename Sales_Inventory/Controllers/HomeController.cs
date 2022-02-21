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
            var PaymentReceiptCount = worker.PaymentReceiptEntity.Get().ToList().Count;
            var TotalStock = worker.PurchaseProductEntity.Get().ToList().Count;

            TempData["EmployeeCount"] = EmployeeCount;
            TempData["PurchaseCount"] = PurchaseCount;
            TempData["SalesCount"] = SalesCount;
            TempData["PaymentCount"] = PaymentCount;
            TempData["PaymentReceiptCount"] = PaymentReceiptCount;
            TempData["TotalStockCount"] = TotalStock;

            return View();
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

        public ActionResult TotalStock(string StartDate, string EndDate)
        {
            try
            {
                if (StartDate != null && EndDate != null)
                {
                    return View(GetFilterStockList(StartDate, EndDate));
                }
                else
                {
                    return View(GetStockList());
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
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
        public List<Purchase_Products> GetFilterStockList(string StartDate, string EndDate)
        {
            List<Purchase_Products> FilterStockList = new List<Purchase_Products>();
            var NewStartDate = Convert.ToDateTime(StartDate);
            var NewEndDate = Convert.ToDateTime(EndDate);
            var list = worker.PurchaseProductEntity.Get(x => x.CreatedDate == NewStartDate && x.CreatedDate == NewEndDate).ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    FilterStockList.Add(new Purchase_Products
                    {
                        Id = item.Id,
                        ItemName = item.ItemName,
                        Quantity = item.Quantity,
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            return FilterStockList;
        }
    }
}