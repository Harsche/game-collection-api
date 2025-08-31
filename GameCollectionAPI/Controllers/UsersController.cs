using GameCollectionAPI.DTOs.Users;
using GameCollectionAPI.Exceptions;
using GameCollectionAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameCollectionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0) { return BadRequest(); }

            var user = await _service.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] UserCreateDto createdUser)
        {
            if (createdUser == null) { return BadRequest(); }

            try
            {
                var user = await _service.CreateUserAsync(createdUser);
                return CreatedAtAction("GetUserById", new { id = user.Id }, user);
            }
            catch (DuplicateUsernameException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateUsername(int id, [FromBody] UpdateUsernameDto updateUsernameDto)
        {
            if (id == 0 || updateUsernameDto == null) { return BadRequest(); }

            try
            {
                bool successful = await _service.UpdateUsernameAsync(id, updateUsernameDto);
                return successful ? NoContent() : NotFound();
            }
            catch (DuplicateUsernameException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0 || id == -1) { return BadRequest(); }

            bool successful = await _service.DeleteUserAsync(id);
            return successful ? NoContent() : NotFound();
        }
    }
}
