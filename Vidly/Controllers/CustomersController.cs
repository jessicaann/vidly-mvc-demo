using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;
        public CustomersController()
        {
            _context = new ApplicationDbContext(); //dbcontext is a disposable object so we have to dispose of it
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        
        // GET: Customers
        [Route("customers")]
        public ViewResult Index()
        {
            var viewModel = new AllCustomersViewModel();

            return View(viewModel);
        }
        [Route("customers/details/{id}")]
        public ActionResult CustomerDetails(int id)
        {
            var customers = _context.Customers.Include(c => c.MembershipType).ToList();

            Customer customer = customers.Where(p => p.Id == id).SingleOrDefault();

            if (customer == null)
                return HttpNotFound();

            return View("CustomerDetails", customer);
            
        }
        [Route("customers/new")]
        public ActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var viewModel = new CustomerFormViewModel
            {
                Customer = new Customer(),
                MembershipTypes = membershipTypes
            };
            return View("CustomerForm", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer) //mvc framework binds the request data to this
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };

                return View("CustomerForm", viewModel);
            }
            if(customer.Id == 0)
                _context.Customers.Add(customer); //create a new customer because there's no ID
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id); //find the existing customer

                //don't use TryUpdateModel - exposes security issues (all your data being accessible at once)
                //instead update each property individually
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
                //could use a library like AutoMapper to do what we've done above 
                //ie Mapper.map(customer, customerInDb)
                //could also create another Customer class that represents a data transfer object
                //which might have a small portion of the customer props, only the ones you want updated
                
            }
            _context.SaveChanges(); //this is what persists the changes to the db
            return RedirectToAction("Index", "Customers");
        }
        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if(customer == null)
            {
                return HttpNotFound();
            }
            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };
            return View("CustomerForm", viewModel);
        }
    }
}