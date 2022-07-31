using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U7L6.Models;

namespace U7L6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private MoviesDbContext _MoviesDB;

        public MoviesController(MoviesDbContext moviesDB)
        {
            _MoviesDB = moviesDB;
        }


        [Route("AllMovies")]
        public Movies[] GetAllMovies()
        {
            return _MoviesDB.Movies.ToArray();
        }

        [Route("MoviesByCategory")]
        public Movies[] GetMoviesByCategory(string category)
        {
            List<Movies> targetMovies = new List<Movies>();
            foreach (var movie in _MoviesDB.Movies)
            {
                if (movie.Category.Equals(category, StringComparison.CurrentCultureIgnoreCase))
                {
                    targetMovies.Add(movie);
                }
            }
            return targetMovies.ToArray();
        }

        [Route("RandomMovie")]
        public Movies GetARandomMovie()
        {
            Movies randomMovie = new Movies();
            int randomLimit = _MoviesDB.Movies.Count()+1;
            Random random = new Random();
            int randomNumber = random.Next(1,randomLimit);
            foreach (var movie in _MoviesDB.Movies)
            {
                if (movie.ID==randomNumber)
                {
                    randomMovie = movie;
                    break;
                }
            }
            return randomMovie;
        }

        [Route("RandomMovieByCategory")]
        public Movies GetRandomMovieByCategory(string category)
        {
            List<Movies> moviesOfTheCategory = new List<Movies>();
            foreach (var movie in _MoviesDB.Movies)
            {
                if (movie.Category.Equals(category,StringComparison.CurrentCultureIgnoreCase))
                {
                    moviesOfTheCategory.Add(movie);
                }
            }

            int randomNumLimit = moviesOfTheCategory.Count();
            Random random = new Random();
            int randomNumber = random.Next(0, randomNumLimit);
            return moviesOfTheCategory[randomNumber];
        }

        [Route("RandomMoviePicks")]

    }
}
