using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Net;
using System.Web.Http;
using Vidly.Models;
using Vidly.Dtos;

namespace Vidly.Controllers.Api
{
    public class NewRentalsController : ApiController
    {
        private ApplicationDbContext _context;
        public NewRentalsController()
        {
            _context = new ApplicationDbContext();
        }
        //GET /api/rentals
        public IHttpActionResult GetRentals()
        {
            var rentalsDtos
        }
        //GET /api/rentals/:id
        public IHttpActionResult GetRental()
        {

        }
        //POST /api/rentals
        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            if (!ModelState.IsValid)
                return NotFound();
            foreach(int Id in newRental.MovieIds)
            {
                var rental = new Rental
                {
                    Customer = _context.Customers
                               .ToList()
                               .Single(p => p.Id == newRental.CustomerId),
                    Movie = _context.Movies.ToList().Single(p => p.Id == Id),
                    DateRented = DateTime.Now,
                    DateReturned = null
                };
                _context.Rentals.Add(rental);
                _context.SaveChanges();
            }
            
            return Ok();
        }
        //DELETE /api/rentals/:id
        [HttpDelete]
        public void DeleteRental(int id, NewRentalDto rentalDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var rentalExists = _context.Rentals.SingleOrDefault(p => p.Id == id);
            if(rentalExists == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            AutoMapper.Mapper.Map(rentalDto, rentalExists);
        }
    }
}