using Microsoft.AspNetCore.Mvc;
using Thoth.Domain.Requests;
using Thoth.Domain.Services;

namespace Thoth.API.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase {
		private readonly UserService _userService;

		public UsersController(UserService userService) {
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll() {
			var users = await _userService.GetAllUsersAsync();
			return Ok(users);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateUserRequest request) {
			var result = await _userService.CreateUserAsync(request);

			if (!result)
				return BadRequest(request.Notifications);

			return Ok(new { message = "User created successfully" });
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] UpdateUserRequest request) {
			var result = await _userService.UpdateUserAsync(request);

			if (!result)
				return BadRequest(request.Notifications);

			return Ok(new { message = "User updated successfully" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id) {
			var result = await _userService.DeleteUserAsync(id);

			if (!result)
				return NotFound(new { message = "User not found" });

			return Ok(new { message = "User deleted successfully" });
		}
	}
}
