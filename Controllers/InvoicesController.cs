using Patel_DBP_A7_Project3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patel_DBP_A7_Project3.Controllers
{
    public class InvoicesController : Controller
    {
        //db connection
        BooksEntities context = new BooksEntities();

        // GET: Invoices
        /// <summary>
        /// The view displays list of Invoices 
        /// </summary>
        /// <param name="id">InvoiceID</param>
        /// <param name="sortBy">0 = InvoiceID, 1 = CustomerID, 2 = InvoiceDate, 3 = ProductTotal, 4 = SalesTax, 5 = Shipping, 6 = InvoiceTotal</param>
        /// <param name="isDesc"></param>
        /// <returns>A list of Invoices Objects</returns>
        public ActionResult AllInvoice(string id, int sortBy = 0, bool isDesc = true)
        {
            List<Invoice> allInvoices;

            //Sort
            switch (sortBy)
            {
                case 1:
                    if (isDesc)
                        allInvoices = context.Invoices.OrderByDescending(i => i.CustomerID).ToList();
                    else
                        allInvoices = context.Invoices.OrderBy(i => i.CustomerID).ToList();
                    break;
                case 2:
                    if (isDesc)
                        allInvoices = context.Invoices.OrderByDescending(i => i.InvoiceDate).ToList();
                    else
                        allInvoices = context.Invoices.OrderBy(i => i.InvoiceDate).ToList();
                    break;
                case 3:
                    if (isDesc)
                        allInvoices = context.Invoices.OrderByDescending(i => i.ProductTotal).ToList();
                    else
                        allInvoices = context.Invoices.OrderBy(i => i.ProductTotal).ToList();
                    break;
                case 4:
                    if (isDesc)
                        allInvoices = context.Invoices.OrderByDescending(i => i.SalesTax).ToList();
                    else
                        allInvoices = context.Invoices.OrderBy(i => i.SalesTax).ToList();
                    break;
                case 5:
                    if (isDesc)
                        allInvoices = context.Invoices.OrderByDescending(i => i.Shipping).ToList();
                    else
                        allInvoices = context.Invoices.OrderBy(i => i.Shipping).ToList();
                    break;
                case 6:
                    if (isDesc)
                        allInvoices = context.Invoices.OrderByDescending(i => i.InvoiceTotal).ToList();
                    else
                        allInvoices = context.Invoices.OrderBy(i => i.InvoiceTotal).ToList();
                    break;

                case 0:

                default:
                    if (isDesc)
                        allInvoices = context.Invoices.OrderByDescending(i => i.InvoiceID).ToList();
                    else
                        allInvoices = context.Invoices.OrderBy(i => i.InvoiceID).ToList();
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
                    allInvoices = allInvoices.Where(i => i.InvoiceID == num ||
                                                         i.Shipping == num || i.CustomerID == num ||
                                                         i.ProductTotal == num ||
                                                         i.SalesTax == num || i.InvoiceTotal == num).ToList();
                }
                else // if id does not parse
                {
                    allInvoices = allInvoices.Where(i => i.InvoiceDate.ToString().Contains(id)).ToList();
                }
            }

            //return view or list to allInvoice.cshtml page
            return View(allInvoices);
        }

        /// <summary>
        /// It is used to retrieve a Invoices and its view details
        /// </summary>
        /// <param name="id">InvoiceID field</param>
        /// <returns>A Invoices object</returns>
        [HttpGet]
        public ActionResult AddOrUpdateInvoice(string id)
        {
            Invoice invoice;
            int cid;
            int.TryParse(id, out cid);

            if (cid == 0)
            {
                invoice = new Invoice();
            }
            else
            {
                invoice = context.Invoices.Where(i => i.InvoiceID == cid).FirstOrDefault();
            }

            //dynamic customer list
            List<Customer> customers = context.Customers.ToList();
            InvoiceDTO viewModel = new InvoiceDTO()   //shallow copy of object
            {
                invoice1 = invoice,
                customer1 = customers
            };

            //return View(invoice);
            return View(viewModel);
        }

        /// <summary>
        /// get data/ob from AddOrUpdateInvoice.cshtml page through action 
        /// </summary>
        /// <param name="model">get a new ob for insert operation</param>
        /// <returns>redirect to AllInvoice.cshtml page after successful insert/update operation</returns>
        [HttpPost]
        public ActionResult AddOrUpdateInvoice(InvoiceDTO model)
        {
            Invoice invoice = model.invoice1;
            
            try
            {
                if (context.Invoices.Where(i => i.InvoiceID == invoice.InvoiceID).Count() > 0)
                {
                    var upInvoice = context.Invoices.Where(i => i.InvoiceID == invoice.InvoiceID).FirstOrDefault();

                    upInvoice.CustomerID = invoice.CustomerID;
                    upInvoice.InvoiceDate = invoice.InvoiceDate;
                    upInvoice.ProductTotal = invoice.ProductTotal;
                    upInvoice.SalesTax = invoice.SalesTax;
                    upInvoice.Shipping = invoice.Shipping;
                    upInvoice.InvoiceTotal = invoice.InvoiceTotal;

                }
                else
                    context.Invoices.Add(invoice);

                context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("AllInvoice");
        }

        /// <summary>
        /// Get id from invoice table for delete operation
        /// </summary>
        /// <param name="id">InvoiceID</param>
        /// <returns>redirect to AllInvoice.cshtml page after successful delete operation</returns>
        [HttpGet]
        public ActionResult DeleteInvoice(string id)
        {
            int cid = 0;

            if (int.TryParse(id, out cid))
            {
                try
                {
                    Invoice invoice = context.Invoices.Where(i => i.InvoiceID == cid).FirstOrDefault();
                    context.Invoices.Remove(invoice);

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

            return RedirectToAction("AllInvoice");

        }
    }
}