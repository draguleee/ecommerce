using ecommerce.Data.Base;
using ecommerce.Data.Services.Interfaces;
using ecommerce.Data.ViewModels;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Data.Services.Classes
{
    public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
    {
        // Inject the AppDbContext
        private readonly AppDbContext _context;
        public MoviesService(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        // Add a new movie
        public async Task AddNewMovieAsync(NewMovieVM data)
        {
            var newMovie = new Movie()
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ImageUrl = data.ImageUrl,
                CinemaId = data.CinemaId,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                MovieCategory = data.MovieCategory,
                ProducerId = data.ProducerId
            };
            await _context.Movies.AddAsync(newMovie);
            await _context.SaveChangesAsync();

            // Add the movie actors
            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = newMovie.Id,
                    ActorId = actorId
                };
                await _context.Actor_Movies.AddAsync(newActorMovie);
            }
            await _context.SaveChangesAsync();
        }

        // Get movies by ID
        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movieDetails = await _context.Movies
                .Include(c => c.Cinema)
                .Include(p => p.Producer)
                .Include(am => am.Actors_Movies).ThenInclude(a => a.Actor)
                .FirstOrDefaultAsync(n => n.Id == id);
            return movieDetails;
        }

        // Get movie dropdowns: Producers, Cinemas, Actors
        public async Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues()
        {
            var response = new NewMovieDropdownsVM()
            {
                Producers = await _context.Producers.OrderBy(n => n.FullName).ToListAsync(),
                Cinemas = await _context.Cinemas.OrderBy(n => n.Name).ToListAsync(),
                Actors = await _context.Actors.OrderBy(n => n.FullName).ToListAsync()
            };
            return response;
        }

        // Update movie by ID
        public async Task UpdateMovieAsync(NewMovieVM data)
        {
            var dbMovie = await _context.Movies.FirstOrDefaultAsync(n => n.Id == data.Id);
            if (dbMovie != null)
            {
                dbMovie.Name = data.Name;
                dbMovie.Description = data.Description;
                dbMovie.Price = data.Price;
                dbMovie.ImageUrl = data.ImageUrl;
                dbMovie.CinemaId = data.CinemaId;
                dbMovie.StartDate = data.StartDate;
                dbMovie.EndDate = data.EndDate;
                dbMovie.MovieCategory = data.MovieCategory;
                dbMovie.ProducerId = data.ProducerId;
                await _context.SaveChangesAsync();
            }

            // Remove existing actors
            var existingActorsDb = _context.Actor_Movies.Where(n => n.MovieId == data.Id).ToList();
            _context.Actor_Movies.RemoveRange(existingActorsDb);
            await _context.SaveChangesAsync();

            // Add the movie actors
            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = data.Id,
                    ActorId = actorId
                };
                await _context.Actor_Movies.AddAsync(newActorMovie);
            }
            await _context.SaveChangesAsync();
        }
    }
}
