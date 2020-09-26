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
            
            // We have used reverse maps for mapping 2 both the ways i.e 
            //message to messageforcreatingDTO and ice versa

            CreateMap<MessageForCreationDTO ,Message>().ReverseMap();
            CreateMap<Message,MessageToReturnDTO>()
            .ForMember(m => m.SenderPhotoUrl , opt => opt
            .MapFrom(s => s.Sender.Photos.FirstOrDefault(p => p.isMain).Url))
            .ForMember(m => m.RecepientPhotoUrl , opt => opt
            .MapFrom(s => s.Recepient.Photos.FirstOrDefault(p => p.isMain).Url));
        }

    }
}