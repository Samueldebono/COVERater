using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVERater.API.Models;
using AutoMapper;
using COVERater.API.Dto;

namespace COVERater.API.Profiles
{
    public class SubImageProfile : Profile
    {
        public SubImageProfile()
        {
            CreateMap<SubImage, SubImageDto>();
        }
    }
}
