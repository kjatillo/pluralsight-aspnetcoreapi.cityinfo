using Microsoft.EntityFrameworkCore;
using Pluralsight.AspNetCoreWebApi.CityInfo.Entities;

namespace Pluralsight.AspNetCoreWebApi.CityInfo.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options)
        : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterests { get; set; }
    }
}
