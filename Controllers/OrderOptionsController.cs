using Patel_DBP_A7_Project3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patel_DBP_A7_Project3.Controllers
{
    public class OrderOptionsController : Controller
    {
        //db connection
        BooksEntities context = new BooksEntities();

        // GET: OrderOptions
        /// <summary>
        /// The view displays list of OrderOptions 
        /// </summary>
        /// <param name="id">OrderOptionID</param>
        /// <param name="sortBy">0 = OrderOptionID, 1 = SalesTaxRate, 2 = FirstBookShipCharge, 3 = AdditionalBookShipCharge</param>
        /// <param name="isDesc"></param>
        /// <returns>A list of OrderOptions Objects</returns>
        public ActionResult AllOrder(string id, int sortBy = 0, bool isDesc = true)
        {
            List<OrderOption> allOrders;

            //Sort
            switch (sortBy)
            {
                case 1:
                    if (isDesc)
                        allOrders = context.OrderOptions.OrderByDescending(o => o.SalesTaxRate).ToList();
                    else
                        allOrders = context.OrderOptions.OrderBy(o => o.SalesTaxRate).ToList();
                    break;
                case 2:
                    if (isDesc)
                        allOrders = context.OrderOptions.OrderByDescending(o => o.FirstBookShipCharge).ToList();
                    else
                        allOrders = context.OrderOptions.OrderBy(o => o.FirstBookShipCharge).ToList();
                    break;
                case 3:
                    if (isDesc)
                        allOrders = context.OrderOptions.OrderByDescending(o => o.AdditionalBookShipCharge).ToList();
                    else
                        allOrders = context.OrderOptions.OrderBy(o => o.AdditionalBookShipCharge).ToList();
                    break;

                case 0:

                default:
                    if (isDesc)
                        allOrders = context.OrderOptions.OrderByDescending(o => o.OrderOptionID).ToList();
                    else
                        allOrders = context.OrderOptions.OrderBy(o => o.OrderOptionID).ToList();
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
                    allOrders = allOrders.Where(o => o.OrderOptionID == num || o.AdditionalBookShipCharge == num ||
                                                     o.SalesTaxRate == num || o.FirstBookShipCharge == num).ToList();
                }
            }

            //return view or list to allOrder.cshtml page
            return View(allOrders);
        }

        /// <summary>
        /// It is used to retrieve a OrderOptions and its view details
        /// </summary>
        /// <param name="id">OrderOptionID field</param>
        /// <returns>A OrderOptions object</returns>
        [HttpGet]
        public ActionResult AddOrUpdateOrder(string id)
        {
            OrderOption order;
            int cid;
            int.TryParse(id, out cid);

            if (cid == 0)
            {
                order = new OrderOption();
            }
            else
            {
                order = context.OrderOptions.Where(o => o.OrderOptionID == cid).FirstOrDefault();
            }

            return View(order);
        }

        /// <summary>
        /// get data/ob from AddOrUpdateOrder.cshtml page through action 
        /// </summary>
        /// <param name="order">get a new ob for insert operation</param>
        /// <returns>redirect to AllOrder.cshtml page after successful insert/update operation</returns>
        [HttpPost]
        public ActionResult AddOrUpdateOrder(OrderOption order)
        {
            try
            {
                if (context.OrderOptions.Where(o => o.OrderOptionID == order.OrderOptionID).Count() > 0)
                {
                    var upOrder = context.OrderOptions.Where(o => o.OrderOptionID == order.OrderOptionID).FirstOrDefault();

                    upOrder.SalesTaxRate = order.SalesTaxRate;
                    upOrder.FirstBookShipCharge = order.FirstBookShipCharge;
                    upOrder.AdditionalBookShipCharge = order.AdditionalBookShipCharge;

                }
                else
                    context.OrderOptions.Add(order);

                context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("AllOrder");
        }

        /// <summary>
        /// Get id from orderOption table for delete operation
        /// </summary>
        /// <param name="id">OrderOptionID</param>
        /// <returns>redirect to AllOrder.cshtml page after successful delete operation</returns>
        [HttpGet]
        public ActionResult DeleteOrder(string id)
        {
            int cid = 0;

            if (int.TryParse(id, out cid))
            {
                try
                {
                    OrderOption order = context.OrderOptions.Where(o => o.OrderOptionID == cid).FirstOrDefault();
                    context.OrderOptions.Remove(order);

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

            return RedirectToAction("AllOrder");

        }
    }
}