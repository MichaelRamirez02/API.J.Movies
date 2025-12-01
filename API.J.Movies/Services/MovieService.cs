using API.J.Movies.DAL.Models;
using API.J.Movies.DAL.Models.Dtos;
using API.J.Movies.Repository.IRepository;
using API.J.Movies.Services.IServices;
using AutoMapper;

namespace API.J.Movies.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        public MovieService(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
        public async Task<MovieDto> CreateMovieAsync(MovieCreateUpdateDto movieCreateDto)
        {

            var categoryExists = await _movieRepository.MovieExistsByNameAsync(movieCreateDto.Name);
            if (categoryExists)
            {
                throw new InvalidOperationException($"Ya existe una película con el nombre {movieCreateDto.Name}");
            }

            if (movieCreateDto.Duration <= 0)
            {
                throw new InvalidOperationException($"No es posible ingresar una duración menor o igual a 0");
            }

            var movie = _mapper.Map<Movie>(movieCreateDto);

            var movieCreated = await _movieRepository.CreateMovieAsync(movie);

            if (!movieCreated)
            {
                throw new Exception("Ocurrio un error al crear la película");
            }

            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var existingMovie = await _movieRepository.GetMovieAsync(id);
            if (existingMovie == null)
            {
                throw new InvalidOperationException($"No se encontró la pelicula con ID: {id}");
            }

            var isDeleted = await _movieRepository.DeleteMovieAsync(id);
            if (!isDeleted)
            {
                throw new Exception("Ocurrió un error al eliminar la película");
            }
            return isDeleted;
        }

        public async Task<MovieDto> GetMovieAsync(int id)
        {
            var existingMovie = await _movieRepository.GetMovieAsync(id);
            if (existingMovie == null)
            {
                throw new InvalidOperationException($"No se encontró la pelicula con ID: {id}");
            }
            return _mapper.Map<MovieDto>(existingMovie);
        }

        public async Task<ICollection<MovieDto>> GetMoviesAsync()
        {
            var movies = await _movieRepository.GetMoviesAsync();
            return _mapper.Map<ICollection<MovieDto>>(movies);
        }

        public async Task<MovieDto> UpdateMovieAsync(MovieCreateUpdateDto movieDto, int id)
        {
            var existingMovie = await _movieRepository.GetMovieAsync(id);
            if (existingMovie == null)
            {
                throw new InvalidOperationException($"No se encontró la pelicula con ID: {id}");
            }

            var nameExist = await _movieRepository.MovieExistsByNameAsync(movieDto.Name);

            if (nameExist && !existingMovie.Name.Equals(movieDto.Name, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Ya existe una categoría con el nombre de {movieDto.Name}");
            }

            if (movieDto.Duration <= 0)
            {
                throw new InvalidOperationException($"No es posible ingresar una duración menor o igual a 0");
            }

            _mapper.Map(movieDto, existingMovie);

            var updated = await _movieRepository.UpdateMovieAsync(existingMovie);

            if (!updated)
            {
                throw new Exception("Error al actualizar la categoría");
            }

            return _mapper.Map<MovieDto>(existingMovie);
        }
    }
}