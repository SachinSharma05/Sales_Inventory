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
    public class PurchaseController : Controller
    {
        DBWorker worker = new DBWorker();

        // GET: Purchase
        #region Purchase List
        public ActionResult List()
        {
            return View(GetPurchaseList());
        }
        public List<PurchaseViewModel> GetPurchaseList()
        {
            List<PurchaseViewModel> PurchaseList = new List<PurchaseViewModel>();
            var list = worker.PurchaseEntity.Get().ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    PurchaseList.Add(new PurchaseViewModel
                    {
                        Id = item.Id,
                        Purchase_No = item.Purchase_No,
                        Purchase_From = item.Purchase_From,
                        Purchase_From_Phone = item.Purchase_From_Phone,
                        Purchase_Date = item.Purchase_Date,
                        GrossTotal = item.GrossTotal,
                        Balance = item.Balance
                    });
                }
            }
            return PurchaseList;
        }
        #endregion

        #region Create Purchase
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
            PurchaseViewModel viewModel = new PurchaseViewModel();
            viewModel.purchase_Products = new List<Purchase_Products>();
            viewModel.ProductList = GetProductTypeList();
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(string purchaseFrom, string sellerPhoneNo, string purchaseDate, List<string> purchase_Prod)
        {
            string json = purchase_Prod[0].ToString();
            string Purchase_No = "";
            int GrossTotal = 0;
            List<Purchase_Products> purchase_Products = JsonConvert.DeserializeObject<List<Purchase_Products>>(json);
            try
            {
                foreach (var item in purchase_Products)
                {
                    GrossTotal += Convert.ToInt32(item.Total);
                }

                if (ModelState.IsValid)
                {
                    Purchase pur = new Purchase();
                    pur.Purchase_From = purchaseFrom;
                    pur.Purchase_From_Phone = sellerPhoneNo;
                    pur.Purchase_Date = Convert.ToDateTime(purchaseDate);
                    pur.GrossTotal = GrossTotal;
                    pur.Balance = GrossTotal;
                    pur.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    pur.CreatedDate = DateTime.Now.Date;
                    worker.PurchaseEntity.Insert(pur);
                    worker.Save();

                    var Id = pur.Id;
                    Purchase_No = Id == 1 ? "PR-1" : "PR-" + Id;

                    var record = worker.SaleEntity.Get(x => x.Id == Id).ToList();
                    if (record != null)
                    {
                        pur.Purchase_No = Purchase_No;
                        worker.Save();
                    }

                    foreach (var item in purchase_Products)
                    {
                        Purchase_Product purchase_Product = new Purchase_Product();
                        purchase_Product.Purchase_No = pur.Purchase_No;
                        purchase_Product.ItemName = item.ItemName;
                        purchase_Product.Quantity = item.Quantity;
                        purchase_Product.Price = item.Price;
                        purchase_Product.Total = Convert.ToInt32(item.Total);
                        purchase_Product.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                        purchase_Product.CreatedDate = DateTime.Now.Date;
                        worker.PurchaseProductEntity.Insert(purchase_Product);
                        worker.Save();
                    }

                    foreach (var item in purchase_Products)
                    {
                        var stockList = worker.StockEntity.Get(x => x.Product == item.ItemName).ToList();                      
                        if(stockList.Count > 0)
                        {
                            Stock stockItem = worker.StockEntity.GetByID(stockList[0].Id);
                            stockItem.TotalQuantity = stockItem.TotalQuantity + item.Quantity;
                            stockItem.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                            stockItem.CreatedDate = DateTime.Now.Date;
                            worker.StockEntity.Update(stockItem);
                            worker.Save();
                        }
                        else
                        {
                            Stock stock = new Stock();
                            stock.Product = item.ItemName;
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

        //public bool UpdateStock(string ItemName)
        //{
        //    try
        //    {
        //        var chkProduct = worker.StockEntity.Get(x => x.Product == ItemName).ToList();
        //        var oldQuantity = worker.PurchaseProductEntity.Get(x => x.ItemName == ItemName).ToList();
        //        if (chkProduct.Count > 0)
        //        {
        //            Stock stock = new Stock();
        //            stock.TotalQuantity = oldQuantity[0].Quantity + chkProduct[0].TotalQuantity;
        //            stock.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
        //            stock.CreatedDate = DateTime.Now.Date;
        //            worker.StockEntity.Update(stock);
        //            worker.Save();
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool InsertStock()
        //{
        //    try
        //    {
        //        Stock stock = new Stock();
        //        stock.Product = item.ItemName;
        //        stock.TotalQuantity = item.Quantity;
        //        stock.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
        //        stock.CreatedDate = DateTime.Now.Date;
        //        worker.StockEntity.Insert(stock);
        //        worker.Save();

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        #endregion

        #region Edit Purchase
        public ActionResult Edit(int Id)
        {
            try
            {
                PurchaseViewModel model = new PurchaseViewModel();
                List<Purchase_Products> purchase_Products = new List<Purchase_Products>();
                var pur = worker.PurchaseEntity.GetByID(Id);
                model.Id = pur.Id;
                model.Purchase_No = pur.Purchase_No;
                model.Purchase_From = pur.Purchase_From;
                model.Purchase_From_Phone = pur.Purchase_From_Phone;
                model.Purchase_Date = pur.Purchase_Date;

                var pur_prod = worker.PurchaseProductEntity.Get(x => x.Purchase_No == pur.Purchase_No).ToList();
                foreach (var item in pur_prod)
                {
                    Purchase_Products purchase_Product = new Purchase_Products();
                    purchase_Product.Purchase_No = pur.Purchase_No;
                    purchase_Product.ItemName = item.ItemName;
                    purchase_Product.Quantity = item.Quantity;
                    purchase_Product.Price = item.Price;
                    purchase_Product.Total = Convert.ToString(item.Total);
                    purchase_Products.Add(purchase_Product);
                }
                model.purchase_Products = purchase_Products;
                model.ProductList = GetProductTypeList();
                return View(model);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(string purchaseId, string purchaseFrom, string purchaseBy, string sellerPhoneNo, string buyerPhoneNo, string purchaseDate, List<string> purchase_Prod)
        {
            string json = purchase_Prod[0].ToString();
            List<Purchase_Products> purchase_Products = JsonConvert.DeserializeObject<List<Purchase_Products>>(json);
            if (ModelState.IsValid)
            {
                try
                {
                    Purchase pur = worker.PurchaseEntity.GetByID(Convert.ToInt32(purchaseId));
                    pur.Purchase_From = purchaseFrom;
                    pur.Purchase_From_Phone = sellerPhoneNo;
                    pur.Purchase_Date = Convert.ToDateTime(purchaseDate);
                    worker.PurchaseEntity.Update(pur);
                    worker.Save();

                    var pur_prod = worker.PurchaseProductEntity.Get(x => x.Purchase_No == pur.Purchase_No).ToList();
                    foreach (var item in pur_prod)
                    {
                        Purchase_Product purchase_Product = new Purchase_Product();
                        purchase_Product.ItemName = item.ItemName;
                        purchase_Product.Quantity = item.Quantity;
                        purchase_Product.Price = item.Price;
                        purchase_Product.Total = item.Total;
                        worker.PurchaseProductEntity.Update(purchase_Product);
                        worker.Save();
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return RedirectToAction("List");
        }
        #endregion

        #region Delete Purchase
        [HttpGet]
        public ActionResult Delete(int id)
        {
            DeletePurchase(id);
            return RedirectToAction("List");
        }

        public bool DeletePurchase(int id)
        {
            var purchase = worker.PurchaseEntity.GetByID(id);
            DeletePurchaseProduct(purchase.Purchase_No);
            if (purchase != null)
            {
                worker.PurchaseEntity.Delete(purchase);
                worker.Save();
                return true;
            }
            else
                return false;
        }

        public bool DeletePurchaseProduct(string PurchaseNo)
        {
            var purchase_product = worker.PurchaseProductEntity.Get(x => x.Purchase_No == PurchaseNo).ToList();
            if (purchase_product != null)
            {
                foreach (var item in purchase_product)
                {
                    worker.PurchaseProductEntity.Delete(item);
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