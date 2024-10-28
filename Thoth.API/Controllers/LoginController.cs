using Microsoft.AspNetCore.Mvc;
using Thoth.Domain.Requests;
using Thoth.Domain.Services;

namespace Thoth.API.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class LoginController : ControllerBase {
		private readonly LoginService _loginService;

		public LoginController(LoginService loginService) {
			_loginService = loginService;
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginRequest request) {
			var (success, token) = await _loginService.LoginAsync(request);

			if (!success) {
				return BadRequest(request.Notifications);
			}

			return Ok(new { token });
		}
	}
}
