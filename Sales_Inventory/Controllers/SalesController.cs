using Newtonsoft.Json;
using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class SalesController : Controller
    {
        DBWorker worker = new DBWorker();

        // GET: Sales
        #region Sale List
        public ActionResult List()
        {
            return View(GetSaleList());
        }
        public List<SalesViewModel> GetSaleList()
        {
            List<SalesViewModel> SaleList = new List<SalesViewModel>();
            var list = worker.SaleEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    SaleList.Add(new SalesViewModel
                    {
                        Id = item.Id,
                        Sale_No = item.Sale_No,
                        Sale_To = item.Sale_To,
                        Sale_To_Phone = item.Sale_To_Phone,
                        Sale_Date = item.Sale_Date,
                        GrossTotal = item.GrossTotal,
                        Balance = item.Balance
                    });
                }
            }
            return SaleList;
        }
        #endregion

        #region Create Sale
        public List<SelectListItem> GetProductTypeList()
        {
            var query = worker.ProductTypeEntity.Get().ToList();

            var list = new List<SelectListItem> { new SelectListItem { Value = null, Text = "Select Product" } };
            list.AddRange(query.ToList().Select(C => new SelectListItem
            {
                Value = C.Id.ToString(),
                Text = C.Product
            }));

            ViewBag.ProductList = list;

            return list;
        }
        public ActionResult Create()
        {
            SalesViewModel viewModel = new SalesViewModel();
            viewModel.sale_Products = new List<Sale_Products>();
            viewModel.ProductList = GetProductTypeList();
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(string saleTo, string buyerPhoneNo, string saleDate, List<string> sale_Prod)
        {
            string json = sale_Prod[0].ToString();
            string Sale_No = "";
            int GrossTotal = 0;
            List<Sale_Products> sale_Products = JsonConvert.DeserializeObject<List<Sale_Products>>(json);
            try
            {
                foreach (var item in sale_Products)
                {
                    GrossTotal += Convert.ToInt32(item.Total);
                }

                if (ModelState.IsValid)
                {
                    Sale sale = new Sale();
                    sale.Sale_To = saleTo;
                    sale.Sale_To_Phone = buyerPhoneNo;
                    sale.Sale_Date = Convert.ToDateTime(saleDate);
                    sale.GrossTotal = GrossTotal;
                    sale.Balance = GrossTotal;
                    sale.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    sale.CreatedDate = DateTime.Now.Date;
                    worker.SaleEntity.Insert(sale);
                    worker.Save();

                    var Id = sale.Id;
                    Sale_No = Id == 1 ? "SL-1" : "SL-" + Id;

                    var record = worker.SaleEntity.Get(x => x.Id == Id).ToList();
                    if(record != null)
                    {
                        sale.Sale_No = Sale_No;
                        worker.Save();
                    }

                    foreach (var item in sale_Products)
                    {
                        Sale_Product sale_Product = new Sale_Product();
                        sale_Product.Sale_No = sale.Sale_No;
                        sale_Product.Item = item.Item;
                        sale_Product.Quantity = item.Quantity;
                        sale_Product.Price = item.Price;
                        sale_Product.Total = item.Total;
                        sale_Product.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                        sale_Product.CreatedDate = DateTime.Now.Date;
                        worker.SaleProductEntity.Insert(sale_Product);
                        worker.Save();
                    }

                    foreach (var item in sale_Products)
                    {
                        var stockList = worker.StockEntity.Get(x => x.Product == item.Item).ToList();
                        if(stockList.Count > 0)
                        {
                            Stock stockItem = worker.StockEntity.GetByID(stockList[0].Id);
                            stockItem.TotalQuantity = stockItem.TotalQuantity - item.Quantity;
                            stockItem.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                            stockItem.CreatedDate = DateTime.Now.Date;
                            worker.StockEntity.Update(stockItem);
                            worker.Save();
                        }
                        else
                        {
                            Stock stock = new Stock();
                            stock.Product = item.Item;
                            stock.TotalQuantity = item.Quantity;
                            stock.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                            stock.CreatedDate = DateTime.Now.Date;
                            worker.StockEntity.Insert(stock);
                            worker.Save();
                        }
                    }
                }
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Edit Sale
        public ActionResult Edit(int Id)
        {
            try
            {
                SalesViewModel model = new SalesViewModel();
                List<Sale_Products> sale_Products = new List<Sale_Products>();
                var sale = worker.SaleEntity.GetByID(Id);
                model.Id = sale.Id;
                model.Sale_No = sale.Sale_No;
                model.Sale_To = sale.Sale_To;
                model.Sale_To_Phone = sale.Sale_To_Phone;
                model.Sale_Date = sale.Sale_Date;
                model.GrossTotal = sale.GrossTotal;
                model.Balance = sale.Balance;

                var sale_prod = worker.SaleProductEntity.Get(x => x.Sale_No == sale.Sale_No).ToList();
                foreach (var item in sale_prod)
                {
                    Sale_Products sale_Product = new Sale_Products();
                    sale_Product.Sale_No = sale.Sale_No;
                    sale_Product.Item = item.Item;
                    sale_Product.Quantity = item.Quantity;
                    sale_Product.Price = item.Price;
                    sale_Product.Total = item.Total;
                    sale_Products.Add(sale_Product);
                }
                model.sale_Products = sale_Products;
                model.ProductList = GetProductTypeList();
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete Sales
        [HttpGet]
        public ActionResult Delete(int id)
        {
            DeleteSale(id);
            return RedirectToAction("List");
        }

        public bool DeleteSale(int id)
        {
            var sale = worker.SaleEntity.GetByID(id);
            DeleteSaleProduct(sale.Sale_No);
            if (sale != null)
            {
                worker.SaleEntity.Delete(sale);
                worker.Save();
                return true;
            }
            else
                return false;
        }

        public bool DeleteSaleProduct(string SaleNo)
        {
            var sale_product = worker.SaleProductEntity.Get(x => x.Sale_No == SaleNo).ToList();
            if (sale_product != null)
            {
                foreach (var item in sale_product)
                {
                    worker.SaleProductEntity.Delete(item);
                    worker.Save();
                }
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}