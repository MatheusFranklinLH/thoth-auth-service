using Microsoft.AspNetCore.Mvc;
using Thoth.Domain.Requests;
using Thoth.Domain.Services;

namespace Thoth.API.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class RolesController : ControllerBase {
		private readonly RoleService _roleService;

		public RolesController(RoleService roleService) {
			_roleService = roleService;
		}


		[HttpGet]
		public async Task<IActionResult> GetAll() {
			var roles = await _roleService.GetAllRolesAsync();
			return Ok(roles);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateRoleRequest request) {
			var result = await _roleService.CreateRoleAsync(request);

			if (!result)
				return BadRequest(request.Notifications);

			return Ok(new { message = "Role created successfully" });
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] UpdateRoleRequest request) {
			var result = await _roleService.UpdateRoleAsync(request);

			if (!result)
				return BadRequest(request.Notifications);

			return Ok(new { message = "Role updated successfully" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id) {
			var result = await _roleService.DeleteRoleAsync(id);

			if (!result)
				return NotFound(new { message = "Role not found" });

			return Ok(new { message = "Role deleted successfully" });
		}
	}
}
