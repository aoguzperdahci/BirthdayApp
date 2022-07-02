using BirthdayApp.Core.Entities;
using BirthdayApp.Core.Repositories;
using Neo4jClient;

namespace BirthdayApp.Infrastructure.Repositories
{
    public class Neo4jCityRepository : ICityRepository
    {
        private readonly IGraphClient _client;

        public Neo4jCityRepository(IGraphClient client)
        {
            _client = client;
        }

        public async Task AddAsync(City entity)
        {
            var create = _client.Cypher
                        .Create("(city:City $c)")
                        .WithParam("c", entity);

            await create.ExecuteWithoutResultsAsync();
        }

        public async ValueTask<IEnumerable<City>> GetAllAsync()
        {
            var getCities = _client.Cypher
                            .Match("(city:City)")
                            .Return(city => city.As<City>());

            var result = await getCities.ResultsAsync;

            return result;
        }

        public async Task<IEnumerable<string>> GetAllCitiesOfCountryAsync(string countryName)
        {
            var getCities = _client.Cypher
                            .Match("(city:City)")
                            .Where("city.Country = $country")
                            .WithParam("country", countryName)
                            .Return(city => city.As<City>().Name);

            var result = await getCities.ResultsAsync;

            return result;
        }

        public async Task<IEnumerable<string>> GetAllCountriesAsync()
        {
            var getCountries = _client.Cypher
                                .Match("(city:City)")
                                .ReturnDistinct(city => city.As<City>().Country);

            var result = await getCountries.ResultsAsync;

            return result;
        }

        public async ValueTask<City> GetByIdAsync(long id)
        {
            var getCity = _client.Cypher
                        .Match("(city:City)")
                        .Where("ID(city) = $cityID")
                        .WithParam("cityID", id)
                        .Return(u => u.As<City>());

            var result = await getCity.ResultsAsync;

            return result.FirstOrDefault();
        }

        public async ValueTask<City> GetUserCityAsync(string username)
        {
            var getCity = _client.Cypher
             .Match("(user:User)-[r:RESIDE_IN]->(city:City)")
             .Where("user.Username = $username")
             .WithParam("username", username)
             .Return(city => city.As<City>());

            var result = await getCity.ResultsAsync;

            return result.FirstOrDefault();
        }

        public async Task RemoveAsync(City entity)
        {
            var remove = _client.Cypher
                         .Match("(city:City)")
                         .Where("city.Name = $name AND city.Country = $country")
                         .WithParam("name", entity.Name)
                         .WithParam("country", entity.Country)
                         .DetachDelete("c");

            await remove.ExecuteWithoutResultsAsync();
        }

        public async Task UpdateAsync(City entity)
        {
            var update = _client.Cypher
                         .Match("(city:City)")
                         .Where("city.Name = $name AND city.Country = $country")
                         .Set("city = $c")
                         .WithParam("name", entity.Name)
                         .WithParam("country", entity.Country)
                         .WithParam("c", entity);

            await update.ExecuteWithoutResultsAsync();
        }
    }
}