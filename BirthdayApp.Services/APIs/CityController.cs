using BirthdayApp.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayApp.Services.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository countryRepository;

        public CityController(ICityRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        [HttpGet]
        public async Task<List<string>> Get(string countryName)
        {
            var result = await countryRepository.GetAllCitiesOfCountryAsync(countryName);

            return result.ToList();
        }
    }
}