using API.J.Movies.DAL;
using API.J.Movies.DAL.Models;
using API.J.Movies.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API.J.Movies.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateMovieAsync(Movie movie)
        {
            movie.CreatedDate = DateTime.UtcNow;
            await _context.Movies.AddAsync(movie);
            return await SaveAsync();
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await GetMovieAsync(id);//Realizo la consulta para saber si existe
            if (movie == null)
            {
                return false;//la categoria no existe
            }
            _context.Movies.Remove(movie);
            return await SaveAsync();
        }

        public async Task<Movie> GetMovieAsync(int id)
        {
            return await _context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);//lambda expressiones
            //c=>c.Id == id es como realizar un select * from Categories where Id=id
        }

        public async Task<ICollection<Movie>> GetMoviesAsync()
        {
            var movie = await _context.Movies
            .AsNoTracking()
            .OrderBy(c => c.Name)//Ascendente 
            .ToListAsync();
            return movie;
        }

        public async Task<bool> MovieExistsByIdAsync(int id)
        {
            return await _context.Movies
               .AsNoTracking()
               .AnyAsync(c => c.Id == id);
        }

        public async Task<bool> MovieExistsByNameAsync(string name)
        {
            return await _context.Movies
            .AsNoTracking()
            .AnyAsync(c => c.Name == name);
        }

        public async Task<bool> UpdateMovieAsync(Movie movie)
        {
            movie.ModifiedDate = DateTime.UtcNow;
            _context.Movies.Update(movie);
            return await SaveAsync();
        }

        private async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}