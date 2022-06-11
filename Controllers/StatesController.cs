using Patel_DBP_A7_Project3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patel_DBP_A7_Project3.Controllers
{
    public class StatesController : Controller
    {
        //db connection
        BooksEntities context = new BooksEntities();

        // GET: States
        /// <summary>
        /// The view displays list of States 
        /// </summary>
        /// <param name="id">StateCode</param>
        /// <param name="sortBy">0 = StateCode, 1 = StateName</param>
        /// <param name="isDesc"></param>
        /// <returns>A list of States Objects</returns>
        public ActionResult AllState(string id, int sortBy = 0, bool isDesc = true)
        {
            List<State> allStates;

            //Sort
            switch (sortBy)
            {
                case 1:
                    if (isDesc)
                        allStates = context.States.OrderByDescending(s => s.StateName).ToList();
                    else
                        allStates = context.States.OrderBy(s => s.StateName).ToList();
                    break;

                case 0:

                default:
                    if (isDesc)
                        allStates = context.States.OrderByDescending(s => s.StateCode).ToList();
                    else
                        allStates = context.States.OrderBy(s => s.StateCode).ToList();
                    break;
            }

            //Search
            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                allStates = allStates.Where(s => s.StateCode.ToLower().Contains(id) ||
                                                       s.StateName.ToLower().Contains(id)).ToList();
            }

            //return view or list to allStates.cshtml page
            return View(allStates);
        }

        /// <summary>
        /// It is used to retrieve a States and its view details
        /// </summary>
        /// <param name="id">StateCode field</param>
        /// <returns>A States object</returns>
        [HttpGet]
        public ActionResult AddOrUpdateState(string id)
        {
            State state;

            if (id == "0")
            {
                state = new State();
            }
            else
            {
                state = context.States.Where(s => s.StateCode == id).FirstOrDefault();
            }

            return View(state);
        }

        /// <summary>
        /// get data/ob from AddOrUpdateState.cshtml page through action 
        /// </summary>
        /// <param name="state">get a new ob for insert operation</param>
        /// <returns>redirect to AllState.cshtml page after successful insert/update operation</returns>
        [HttpPost]
        public ActionResult AddOrUpdateState(State state)
        {
           try
            {
                if (context.States.Where(s => s.StateCode == state.StateCode).Count() > 0)
                {
                    var upState = context.States.Where(s => s.StateCode == state.StateCode).FirstOrDefault();

                    //upState.StateCode = state.StateCode;
                    upState.StateName = state.StateName;

                }
                else
                    context.States.Add(state);

                context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("AllState");
        }

        /// <summary>
        /// Get id from state table for delete operation
        /// </summary>
        /// <param name="id">StateCode</param>
        /// <returns>redirect to AllState.cshtml page after successful delete operation</returns>
        [HttpGet]
        public ActionResult DeleteState(string id)
        {
                try
                {
                    State state = context.States.Where(s => s.StateCode == id).FirstOrDefault();
                    context.States.Remove(state);

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            return RedirectToAction("AllState");
        }
        
    }
}