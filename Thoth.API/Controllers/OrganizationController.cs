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
	}
}
