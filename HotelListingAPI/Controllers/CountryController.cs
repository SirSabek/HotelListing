﻿using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.DTOs.Country;
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
    public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
    {
        try
        {
            var countries = await _unitOfWork.Countries.GetPagedList(requestParams);
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");
            return BadRequest(ModelState);
        }

        try
        {
            var country = _mapper.Map<Country>(countryDto);
            await _unitOfWork.Countries.Insert(country);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
        }
        catch (Exception e)
        {
           _logger.LogError(e, $"Something is wrong in th {nameof(CreateCountry)}");
           return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please Try Again Later.");
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDto)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
            return BadRequest(ModelState);
        }

        try
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                return BadRequest("Submitted data is invalid");
            }

            _mapper.Map(countryDto, country);
            _unitOfWork.Countries.Update(country);
            await _unitOfWork.Save();
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Something is wrong in th {nameof(UpdateCountry)}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please Try Again Later.");
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        if (id < 1)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
            return BadRequest();
        }

        try
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return BadRequest("Submitted data is invalid");
            }

            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Something is wrong in th {nameof(DeleteCountry)}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please Try Again Later.");
        }
    }
}