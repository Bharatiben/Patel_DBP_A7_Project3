using Patel_DBP_A7_Project3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patel_DBP_A7_Project3.Controllers
{
    public class ProductsController : Controller
    {
        //db connection
        BooksEntities context = new BooksEntities();

        // GET: Products
        /// <summary>
        /// The view displays list of Products 
        /// </summary>
        /// <param name="id">ProductCode</param>
        /// <param name="sortBy">0 = ProductCode, 1 = Description, 2 = UnitPrice, 3 = OnHandQuantity</param>
        /// <param name="isDesc"></param>
        /// <returns>A list of Products Objects</returns>
        public ActionResult AllProduct(string id, int sortBy = 0, bool isDesc = true)
        {
            List<Product> allProducts;

            //Sort
            switch (sortBy)
            {
                case 1:
                    if (isDesc)
                        allProducts = context.Products.OrderByDescending(p => p.Description).ToList();
                    else
                        allProducts = context.Products.OrderBy(p => p.Description).ToList();
                    break;
                case 2:
                    if (isDesc)
                        allProducts = context.Products.OrderByDescending(p => p.UnitPrice).ToList();
                    else
                        allProducts = context.Products.OrderBy(p => p.UnitPrice).ToList();
                    break;
                case 3:
                    if (isDesc)
                        allProducts = context.Products.OrderByDescending(p => p.OnHandQuantity).ToList();
                    else
                        allProducts = context.Products.OrderBy(p => p.OnHandQuantity).ToList();
                    break;

                case 0:

                default:
                    if (isDesc)
                        allProducts = context.Products.OrderByDescending(p => p.ProductCode).ToList();
                    else
                        allProducts = context.Products.OrderBy(p => p.ProductCode).ToList();
                    break;

            }

            //Search
            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                int num = 0;

                //in case id is an integer
                if (int.TryParse(id, out num))
                {
                    allProducts = allProducts.Where(p => p.UnitPrice == num ||
                                                        p.OnHandQuantity == num).ToList();
                }
                else // if id does not parse
                {
                    allProducts = allProducts.Where(p => p.ProductCode.ToLower().Contains(id) ||
                                                       p.Description.ToLower().Contains(id)).ToList();
                }
            }

            //return view or list to allProduct.cshtml page
            return View(allProducts);
        }

        /// <summary>
        /// It is used to retrieve a Products and its view details
        /// </summary>
        /// <param name="id">ProductCode field</param>
        /// <returns>A Products object</returns>
        [HttpGet]
        public ActionResult AddOrUpdateProduct(string id)
        {
            Product product;

            if (id == "0")
            {
                product = new Product();
            }
            else
            {
                product = context.Products.Where(p => p.ProductCode == id).FirstOrDefault();
            }

            return View(product);
        }

        /// <summary>
        /// get data/ob from AddOrUpdateProduct.cshtml page through action 
        /// </summary>
        /// <param name="product">get a new ob for insert operation</param>
        /// <returns>redirect to AllProduct.cshtml page after successful insert/update operation</returns>
        [HttpPost]
        public ActionResult AddOrUpdateProduct(Product product)
        {
            try
            {
                if (context.Products.Where(p => p.ProductCode == product.ProductCode).Count() > 0)
                {
                    var upProd = context.Products.Where(p => p.ProductCode == product.ProductCode).FirstOrDefault();

                    upProd.Description = product.Description;
                    upProd.UnitPrice = product.UnitPrice;
                    upProd.OnHandQuantity = product.OnHandQuantity;

                }
                else
                    context.Products.Add(product);

                context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("AllProduct");
        }

        /// <summary>
        /// Get id from product table for delete operation
        /// </summary>
        /// <param name="id">ProductCode</param>
        /// <returns>redirect to AllProduct.cshtml page after successful delete operation</returns>
        [HttpGet]
        public ActionResult DeleteProduct(string id)
        {
            try
                {
                    Product product = context.Products.Where(p => p.ProductCode == id).FirstOrDefault();
                    context.Products.Remove(product);

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            return RedirectToAction("AllProduct");

        }
    }
}