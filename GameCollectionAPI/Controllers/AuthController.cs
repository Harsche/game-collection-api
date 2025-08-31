using GameCollectionAPI.DTOs;
using GameCollectionAPI.Exceptions;
using GameCollectionAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameCollectionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] AuthDto authDto)
        {
            if (authDto == null) { return BadRequest(); }

            try
            {
                var createdUser = await _authService.RegisterUserAsync(authDto);
                return CreatedAtRoute("GetUserById", new { id = createdUser.Id }, createdUser);
            }
            catch (DuplicateUsernameException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] AuthDto authDto)
        {
            if (authDto == null) { return BadRequest(); }

            try
            {
                var token = await _authService.LoginUserAsync(authDto);
                return Ok(new { jwt = token });
            }
            catch (InvalidDataException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
