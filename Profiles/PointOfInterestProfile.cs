using AutoMapper;
using Pluralsight.AspNetCoreWebApi.CityInfo.Entities;
using Pluralsight.AspNetCoreWebApi.CityInfo.Models;

namespace Pluralsight.AspNetCoreWebApi.CityInfo.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<PointOfInterest, PointOfInterestDto>();
            CreateMap<PointOfInterestForCreationDto, PointOfInterest>();
            CreateMap<PointOfInterestForUpdateDto, PointOfInterest>();
            CreateMap<PointOfInterest, PointOfInterestForUpdateDto>();
        }
    }
}
