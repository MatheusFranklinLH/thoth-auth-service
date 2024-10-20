using Microsoft.AspNetCore.Mvc;
using Thoth.API.Requests;
using Thoth.Domain.Services;

namespace Thoth.API.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class PermissionsController : ControllerBase {
		private readonly PermissionService _permissionService;

		public PermissionsController(PermissionService permissionService) {
			_permissionService = permissionService;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request) {
			var result = await _permissionService.CreatePermissionAsync(request);

			if (!result)
				return BadRequest(request.Notifications);

			return Ok(new { message = "Permission created successfully" });
		}

		[HttpGet]
		public async Task<IActionResult> GetAll() {
			var permissions = await _permissionService.GetAllPermissionsAsync();
			return Ok(permissions);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] UpdatePermissionRequest request) {
			var result = await _permissionService.UpdatePermissionAsync(request);

			if (!result)
				return BadRequest(request.Notifications);

			return Ok(new { message = "Permission updated successfully" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id) {
			var result = await _permissionService.DeletePermissionAsync(id);

			if (!result)
				return NotFound(new { message = "Permission not found" });

			return Ok(new { message = "Permission deleted successfully" });
		}
	}
}
