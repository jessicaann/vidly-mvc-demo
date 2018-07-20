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
        //GET /api/newrentals
        public IHttpActionResult GetRentals()
        {
            var rentals = _context.Rentals.Include(m => m.Customer).Include(m => m.Movie).ToList();
            if (rentals == null)
                return NotFound();
            //.Select(AutoMapper.Mapper.Map<Rental, NewRentalDto>);
            return Ok(rentals);
        }
        //GET /api/newrentals/:id
        public IHttpActionResult GetRental(int id)
        {
            var rental = _context.Rentals.Include(m => m.Customer).Include(m => m.Movie).ToList()
                .SingleOrDefault(m => m.Id == id);
            if (rental == null)
                return NotFound();
            return Ok(rental);
        }
        //POST /api/newrentals
        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            if (!ModelState.IsValid)
                return NotFound();
            //DEfensive programming for edge cases if your api were to be used externally
            //if there are no movies
            //if the customer is invalid
            //if one or more movies are invalid (after querying the db)
            //a movie is not available
            var customer = _context.Customers
                           .ToList()
                           .Single(p => p.Id == newRental.CustomerId);
            var movies = _context.Movies
                        .Where(m => newRental
                        .MovieIds.Contains(m.Id))
                        .ToList();

            foreach (Movie movie in movies)
            {
                if (movie.NumberAvailable == 0)
                    return BadRequest("Movie iss not available");

                movie.NumberAvailable--;

                var rental = new Rental
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now,
                    DateReturned = null
                };
                _context.Rentals.Add(rental);
                _context.SaveChanges();
            }
            return Ok();
        }
        //DELETE /api/newrentals/:id
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
            _context.Rentals.Remove(rentalExists);
            _context.SaveChanges();
        }
    }
}