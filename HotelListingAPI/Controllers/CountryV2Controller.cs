using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.DTOs.Country;
using HotelListingAPI.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListingAPI.Controllers
{
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/Country")]
    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
        private readonly Context _context;

        public CountryV2Controller(Context context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            return Ok(_context.Countries);
        }

    }
}
