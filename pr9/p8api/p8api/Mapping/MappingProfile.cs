using AutoMapper;
using p8api.DTOs;
using p8api.Models;

namespace p8api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Cart, CartDTO>();
        }
    }
}
