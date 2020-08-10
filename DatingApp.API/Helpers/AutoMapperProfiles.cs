using Automapper;
namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User , UserforDetailDTO>();
            CreateMap<User , UserForListDTO>();
            
        }
        
    }
}