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
            Stock();
            var EmployeeCount = worker.EmployeeEntity.Get().ToList().Count;
            var PurchaseCount = worker.PurchaseEntity.Get().ToList().Count;
            var SalesCount = worker.SaleEntity.Get().ToList().Count;
            var StockCount = StockList();
            TempData["EmployeeCount"] = EmployeeCount;
            TempData["PurchaseCount"] = PurchaseCount;
            TempData["SalesCount"] = SalesCount;
            return View(StockCount);
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

        public ActionResult Stock()
        {
            var result = worker.StockEntity.context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Stock]");

            var StockCount = worker.PurchaseProductEntity.Get().GroupBy(x => x.ItemName).Select(x => new StockViewModel { Product = x.Key, TotalQuantity = x.Sum(y => y.Quantity) }).ToList();
            foreach (var item in StockCount)
            {
                Stock stock = new Stock();
                stock.Product = item.Product;
                stock.TotalQuantity = item.TotalQuantity;
                worker.StockEntity.Insert(stock);
                worker.Save();
            }

            return RedirectToAction("List");
        }
    }
}