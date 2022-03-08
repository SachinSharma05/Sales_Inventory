﻿using Newtonsoft.Json;
using Sales_Inventory.DAL;
using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Inventory.Controllers
{
    public class SalesController : BaseController
    {
        DBWorker worker = new DBWorker();

        // GET: Sales
        #region Sale List
        public ActionResult List()
        {
            SalesViewModel model = new SalesViewModel();
            model.SaleTo = GetSaleTo();
            model.List = GetSaleList();
            return View(model);
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
        public List<SelectListItem> GetSaleTo()
        {
            var query = worker.SaleEntity.Get().ToList();

            var list = new List<SelectListItem> { new SelectListItem { Value = null, Text = "" } };
            list.AddRange(query.ToList().Select(C => new SelectListItem
            {
                Value = C.Id.ToString(),
                Text = C.Sale_To
            }));

            return list;
        }
        #endregion

        #region Create Sale
        public List<SelectListItem> GetProductTypeList()
        {
            var query = worker.ProductTypeEntity.Get().ToList().OrderBy(x => x.Product);

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
                            stockItem.TotalQuantity = stockItem.TotalQuantity - Convert.ToInt32(item.Quantity);
                            stockItem.CreatedBy = (int)System.Web.HttpContext.Current.Session["UserId"];
                            stockItem.CreatedDate = DateTime.Now.Date;
                            worker.StockEntity.Update(stockItem);
                            worker.Save();
                        }
                        else
                        {
                            Stock stock = new Stock();
                            stock.Product = item.Item;
                            stock.TotalQuantity = Convert.ToInt32(item.Quantity);
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
                    sale_Product.Quantity = (decimal)item.Quantity;
                    sale_Product.Price = (decimal)item.Price;
                    sale_Product.Total = (decimal)item.Total;
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

        [HttpPost]
        public ActionResult Update(string saleId, string saleTo, string buyerPhoneNo, string saleDate, List<string> sale_prod)
        {
            string json = sale_prod[0].ToString();
            List<Sale_Products> sale_Products = JsonConvert.DeserializeObject<List<Sale_Products>>(json);
            if(ModelState.IsValid)
            {
                try
                {
                    if(!String.IsNullOrEmpty(saleId))
                    {
                        Sale sale = worker.SaleEntity.GetByID(Convert.ToInt32(saleId));
                        sale.Sale_To = saleTo;
                        sale.Sale_To_Phone = buyerPhoneNo;
                        sale.Sale_Date = Convert.ToDateTime(saleDate);
                        worker.SaleEntity.Update(sale);
                        worker.Save();

                        var sale_products = worker.SaleProductEntity.Get(x => x.Sale_No == sale.Sale_No).ToList();

                        foreach (var item in sale_Products)
                        {
                            var itmName = sale_products.Any(x => x.Item == item.Item);
                            if (itmName)
                            {
                                var SaleNo = worker.SaleEntity.GetByID(Convert.ToInt32(saleId)).Sale_No;
                                var data = worker.SaleProductEntity.Get(x => x.Sale_No == SaleNo && x.Item == item.Item).FirstOrDefault();
                                Sale_Product sale_Product = worker.SaleProductEntity.GetByID(data.Id);
                                sale_Product.Item = item.Item;
                                sale_Product.Quantity = item.Quantity;
                                sale_Product.Price = item.Price;
                                sale_Product.Total = item.Total;
                                worker.SaleProductEntity.Update(sale_Product);
                                worker.Save();
                                continue;
                            }
                            else
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
                                continue;
                            }
                        }
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

        #region Search List
        public ActionResult SearchList(string PurchaseName, string StartDate, string EndDate)
        {
            try
            {
                List<SalesViewModel> model = new List<SalesViewModel>();
                var SDate = StartDate != "" ? Convert.ToDateTime(StartDate).Date : DateTime.Now;
                var EDate = EndDate != "" ? Convert.ToDateTime(EndDate).Date : DateTime.Now;

                if (PurchaseName != "" && StartDate != "" && EndDate != "")
                {
                    var list = worker.SaleEntity.Get(x => x.Sale_To == PurchaseName && x.Sale_Date >= SDate && x.Sale_Date <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new SalesViewModel
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
                else if (PurchaseName != "" && StartDate != "" && EndDate == "")
                {
                    var list = worker.SaleEntity.Get(x => x.Sale_To == PurchaseName && x.Sale_Date == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new SalesViewModel
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
                else if (PurchaseName != "" && EndDate != "" && StartDate == "")
                {
                    var list = worker.SaleEntity.Get(x => x.Sale_To == PurchaseName && x.Sale_Date == EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new SalesViewModel
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
                else if (StartDate != "" && EndDate != "" && PurchaseName == "")
                {
                    var list = worker.SaleEntity.Get(x => x.Sale_Date == SDate && x.Sale_Date == EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new SalesViewModel
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
                else if (PurchaseName != null && StartDate == "" && EndDate == "")
                {
                    var list = worker.SaleEntity.Get(x => x.Sale_To == PurchaseName).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new SalesViewModel
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
                else if (StartDate != "" && PurchaseName == "" && EndDate == "")
                {
                    var list = worker.SaleEntity.Get(x => x.Sale_Date == SDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new SalesViewModel
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
                else if (EndDate != "" && StartDate == "" && PurchaseName == "")
                {
                    var list = worker.SaleEntity.Get(x => x.Sale_Date <= EDate).ToList();
                    foreach (var item in list)
                    {
                        model.Add(new SalesViewModel
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

                return PartialView("_SearchList", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Print Invoice
        public ActionResult InvoicePrint(int Id)
        {
            int TotalAmount = 0;
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
                    sale_Product.Quantity = (decimal)item.Quantity;
                    sale_Product.Price = (decimal)item.Price;
                    sale_Product.Total = (decimal)item.Total;
                    TotalAmount += Convert.ToInt32(item.Total);
                    sale_Products.Add(sale_Product);
                }
                model.sale_Products = sale_Products;
                model.ProductList = GetProductTypeList();
                ViewBag.TotalAmount = TotalAmount;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}