using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [Route("movies")]
        //GET: Movies
        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.CanManageMovies))
                return View("List");
            else
                return View("ReadOnlyList");

            //var movies = _context.Movies.Include(m => m.Genre).ToList();
            
            //var viewModel = new AllMoviesViewModel
            //{
            //    Movies = movies
            //};

            //return View(viewModel);
        }
        [Route("movies/details/{id}")]
        public ActionResult MovieDetails(int id)
        {
            var movies = _context.Movies.Include(m => m.Genre).ToList();

            Movie movie = movies.Where(p => p.Id == id).SingleOrDefault();

            if (movie == null)
                return HttpNotFound();

            return View("MovieDetails", movie);

        }
        // GET: Movies/Random
        public ActionResult Random()
        {
            List<Movie> movies = _context.Movies.Include(m => m.Genre).ToList();
            Random rnd = new Random();
            int randomNum = rnd.Next(1, 11);
            Movie movie = movies.Where(m => m.Id == randomNum).SingleOrDefault();
            var customers = new List<Customer>
            {
                new Customer {Name = "Customer 1"},
                new Customer {Name = "Customer 2"}
            };
            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };
            return View(viewModel);
        }
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit(int id)
        {
            //find the movie by id
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);
            if(movie == null)
            {
                return HttpNotFound();
            }
            var viewModel = new MovieFormViewModel(movie)
            {
                Genres = _context.Genres.ToList()
            };
            //if movie id doesn't exist, show http not found
            //add the movie to the viewmodel and include the genres
            return View("MovieForm", viewModel);
        }
        
        [Route("movies/new")]
        [Authorize(Roles= RoleName.CanManageMovies)]
        public ActionResult New()
        {
            var genres = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel
            {
                Genres = genres
            };
            return View("MovieForm", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel(movie)
                {
                    Genres = _context.Genres.ToList()
                };
                return View("MovieForm", viewModel);
            }
            //if the movie id is 0, add the movie to the db
            if(movie.Id == 0)
            {
                                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie);
            }
            //else, find the movie being edited
            else
            {
                var editedDbMovie = _context.Movies.SingleOrDefault(m => m.Id == movie.Id);
                //map the properties back so as not to expose to security vulnerabilities
                editedDbMovie.Name = movie.Name;
                editedDbMovie.NumberInStock = movie.NumberInStock;
                editedDbMovie.ReleaseDate = movie.ReleaseDate;
                editedDbMovie.GenreId = movie.GenreId;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Movies");
        }
    }
}