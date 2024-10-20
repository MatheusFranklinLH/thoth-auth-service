using Moq;
using Thoth.Domain.Entities;
using Thoth.Domain.Interfaces;
using Thoth.Domain.Repositories;
using Thoth.Domain.Requests;
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

		[Fact]
		public async Task Should_Return_All_Organizations() {
			var organizations = new List<Organization>
			{
				new("Org 1"),
				new("Org 2")
			};
			_repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(organizations);

			var result = await _organizationService.GetAllOrganizationsAsync();

			Assert.Equal(2, result.Count);
			_repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
		}

		[Fact]
		public async Task Should_Update_Organization_When_Request_Is_Valid_And_Exists() {
			var request = new UpdateOrganizationRequest { Id = 1, Name = "Updated Organization" };
			var existingOrganization = new Organization("Old Organization");
			_repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingOrganization);
			_repositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Organization)null);

			var result = await _organizationService.UpdateOrganizationAsync(request);

			Assert.True(result);
			_repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Organization>()), Times.Once);
		}

		[Fact]
		public async Task Should_Return_False_When_Organization_To_Update_Does_Not_Exist() {
			var request = new UpdateOrganizationRequest { Id = 1, Name = "Updated Organization" };
			_repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Organization)null);

			var result = await _organizationService.UpdateOrganizationAsync(request);

			Assert.False(result);
			Assert.Contains(request.Notifications, n => n.Key == "Id");
			_repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Organization>()), Times.Never);
		}

		[Fact]
		public async Task Should_Return_False_When_Updating_Organization_And_Name_Is_Duplicate() {
			var request = new UpdateOrganizationRequest { Id = 1, Name = "Duplicate Organization" };
			var existingOrganization = new Organization("Old Organization");
			var duplicateOrganization = new Organization("Duplicate Organization");
			duplicateOrganization.SetId(2);
			_repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingOrganization);
			_repositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(duplicateOrganization);

			var result = await _organizationService.UpdateOrganizationAsync(request);

			Assert.False(result);
			Assert.Contains(request.Notifications, n => n.Key == "Name");
			_repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Organization>()), Times.Never);
		}

		[Fact]
		public async Task Should_Delete_Organization_When_Exists() {
			var existingOrganization = new Organization("Org to Delete");
			_repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingOrganization);

			var result = await _organizationService.DeleteOrganizationAsync(1);

			Assert.True(result);
			_repositoryMock.Verify(repo => repo.DeleteAsync(existingOrganization), Times.Once);
		}

		[Fact]
		public async Task Should_Return_False_When_Organization_To_Delete_Does_Not_Exist() {
			_repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Organization)null);

			var result = await _organizationService.DeleteOrganizationAsync(1);

			Assert.False(result);
			_repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Organization>()), Times.Never);
		}
	}
}
