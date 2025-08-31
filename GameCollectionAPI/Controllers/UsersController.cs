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

        /// <summary>
        /// Gets a user by its ID.
        /// </summary>
        /// <param name="id">User's ID.</param>
        /// <returns>The user if found, otherwise NotFound.</returns>
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


        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="createdUser">The user creation DTO.</param>
        /// <returns>The created user.</returns>
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

        /// <summary>
        /// Updates the username of an existing user.
        /// </summary>
        /// <param name="id">User's ID.</param>
        /// <param name="updateUsernameDto">DTO with updated username.</param>
        /// <returns>No content if successful; NotFound if user does not exist.</returns>
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

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">User's ID.</param>
        /// <returns>No content if deleted; NotFound if user does not exist.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0 || id == -1) { return BadRequest(); } // Default admin (with ID -1) can't be deleted

            bool successful = await _service.DeleteUserAsync(id);
            return successful ? NoContent() : NotFound();
        }
    }
}
