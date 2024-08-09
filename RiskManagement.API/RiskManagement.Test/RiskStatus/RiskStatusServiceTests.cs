using Microsoft.AspNetCore.Http;
using RiskManagement.API.RiskManagement.Services.Interfaces;

namespace RiskManagement.Test.RiskStatus;

using NUnit.Framework;
using Moq;
using RiskManagement.API.RiskManagement.Models.Risks;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;
using RiskManagement.API.RiskManagement.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

[TestFixture]
public class RiskStatusServiceTests
{
    private Mock<IRiskRepo> _mockRepo;
    private RiskService _riskService;
    private Mock<IUserService> _mockUserService;


    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IRiskRepo>();
        _riskService = new RiskService(_mockRepo.Object, null!);
        _mockUserService = new Mock<IUserService>();

    }

    [Test]
    public async Task UpdateRiskStatusService_ShouldReturnSuccess_WhenStatusIsUpdatedSuccessfully()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var updateStatusDto = new InputRiskStatusDto() { Id = 1, Status = 2 };
        var risk = new API.RiskManagement.DataProvide.Risk { Id = 1, Status = 1, ProjectId = 1 };

        _mockUserService.Setup(x => x.GetUserIdFromHttpContext(httpContext)).Returns(1);
        _mockRepo.Setup(x => x.GetRiskById(1)).ReturnsAsync(risk);
        _mockRepo.Setup(x => x.CheckProjectAccess(risk.ProjectId, 1)).ReturnsAsync(true);
        _mockRepo.Setup(x => x.UpdateRisk(It.Is<API.RiskManagement.DataProvide.Risk>(r => r.Status == 2))).Returns(Task.CompletedTask);

        // Act
        var result = await _riskService.ChangeRiskStatus(updateStatusDto);

        // Assert
        Assert.That(result.Code, Is.EqualTo(200));
    }

    [Test]
    public async Task UpdateRiskStatusService_ShouldReturnError_WhenRiskIsNotFound()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var updateStatusDto = new InputRiskStatusDto() { Id = 1, Status = 2 };

        _mockUserService.Setup(x => x.GetUserIdFromHttpContext(httpContext)).Returns(1);
        _mockRepo.Setup(x => x.GetRiskById(1)).ReturnsAsync((API.RiskManagement.DataProvide.Risk?)null);

        // Act
        var result = await _riskService.ChangeRiskStatus(updateStatusDto);

        // Assert
        Assert.That(result.Code, Is.EqualTo(404));
    }

    [Test]
    public async Task GetAllRiskStatusByProjectIdService_ShouldReturnStatuses_WhenStatusesAreFound()
    {
        // Arrange
        const int projectId = 1;
        var riskStatuses = new List<RiskStatusDto> { new RiskStatusDto { Id = 1, Status = 0 } };

        _mockRepo.Setup(x => x.GetAllRiskStatusByProjectId(projectId)).ReturnsAsync(riskStatuses);

        // Act
        var result = await _riskService.GetAllRiskStatusByProjectIdService(projectId);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Code, Is.EqualTo(200));
            Assert.That(result.Content, Is.EqualTo(riskStatuses));
        });
    }

    [Test]
    public async Task GetAllRiskStatusByProjectIdService_ShouldReturnError_WhenNoStatusesAreFound()
    {
        // Arrange
        const int projectId = 1;

        _mockRepo.Setup(x => x.GetAllRiskStatusByProjectId(projectId)).ReturnsAsync(new List<RiskStatusDto>());

        // Act
        var result = await _riskService.GetAllRiskStatusByProjectIdService(projectId);

        // Assert
        Assert.That(result.Code, Is.EqualTo(404));
        if (result.Content != null) Assert.IsEmpty(result.Content);
    }
}
