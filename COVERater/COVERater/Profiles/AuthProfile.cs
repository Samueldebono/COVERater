using AutoMapper;
using COVERater.API.Dto;
using COVERater.API.Models;

namespace COVERater.API.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<AuthUsers, AuthUserResultsDto>();
        }
    }
}
