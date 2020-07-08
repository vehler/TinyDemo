using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TinyDemo.Common.Helpers;
using TinyDemo.Models;
using TinyDemo.Services;

namespace TinyDemo.Controllers
{
    [Route("v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController(UserService service)
        {
            userService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers()
        {
            return Ok(await userService.GetUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetUser(int id)
        {
            try
            {
                var user = await userService.GetUserAsync(id);

                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<UserViewModel>> CreateUserAsync(UserViewModel user)
        {
            var createdUser = await userService.CreateUserAsync(user);
            return CreatedAtAction("GetUser", new { id = user.Id }, createdUser);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserViewModel>> UpdateUserAsync(int id, UserViewModel user)
        {
            try
            {
                if (id != user.Id)
                {
                    return BadRequest(new { message = "ID is required" });
                }

                var updatedUser = await userService.UpdateUserAsync(id, user);

                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserViewModel>> DeleteUser(int id)
        {
            try
            {
                var deleted = await userService.RemoveUserAsync(id);

                if (deleted)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. Please try again later.");
                }
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
