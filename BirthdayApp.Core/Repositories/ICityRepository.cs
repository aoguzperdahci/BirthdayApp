using BirthdayApp.Core.Entities;

namespace BirthdayApp.Core.Repositories
{
    public interface ICityRepository : IRepository<City>
    {
        Task<IEnumerable<string>> GetAllCountriesAsync();

        Task<IEnumerable<string>> GetAllCitiesOfCountryAsync(string countryName);

        ValueTask<City> GetUserCityAsync(string username);
    }
}