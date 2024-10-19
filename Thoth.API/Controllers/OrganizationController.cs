using Microsoft.AspNetCore.Mvc;
using Thoth.API.Requests;
using Thoth.Domain.Services;

namespace Thoth.API.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class OrganizationsController : ControllerBase {
		private readonly OrganizationService _organizationService;

		public OrganizationsController(OrganizationService organizationService) {
			_organizationService = organizationService;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateOrganizationRequest request) {
			var result = await _organizationService.CreateOrganizationAsync(request);

			if (!result)
				return BadRequest(request.Notifications);

			return Ok(new { message = "Organization created successfully" });
		}

		[HttpGet]
		public async Task<IActionResult> GetAll() {
			var organizations = await _organizationService.GetAllOrganizationsAsync();
			return Ok(organizations);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] UpdateOrganizationRequest request) {
			var result = await _organizationService.UpdateOrganizationAsync(request);

			if (!result)
				return BadRequest(request.Notifications);

			return Ok(new { message = "Organization updated successfully" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id) {
			var result = await _organizationService.DeleteOrganizationAsync(id);

			if (!result)
				return NotFound(new { message = "Organization not found" });

			return Ok(new { message = "Organization deleted successfully" });
		}
	}
}
