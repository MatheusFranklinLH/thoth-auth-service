using Moq;
using Thoth.API.Requests;
using Thoth.Domain.Entities;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Services;

namespace Thoth.Tests.Services {
	public class OrganizationServiceTests {
		private readonly Mock<IOrganizationRepository> _repositoryMock;
		private readonly Mock<ILoggerService> _loggerMock;
		private readonly OrganizationService _organizationService;

		public OrganizationServiceTests() {
			_repositoryMock = new Mock<IOrganizationRepository>();
			_loggerMock = new Mock<ILoggerService>();
			_organizationService = new OrganizationService(_repositoryMock.Object, _loggerMock.Object);
		}

		[Fact]
		public async Task Should_Create_Organization_When_Request_Is_Valid_And_Not_Exists() {
			var request = new CreateOrganizationRequest { Name = "New Organization" };
			_repositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Organization)null);

			var result = await _organizationService.CreateOrganizationAsync(request);

			Assert.True(result);
			_repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Organization>()), Times.Once);
		}

		[Fact]
		public async Task Should_Return_False_When_Organization_Already_Exists() {
			var request = new CreateOrganizationRequest { Name = "Existing Organization" };
			_repositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(new Organization("Existing Organization"));

			var result = await _organizationService.CreateOrganizationAsync(request);

			Assert.False(result);
			Assert.Contains(request.Notifications, n => n.Key == "Name");
			_repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Organization>()), Times.Never);
		}

		[Fact]
		public async Task Should_Return_False_When_Request_Is_Invalid() {
			var request = new CreateOrganizationRequest { Name = "" };

			var result = await _organizationService.CreateOrganizationAsync(request);

			Assert.False(result);
			Assert.Contains(request.Notifications, n => n.Key == "Name");
			_repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Organization>()), Times.Never);
		}
	}
}
