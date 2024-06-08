using AutoMapper;
using Pluralsight.AspNetCoreWebApi.CityInfo.Entities;
using Pluralsight.AspNetCoreWebApi.CityInfo.Models;

namespace Pluralsight.AspNetCoreWebApi.CityInfo.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile() 
        {
            CreateMap<City, CityDto>();
            CreateMap<City, CityWithoutPointsOfInterestDto>();
        }
    }
}
