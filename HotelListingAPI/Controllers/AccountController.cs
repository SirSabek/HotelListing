using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<APIUser> _userManager;
    private readonly SignInManager<APIUser> _signInManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IMapper _mapper;

    public AccountController(
            UserManager<APIUser> userManager,
            SignInManager<APIUser> signInManager,
            ILogger<AccountController> logger,
            IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
    {
        _logger.LogInformation($"Registration Attempt for {userDTO.Email}");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var user = _mapper.Map<APIUser>(userDTO);
            user.UserName = userDTO.Email;
            var result = await _userManager.CreateAsync(user, userDTO.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something Went Wrong in the {nameof(Register)}");
            return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
        }
    }
}