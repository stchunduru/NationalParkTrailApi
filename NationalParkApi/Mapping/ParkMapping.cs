using AutoMapper;
using NationalParkApi.Models;
using NationalParkApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParkApi.Mapping
{
    public class ParkMapping : Profile
    {
        public ParkMapping()
        {
            CreateMap<Park, ParkDto>().ReverseMap();
            CreateMap<Trail, TrailDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap();
            CreateMap<Trail, TrailCreateDto>().ReverseMap();
        }
    }
}
