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
        public ApiResponse<Movies[]> GetAllMovies()
        {
            var apiResponse = new ApiResponse<Movies[]>();

            apiResponse.data = _MoviesDB.Movies.ToArray();
            apiResponse.succeeded = true;
            return apiResponse;
        }

        [Route("MoviesByCategory")]
        public ApiResponse<Movies[]> GetMoviesByCategory(string category)
        {
            var apiResponse = new ApiResponse<Movies[]>();
            apiResponse.succeeded = false;
            List<Movies> targetMovies = new List<Movies>();
            foreach (var movie in _MoviesDB.Movies)
            {
                if (movie.Category.Equals(category, StringComparison.CurrentCultureIgnoreCase))
                {
                    targetMovies.Add(movie);
                }
            }
            if (targetMovies.Count>0)
            {
                apiResponse.data = targetMovies.ToArray();
                apiResponse.succeeded = true;
            }
            else
            {
                apiResponse.errorMessage = $"Category {category} does not exist in the database, no results returned";
                apiResponse.errorCode = 42;
            }
            return apiResponse;
        }

        [Route("RandomMovie")]
        public ApiResponse<Movies> GetARandomMovie()
        {
            var apiResponse = new ApiResponse<Movies>();
            apiResponse.succeeded = true;

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
            apiResponse.data = randomMovie;
            return apiResponse;
        }

        [Route("RandomMovieByCategory")]
        public ApiResponse<Movies> GetRandomMovieByCategory(string category)
        {
            var apiResponse = new ApiResponse<Movies>();
            apiResponse.succeeded = false;

            List<Movies> moviesOfTheCategory = new List<Movies>();
            foreach (var movie in _MoviesDB.Movies)
            {
                if (movie.Category.Equals(category,StringComparison.CurrentCultureIgnoreCase))
                {
                    moviesOfTheCategory.Add(movie);
                }
            }

            if (moviesOfTheCategory.Count()>0)
            {
                int randomNumLimit = moviesOfTheCategory.Count();
                Random random = new Random();
                int randomNumber = random.Next(0, randomNumLimit);
                apiResponse.data = moviesOfTheCategory[randomNumber];
                apiResponse.succeeded = true;
            }
            else
            {
                apiResponse.errorMessage = $"No Movies with Category {category} exist in the database";
                apiResponse.errorCode = 42;
            }
            return apiResponse;
        }

        [Route("RandomMoviePicks")]
        public ApiResponse<List<Movies>> GetRandomMoviePicks(int quantity)
        {
            var apiResponse = new ApiResponse<List<Movies>>();
            List<Movies> moviesPicks = new List<Movies>();
            int randNumLimit = _MoviesDB.Movies.Count() + 1;

            apiResponse.succeeded = false;

            if (quantity<=_MoviesDB.Movies.Count() && quantity>0)
            {
                for (int i = 0; i < quantity + 1; i++)
                {
                    if (moviesPicks.Count < i)
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(randNumLimit);

                        foreach (var movie in _MoviesDB.Movies)
                        {
                            if (!moviesPicks.Contains(movie) && movie.ID == randomNumber)
                            {
                                moviesPicks.Add(movie);
                                break;
                            }
                            else
                            {
                                i--;
                            }
                        }
                    }
                }
                apiResponse.data = moviesPicks;
                apiResponse.succeeded = true;
            }
            else if(quantity>_MoviesDB.Movies.Count())
            {
                apiResponse.errorMessage = "This number is outside the range amount of movies in the database";
                apiResponse.errorCode = 42;
            }
            else if(quantity == null||quantity==0)
            {
                apiResponse.errorMessage = "No quantity of movie picks has been specified to return";
                apiResponse.errorCode = 43;
            }
            return apiResponse;
        }

        [Route("AllMovieCategories")]
        public ApiResponse<List<string>> GetListOfAllCategories()
        {
            var apiResponse = new ApiResponse<List<string>>();
            List<string> movieCategories = new List<string>();
            foreach (var movie in _MoviesDB.Movies)
            {
                if (!movieCategories.Contains(movie.Category))
                {
                    movieCategories.Add(movie.Category.ToString());
                }
            }
            apiResponse.data = movieCategories.ToList();
            apiResponse.succeeded = true;
            return apiResponse;
        }

        [Route("MovieDetails")]
        public ApiResponse<Movies> GetSingleMovieDetails(int ID)
        {
            var apiResponse = new ApiResponse<Movies>();
            apiResponse.succeeded = false;

            if (ID<=_MoviesDB.Movies.Count() && ID>0)
            {
                Movies targetMovie = new Movies();
                foreach (var movie in _MoviesDB.Movies)
                {
                    if (movie.ID == ID)
                    {
                        targetMovie = movie;
                    }
                }
                apiResponse.data = targetMovie;
                apiResponse.succeeded = true;
            }
            else
            {
                apiResponse.errorMessage = $"A Movie with an ID of {ID} does not exist in the database";
                apiResponse.errorCode = 42;
            }
            return apiResponse;
        }

        [Route("KeywordSearch")]
        public ApiResponse<List<Movies>> GetMoviesByKeyword(string keyword)
        {
            var apiResponse = new ApiResponse<List<Movies>>();
            apiResponse.succeeded = false;
            List<Movies> moviesWithKeyword = new List<Movies>();

            foreach (var movie in _MoviesDB.Movies)
            {
                if (movie.Title.Contains(keyword, StringComparison.CurrentCultureIgnoreCase))
                {
                    moviesWithKeyword.Add(movie);
                }
            }
            apiResponse.data = moviesWithKeyword;
            if (moviesWithKeyword.Count>0)
            {
                apiResponse.succeeded = true;
            }
            else
            {
                apiResponse.errorMessage = $"No results were located in the database with keyword {keyword}";
                apiResponse.errorCode = 42;
            }
            return apiResponse;
        }
    }
}
