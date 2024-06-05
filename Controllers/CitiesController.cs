using Microsoft.AspNetCore.Mvc;
using Pluralsight.AspNetCoreWebApi.CityInfo.Models;

namespace Pluralsight.AspNetCoreWebApi.CityInfo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  // use [Route("api/cities")] to make the route more specific, controller takes the name of the controller class
    public class CitiesController : ControllerBase
    {
        private readonly CitiesDataStore _citiesDataStore;

        public CitiesController(CitiesDataStore citiesDataStore)
        {
            _citiesDataStore = citiesDataStore;
        }

        [HttpGet]
        public ActionResult<IEnumerable<City>> GetCities()
        {
            return Ok(_citiesDataStore.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<City> GetCity(int id)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);  // Returns 200 OK
        }
    }
}
