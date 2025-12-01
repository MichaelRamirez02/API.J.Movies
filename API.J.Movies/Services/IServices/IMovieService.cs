using API.J.Movies.DAL.Models.Dtos;

namespace API.J.Movies.Services.IServices
{
    public interface IMovieService
    {
        Task<ICollection<MovieDto>> GetMoviesAsync();
        Task<MovieDto> GetMovieAsync(int id);
        Task<MovieDto> CreateMovieAsync(MovieCreateUpdateDto movieDto);
        Task<MovieDto> UpdateMovieAsync(MovieCreateUpdateDto movieDto, int id);
        Task<bool> DeleteMovieAsync(int id);
    }
}