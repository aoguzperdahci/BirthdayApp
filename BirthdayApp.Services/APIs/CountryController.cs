using BirthdayApp.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayAppWeb.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICityRepository countryRepository;

        public CountryController(ICityRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        [HttpGet]
        public async Task<List<string>> Get()
        {
            var result = await countryRepository.GetAllCountriesAsync();

            return result.ToList();
        }
    }
}