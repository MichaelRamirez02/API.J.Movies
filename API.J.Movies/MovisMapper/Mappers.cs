using API.J.Movies.DAL.Dtos.Category;
using API.J.Movies.DAL.Models;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.J.Movies.MoviesMapper
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreateUpdateDto>().ReverseMap();
        }
    }
}