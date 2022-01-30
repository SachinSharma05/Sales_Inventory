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
        public ActionResult Create()
        {
            PurchaseViewModel viewModel = new PurchaseViewModel();
            viewModel.purchase_Products = new List<Purchase_Products>();
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(PurchaseViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Purchase pur = new Purchase();
                    var Id = worker.PurchaseEntity.Get(x => x.Id == x.Id).LastOrDefault();
                    pur.Purchase_No = "PR-" + Id;
                    pur.Purchase_From = model.Purchase_From;
                    pur.Purchase_By = model.Purchase_By;
                    pur.Purchase_From_Phone = model.Purchase_From_Phone;
                    pur.Purchase_By_Phone = model.Purchase_By_Phone;
                    pur.Purchase_Date = model.Purchase_Date;

                    foreach (var item in model.purchase_Products)
                    {
                        Purchase_Products purchase_Products = new Purchase_Products();
                        purchase_Products.Purchase_No = item.Purchase_No;
                        purchase_Products.Item = item.Item;
                        purchase_Products.Quantity = item.Quantity;
                        purchase_Products.Price = item.Price;
                        purchase_Products.Total = item.Total;
                        purchase_Products.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                        purchase_Products.CreatedDate = DateTime.Now.Date;
                        worker.PurchaseProductEntity.Insert(purchase_Products);
                        worker.Save();
                    }
                    pur.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                    pur.CreatedDate = DateTime.Now.Date;
                    worker.PurchaseEntity.Insert(pur);
                    worker.Save();
                }
                return RedirectToAction("List");
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }
            return RedirectToAction("List");
        }
        #endregion
    }
}