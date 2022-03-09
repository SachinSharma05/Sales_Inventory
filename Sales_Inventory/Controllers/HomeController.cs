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
    }
}