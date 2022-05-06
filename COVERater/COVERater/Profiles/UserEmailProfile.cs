using COVERater.API.Models;
using AutoMapper;
using COVERater.API.Bindings;
using COVERater.API.Dto;

namespace COVERater.API.Profiles
{
    public class UserEmailProfile : Profile
    {
        public UserEmailProfile()
        {
            CreateMap<UserEmails, UserEmailDto>();
            CreateMap<CreateUserEmailBinding, UserEmails>();
        }
    }
}
