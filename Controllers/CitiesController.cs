using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pluralsight.AspNetCoreWebApi.CityInfo.Models;
using Pluralsight.AspNetCoreWebApi.CityInfo.Services;

namespace Pluralsight.AspNetCoreWebApi.CityInfo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  // use [Route("api/cities")] to make the route more specific, controller takes the name of the controller class
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(string? name, string? searchQuery)
        {
            var cities = await _cityInfoRepository.GetCitiesAsync(name, searchQuery);
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>> (cities));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCityAsync(int cityId, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(cityId, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}
