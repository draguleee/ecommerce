using ecommerce.Data.Base;
using ecommerce.Data.ViewModels;
using ecommerce.Models;

namespace ecommerce.Data.Services.Interfaces
{
    public interface IMoviesService : IEntityBaseRepository<Movie>
    {
        Task<Movie> GetMovieByIdAsync(int id);
        Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues();
        Task AddNewMovieAsync(NewMovieVM data);
        Task UpdateMovieAsync(NewMovieVM data);
    }
}
