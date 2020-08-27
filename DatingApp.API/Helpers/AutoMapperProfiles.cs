using System;
using System.Linq;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Createmap<src,destination>
            CreateMap<User, UserforDetailDTO>()
                    .ForMember(dest => dest.photourl,
                            opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.isMain).Url))
                    .ForMember(dest => dest.age,
                             opt => opt.MapFrom(src => src.DateofBirth.ageFromDateofBirth()));
                    
            CreateMap<User, UserForListDTO>()
                    .ForMember(dest => dest.photourl,
                            opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.isMain).Url))
                    .ForMember(dest => dest.age,
                             opt => opt.MapFrom(src => src.DateofBirth.ageFromDateofBirth()));;
            CreateMap<Photo, PhotoForDetailedDTO>();

            CreateMap<UserForUpdateDTO , User>();

            CreateMap<Photo , PhotoForReturnDTO>();
            CreateMap<PhotoForCreationDTO ,Photo>();
            CreateMap<UserRegisterDTO , User>();

        }

    }
}