using Pluralsight.AspNetCoreWebApi.CityInfo.Entities;

namespace Pluralsight.AspNetCoreWebApi.CityInfo.Services
{
    public interface ICityInfoRepository
    {
        public Task<IEnumerable<City>> GetCitiesAsync();
        public Task<City?> GetCityAsync(int cityId, bool includePointOfInterest);
        public Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);
        public Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointOfInterestId);
        public Task AddPointOfInterestAsync(int cityId, PointOfInterest pointOfInterest);

        // Utility Methods
        Task<bool> CityExistAsync(int cityId);
        Task<bool> SaveChangesAsync();
    }
}
