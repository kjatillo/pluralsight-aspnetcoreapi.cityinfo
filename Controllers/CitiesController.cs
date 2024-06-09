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

        const int MAX_CITIES_PAGE_SIZE = 20;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCitiesAsync(string? name, string? searchQuery,
            int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > MAX_CITIES_PAGE_SIZE)
            {
                pageSize = MAX_CITIES_PAGE_SIZE;
            }

            var cities = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);
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
