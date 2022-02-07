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
                        Purchase_By = item.Purchase_By,
                        Purchase_From_Phone = item.Purchase_From_Phone,
                        Purchase_By_Phone = item.Purchase_By_Phone,
                        Purchase_Date = item.Purchase_Date
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
        public ActionResult Create(string purchaseFrom, string purchaseBy, string sellerPhoneNo, string buyerPhoneNo, string purchaseDate, List<string> purchase_Prod)
        {
            string json = purchase_Prod[0].ToString();
            string Purchase_No = "";
            List<Purchase_Products> purchase_Products = JsonConvert.DeserializeObject<List<Purchase_Products>>(json);
            try
            {
                if (ModelState.IsValid)
                {
                    Purchase pur = new Purchase();
                    pur.Purchase_From = purchaseFrom;
                    pur.Purchase_By = purchaseBy;
                    pur.Purchase_From_Phone = sellerPhoneNo;
                    pur.Purchase_By_Phone = buyerPhoneNo;
                    pur.Purchase_Date = Convert.ToDateTime(purchaseDate);
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
                        purchase_Product.Total = item.Total;
                        purchase_Product.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                        purchase_Product.CreatedDate = DateTime.Now.Date;
                        worker.PurchaseProductEntity.Insert(purchase_Product);
                        worker.Save();
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
                model.Purchase_By = pur.Purchase_By;
                model.Purchase_From_Phone = pur.Purchase_From_Phone;
                model.Purchase_By_Phone = pur.Purchase_By_Phone;
                model.Purchase_Date = pur.Purchase_Date;

                var pur_prod = worker.PurchaseProductEntity.Get(x => x.Purchase_No == pur.Purchase_No).ToList();
                foreach (var item in pur_prod)
                {
                    Purchase_Products purchase_Product = new Purchase_Products();
                    purchase_Product.Purchase_No = pur.Purchase_No;
                    purchase_Product.ItemName = item.ItemName;
                    purchase_Product.Quantity = item.Quantity;
                    purchase_Product.Price = item.Price;
                    purchase_Product.Total = item.Total;
                    purchase_Products.Add(purchase_Product);
                }
                model.purchase_Products = purchase_Products;
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
                    pur.Purchase_By = purchaseBy;
                    pur.Purchase_From_Phone = sellerPhoneNo;
                    pur.Purchase_By_Phone = buyerPhoneNo;
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