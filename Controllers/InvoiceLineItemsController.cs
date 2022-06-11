using Patel_DBP_A7_Project3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patel_DBP_A7_Project3.Controllers
{
    public class InvoiceLineItemsController : Controller
    {
        //db connection
        BooksEntities context = new BooksEntities();

        // GET: InvoiceLineItems
        /// <summary>
        /// The view displays list of InvoiceLineItems 
        /// </summary>
        /// <param name="id">InvoiceID</param>
        /// <param name="sortBy">0 = InvoiceID, 1 = ProductCode, 2 = UnitPrice, 3 = Quantity, 4 = ItemTotal</param>
        /// <param name="isDesc"></param>
        /// <returns>A list of InvoiceLineItems Objects</returns>
        public ActionResult AllInvoiceLine(string id, int sortBy = 0, bool isDesc = true)
        {
            List<InvoiceLineItem> allInvoiceLines;

            //Sort
            switch (sortBy)
            {
                case 1:
                    if (isDesc)
                        allInvoiceLines = context.InvoiceLineItems.OrderByDescending(i2 => i2.ProductCode).ToList();
                    else
                        allInvoiceLines = context.InvoiceLineItems.OrderBy(i2 => i2.ProductCode).ToList();
                    break;
                case 2:
                    if (isDesc)
                        allInvoiceLines = context.InvoiceLineItems.OrderByDescending(i2 => i2.UnitPrice).ToList();
                    else
                        allInvoiceLines = context.InvoiceLineItems.OrderBy(i2 => i2.UnitPrice).ToList();
                    break;
                case 3:
                    if (isDesc)
                        allInvoiceLines = context.InvoiceLineItems.OrderByDescending(i2 => i2.Quantity).ToList();
                    else
                        allInvoiceLines = context.InvoiceLineItems.OrderBy(i2 => i2.Quantity).ToList();
                    break;
                case 4:
                    if (isDesc)
                        allInvoiceLines = context.InvoiceLineItems.OrderByDescending(i2 => i2.ItemTotal).ToList();
                    else
                        allInvoiceLines = context.InvoiceLineItems.OrderBy(i2 => i2.ItemTotal).ToList();
                    break;

                case 0:

                default:
                    if (isDesc)
                        allInvoiceLines = context.InvoiceLineItems.OrderByDescending(i2 => i2.InvoiceID).ToList();
                    else
                        allInvoiceLines = context.InvoiceLineItems.OrderBy(i2 => i2.InvoiceID).ToList();
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
                    allInvoiceLines = allInvoiceLines.Where(i2 => i2.InvoiceID == num ||
                                                                  i2.ItemTotal == num ||
                                                                  i2.Quantity == num ||
                                                                  i2.UnitPrice == num).ToList();
                }
                else // if id does not parse
                {
                    allInvoiceLines = allInvoiceLines.Where(i2 => i2.ProductCode.ToLower().Contains(id)).ToList();
                }
            }

            //return view or list to allInvoiceLines.cshtml page
            return View(allInvoiceLines);
        }

        /// <summary>
        /// It is used to retrieve a InvoiceLineItems and its view details
        /// </summary>
        /// <param name="id">InvoiceID field</param>
        /// <returns>A InvoiceLineItems object</returns>
        [HttpGet]
        public ActionResult AddOrUpdateInvoiceLine(string id, string code)
        {
            InvoiceLineItem invoiceLine;
            int cid;
            int.TryParse(id, out cid);

            if (cid == 0 && code == "")
            {
                invoiceLine = new InvoiceLineItem();
            }
            else
            {
                invoiceLine = context.InvoiceLineItems.Where(i2 => i2.InvoiceID == cid && i2.ProductCode == code).FirstOrDefault();
            }

            //dynamic invoice & product list
            List<Invoice> invoices = context.Invoices.ToList();
            List<Product> products = context.Products.ToList();
            InvoiceLineItemDTO viewModel = new InvoiceLineItemDTO()   //shallow copy of object
            {
                invoiceLine1 = invoiceLine,
                invoice1 = invoices,
                product1 = products
            };

            //return View(invoiceLine);
            return View(viewModel);
        }

        /// <summary>
        /// get data/ob from AddOrUpdateInvoiceLine.cshtml page through action 
        /// </summary>
        /// <param name="model">get a new ob for insert operation</param>
        /// <returns>redirect to AllInvoiceLine.cshtml page after successful insert/update operation</returns>
        [HttpPost]
        public ActionResult AddOrUpdateInvoiceLine(InvoiceLineItemDTO model)
        {
            InvoiceLineItem invoiceLine = model.invoiceLine1;
            
            try
            {
                if (context.InvoiceLineItems.Where(i2 => i2.InvoiceID == invoiceLine.InvoiceID
                                                         && i2.ProductCode == invoiceLine.ProductCode).Count() > 0)
                {
                    var upInLine = context.InvoiceLineItems.Where(i2 => i2.InvoiceID == invoiceLine.InvoiceID
                                                               && i2.ProductCode == invoiceLine.ProductCode).FirstOrDefault();

                    upInLine.InvoiceID = invoiceLine.InvoiceID;
                    upInLine.ProductCode = invoiceLine.ProductCode;
                    upInLine.UnitPrice = invoiceLine.UnitPrice;
                    upInLine.ItemTotal = invoiceLine.ItemTotal;
                    upInLine.Quantity = invoiceLine.Quantity;

                }
                else
                    context.InvoiceLineItems.Add(invoiceLine);

                context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("AllInvoiceLine");
        }

        /// <summary>
        /// Get id from InvoiceLineItem table for delete operation
        /// </summary>
        /// <param name="id">InvoiceID</param>
        /// <returns>redirect to AllInvoiceLine.cshtml page after successful delete operation</returns>
        [HttpGet]
        public ActionResult DeleteInvoiceLine(string id)
        {
            int cid = 0;

            if (int.TryParse(id, out cid))
            {
                try
                {
                    InvoiceLineItem invoiceLine = context.InvoiceLineItems.Where(i2 => i2.InvoiceID == cid).FirstOrDefault();
                    context.InvoiceLineItems.Remove(invoiceLine);
                    
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else //not successfull parsing
            {
            }

            return RedirectToAction("AllInvoiceLine");
            
        }
    }
}