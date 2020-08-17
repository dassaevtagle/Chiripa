using AutoMapper;
using ChiripaAPI.Dtos;
using ChiripaAPI.Models;

namespace ChiripaAPI.Profiles
{
    public class HielitosProfile : Profile
    {
        public HielitosProfile()
        {
            // Source => Target
            CreateMap<Hielito, HielitoReadDto>();
            CreateMap<HielitoCreateDto, Hielito>(); 
            CreateMap<HielitoUpdateDto, Hielito>(); 
            CreateMap<Hielito, HielitoUpdateDto>(); 
        }    
    }
}