using Patel_DBP_A7_Project3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patel_DBP_A7_Project3.Controllers
{
    public class CustomersController : Controller
    {
        //db connection
        BooksEntities context = new BooksEntities();

        // GET: Customers
        /// <summary>
        /// The view displays list of Customers 
        /// </summary>
        /// <param name="id">CustomerID</param>
        /// <param name="sortBy">0 = CustomerID, 1 = Name, 2 = Address, 3 = City, 4 = State, 5 = ZipCode</param>
        /// <param name="isDesc"></param>
        /// <returns>A list of customer Objects</returns>
        public ActionResult AllCustomer(string id, int sortBy = 0, bool isDesc = true)
        {
            //store returned list to allCustomers ob of Customer table
            // List<Customer> allCustomers = context.Customers.ToList();
            List<Customer> allCustomers;

            //Sort
            switch (sortBy)
            {
                case 1:
                    if (isDesc)
                        allCustomers = context.Customers.OrderByDescending(c => c.Name).ToList();
                    else
                        allCustomers = context.Customers.OrderBy(c => c.Name).ToList();
                    break;
                case 2:
                    if (isDesc)
                        allCustomers = context.Customers.OrderByDescending(c => c.Address).ToList();
                    else
                        allCustomers = context.Customers.OrderBy(c => c.Address).ToList();
                    break;
                case 3:
                    if (isDesc)
                        allCustomers = context.Customers.OrderByDescending(c => c.City).ToList();
                    else
                        allCustomers = context.Customers.OrderBy(c => c.City).ToList();
                    break;
                case 4:
                    if (isDesc)
                        allCustomers = context.Customers.OrderByDescending(c => c.State).ToList();
                    else
                        allCustomers = context.Customers.OrderBy(c => c.State).ToList();
                    break;
                case 5:
                    if (isDesc)
                        allCustomers = context.Customers.OrderByDescending(c => c.ZipCode).ToList();
                    else
                        allCustomers = context.Customers.OrderBy(c => c.ZipCode).ToList();
                    break;

                case 0:

                default:
                    if (isDesc)
                        allCustomers = context.Customers.OrderByDescending(c => c.CustomerID).ToList();
                    else
                        allCustomers = context.Customers.OrderBy(c => c.CustomerID).ToList();
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
                    allCustomers = allCustomers.Where(c => c.CustomerID == num).ToList();
                }
                else // if id does not parse
                {
                    allCustomers = allCustomers.Where(c => c.Name.ToLower().Contains(id) ||
                                                       c.Address.ToLower().Contains(id) ||
                                                       c.City.ToLower().Contains(id) ||
                                                       c.State.ToLower().Contains(id) ||
                                                       c.ZipCode.ToLower().Contains(id)).ToList();
                }
            }

            //return view or list to allCustomers.cshtml page
            return View(allCustomers);
        }

        /// <summary>
        /// It is used to retrieve a customer and its view details
        /// </summary>
        /// <param name="id">CustomerID field</param>
        /// <returns>A customer object</returns>
        [HttpGet]
        public ActionResult AddOrUpdateCustomer(string id)
        {
            Customer customer;
            int cid;
            int.TryParse(id, out cid);

            if (cid == 0)
            {
                customer = new Customer();
            }
            else
            {
                customer = context.Customers.Where(c => c.CustomerID == cid).FirstOrDefault();
            }

            return View(customer);
        }

        /// <summary>
        /// get data/ob from AddOrUpdateCustomer.cshtml page through action 
        /// </summary>
        /// <param name="customer">get a new ob for insert operation</param>
        /// <returns>redirect to AllCustomer.cshtml page after successful insert/update operation</returns>
        [HttpPost]
        public ActionResult AddOrUpdateCustomer(Customer customer)
        {
            try
            {
                if (context.Customers.Where(c => c.CustomerID == customer.CustomerID).Count() > 0)
                {
                    //Customers already exist
                    //var upCust = context.Customers.Where(c => c.CustomerID == customer.CustomerID).ToList()[0];
                    var upCust = context.Customers.Where(c => c.CustomerID == customer.CustomerID).FirstOrDefault();

                    upCust.Name = customer.Name;
                    upCust.Address = customer.Address;
                    upCust.City = customer.City;
                    upCust.State = customer.State;
                    upCust.ZipCode = customer.ZipCode;

                }
                else
                    //add new customer ob
                    context.Customers.Add(customer);

                context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            //redirect to AllCustomer.cshtml page
            return RedirectToAction("AllCustomer");
        }

        /// <summary>
        /// Get id from customer table for delete operation
        /// </summary>
        /// <param name="id">CustomerID</param>
        /// <returns>redirect to AllCustomer.cshtml page after successful delete operation</returns>
        [HttpGet]
        public ActionResult DeleteCustomer(string id)
        {
            int cid = 0;

            if (int.TryParse(id, out cid))
            {
                try
                {
                    Customer customer = context.Customers.Where(c => c.CustomerID == cid).FirstOrDefault();
                    context.Customers.Remove(customer);
                    // customer.IsDeleted = false;//true
                    //customer.IsDeleted = true;

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                    //return Json(new
                    //{
                    //    Success = false,
                    //    CustomerID = id,
                    //    Message = ex.Message
                    //});
                }

            }
            else //not successfull parsing
            {}

            return RedirectToAction("AllCustomer");
            //return Json(new
            //{
            //    Success = true,
            //    CustomerID = id,
            //    returnUrl = "/Customers/AllCustomer"
            //}, JsonRequestBehavior.AllowGet);
        }
    }
}