﻿using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.DTOs.Hotel;
using HotelListingAPI.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HotelController> _logger;
    private readonly IMapper _mapper;

    public HotelController(IMapper mapper, ILogger<HotelController> logger, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetHotels()
    {
        var hotels = await _unitOfWork.Hotels.GetAll();
        return Ok(_mapper.Map<List<HotelDTO>>(hotels));
    }

    [HttpGet("{id:int}", Name = "GetHotel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetHotel(int id)
    {
        var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id, new List<string> { "Country" });
        return Ok(_mapper.Map<HotelDTO>(hotel));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
            return BadRequest(ModelState);
        }

        var hotel = _mapper.Map<Hotel>(hotelDTO);
        await _unitOfWork.Hotels.Insert(hotel);
        await _unitOfWork.Save();
        return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);
        
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO hotelDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
            return BadRequest(ModelState);
        }

        var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
        if (hotel == null)
        {
            _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
            return BadRequest("Submitted data is invalid");
        }

        _mapper.Map(hotelDTO, hotel);
        _unitOfWork.Hotels.Update(hotel);
        await _unitOfWork.Save();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        if (id < 1)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
            return BadRequest();
        }

        var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
        if (hotel == null)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
            return BadRequest("Submitted data is invalid");
        }

        await _unitOfWork.Hotels.Delete(id);
        await _unitOfWork.Save();
        return NoContent();
    }


}