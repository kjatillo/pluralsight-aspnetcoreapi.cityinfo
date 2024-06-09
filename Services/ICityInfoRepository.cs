using Pluralsight.AspNetCoreWebApi.CityInfo.Entities;

namespace Pluralsight.AspNetCoreWebApi.CityInfo.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<City?> GetCityAsync(int cityId, bool includePointOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointOfInterestId);
        Task AddPointOfInterestAsync(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        // Utility Methods
        Task<bool> CityExistAsync(int cityId);
        Task<bool> SaveChangesAsync();
    }
}
