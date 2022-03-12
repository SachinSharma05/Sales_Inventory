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
                int AdvanceGrossTotal = 0;
                int MiscGrossTotal = 0;

                var PurchaseCount = worker.PurchaseEntity.Get().Count();
                var SalesCount = worker.SaleEntity.Get().Count();
                var PaymentCount = worker.PaymentEntity.Get().Count();
                var PaymentReceiptCount = worker.PaymentReceiptEntity.Get().Count();
                var TotalStock = worker.PurchaseProductEntity.Get().Count();
                var InHouseTransaction = worker.InHouseTransactionEntity.Get().Count();
                var AdvanceTotal = worker.AdvanceEntity.Get().GroupBy(x => x.Advance_Amount).Select(n => n.Sum(m => m.Advance_Amount)).ToList();
                var MiscTotal = worker.MiscExpensesEntity.Get().GroupBy(x => x.ExpenseAmt).Select(n => n.Sum(m => m.ExpenseAmt)).ToList();

                foreach (var item in AdvanceTotal)
                {
                    AdvanceGrossTotal += Convert.ToInt32(item);
                }

                foreach (var item in MiscTotal)
                {
                    MiscGrossTotal += Convert.ToInt32(item);
                }

                TempData["PurchaseCount"] = PurchaseCount;
                TempData["SalesCount"] = SalesCount;
                TempData["PaymentCount"] = PaymentCount;
                TempData["PaymentReceiptCount"] = PaymentReceiptCount;
                TempData["TotalStockCount"] = TotalStock;
                TempData["TotalInHouseTransaction"] = InHouseTransaction;
                TempData["AdvanceTotal"] = AdvanceGrossTotal;
                TempData["MiscTotal"] = MiscGrossTotal;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return View();
        }
        #endregion
    }
}