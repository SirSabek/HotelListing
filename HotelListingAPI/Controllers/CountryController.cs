using AutoMapper;
using HotelListingAPI.DTOs;
using HotelListingAPI.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CountryController> _logger;
    private readonly IMapper _mapper;

    public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetCountries()
    {
        try
        {
            var countries = await _unitOfWork.Countries.GetAll();
            return Ok(_mapper.Map<List<CountryDTO>>(countries));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please Try Again Later.");
        }
    }

    [HttpGet("{id:int}", Name = "GetCountry")]
    public async Task<IActionResult> GetCountry(int id)
    {
        try
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { "Hotels" });
            return Ok(_mapper.Map<CountryDTO>(country));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountry)}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please Try Again Later.");
        }
    }
}