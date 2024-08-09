namespace RiskManagement.Test.Risk;

using RiskManagement.API.RiskManagement.Models.Risks;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;
using RiskManagement.API.RiskManagement.Services;
using RiskManagement.API.RiskManagement.Services.Interfaces;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

[TestFixture]
public class RiskServiceTests
{
    private Mock<IRiskRepo> _mockRepo;
    private Mock<IUserService> _mockUserService;
    private RiskService _riskService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IRiskRepo>();
        _mockUserService = new Mock<IUserService>();
        _riskService = new RiskService(_mockRepo.Object, _mockUserService.Object);
    }

    [Test]
    public async Task AddRiskService_ShouldReturnSuccess_WhenRiskIsAddedSuccessfully()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var riskInputDto = new RiskInputDto
            { Title = "New Risk", ProjectId = 1, SecondaryRiskCategory = new SecondaryRiskCategoryDto() { Id = 1 } };

        _mockUserService.Setup(x => x.GetUserIdFromHttpContext(httpContext)).Returns(1);
        _mockRepo.Setup(x => x.CheckProjectAccess(1, 1)).ReturnsAsync(true);
        _mockRepo.Setup(x => x.AddRiskToDb(It.IsAny<API.RiskManagement.DataProvide.Risk>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _riskService.AddRiskService(httpContext, riskInputDto);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Code, Is.EqualTo(200));
            Assert.That(result.Content, Is.EqualTo("ریسک با موفقیت اضافه شد."));
        });
    }

    [Test]
    public async Task AddRiskService_ShouldReturnError_WhenUserHasNoAccessToProject()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var riskInputDto = new RiskInputDto
            { Title = "New Risk", ProjectId = 1, SecondaryRiskCategory = new SecondaryRiskCategoryDto { Id = 1 } };

        _mockUserService.Setup(x => x.GetUserIdFromHttpContext(httpContext)).Returns(1);
        _mockRepo.Setup(x => x.CheckProjectAccess(1, 1)).ReturnsAsync(false);

        // Act
        var result = await _riskService.AddRiskService(httpContext, riskInputDto);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Code, Is.EqualTo(403));
            Assert.That(result.Content, Is.EqualTo("شما به این پروژه دسترسی ندارید!"));
        });
    }

    [Test]
    public async Task UpdateRiskService_ShouldReturnSuccess_WhenRiskIsUpdatedSuccessfully()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var riskUpdateDto = new RiskUpdateDto
        {
            Id = 1, Title = "Updated Risk", ProjectId = 1,
            SecondaryRiskCategory = new SecondaryRiskCategoryDto { Id = 1 }
        };

        _mockUserService.Setup(x => x.GetUserIdFromHttpContext(httpContext)).Returns(1);
        _mockRepo.Setup(x => x.CheckProjectAccess(1, 1)).ReturnsAsync(true);
        _mockRepo.Setup(x => x.GetRiskById(1)).ReturnsAsync(new API.RiskManagement.DataProvide.Risk());
        _mockRepo.Setup(x => x.UpdateRisk(It.IsAny<API.RiskManagement.DataProvide.Risk>())).Returns(Task.CompletedTask);

        // Act
        var result = await _riskService.UpdateRiskService(httpContext, riskUpdateDto);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Code, Is.EqualTo(200));
            Assert.That(result.Content, Is.EqualTo("ریسک با موفقیت ویرایش شد."));
        });
    }

    [Test]
    public async Task RemoveRiskService_ShouldReturnSuccess_WhenRiskIsRemovedSuccessfully()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        const int riskId = 1;

        _mockUserService.Setup(x => x.GetUserIdFromHttpContext(httpContext)).Returns(1);
        _mockRepo.Setup(x => x.GetRiskById(riskId))
            .ReturnsAsync(new API.RiskManagement.DataProvide.Risk { ProjectId = 1 });
        _mockRepo.Setup(x => x.CheckProjectAccess(1, 1)).ReturnsAsync(true);
        _mockRepo.Setup(x => x.RemoveRisk(It.IsAny<API.RiskManagement.DataProvide.Risk>())).Returns(Task.CompletedTask);

        // Act
        var result = await _riskService.RemoveRiskService(httpContext, riskId);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Code, Is.EqualTo(200));
            Assert.That(result.Content, Is.EqualTo("ریسک با موفقیت حذف شد."));
        });
    }

    [Test]
    public async Task GetAllRiskByProjectId_ShouldReturnRisks_WhenRisksAreAvailable()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        const int projectId = 1;
        var riskList = new List<RiskDto>
            { new RiskDto { Id = 1, Title = "Risk 1" }, new RiskDto { Id = 2, Title = "Risk 2" } };

        _mockUserService.Setup(x => x.GetUserIdFromHttpContext(httpContext)).Returns(1);
        _mockRepo.Setup(x => x.CheckProjectAccess(1, projectId)).ReturnsAsync(true);
        _mockRepo.Setup(x => x.GetAllRiskByProjectId(projectId)).ReturnsAsync(riskList);

        // Act
        var result = await _riskService.GetAllRiskByProjectId(httpContext, projectId);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Code, Is.EqualTo(200));
            Assert.That(result.Content, Is.EqualTo(riskList));
        });
    }

    [Test]
    public async Task GetAllRiskByProjectId_ShouldReturnError_WhenNoRisksAreAvailable()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var projectId = 1;

        _mockUserService.Setup(x => x.GetUserIdFromHttpContext(httpContext)).Returns(1);
        _mockRepo.Setup(x => x.CheckProjectAccess(1, projectId)).ReturnsAsync(true);
        _mockRepo.Setup(x => x.GetAllRiskByProjectId(projectId)).ReturnsAsync(new List<RiskDto>());

        // Act
        var result = await _riskService.GetAllRiskByProjectId(httpContext, projectId);

        // Assert
        Assert.That(result.Code, Is.EqualTo(404));
        if (result.Content != null) Assert.That(result.Content, Is.Empty);
    }
}

