using RiskManagement.API.RiskManagement.Models.Users;

namespace RiskManagement.Test.Project;

using Moq;
using NUnit.Framework;
using RiskManagement.API.RiskManagement.DataProvide;
using RiskManagement.API.RiskManagement.Models;
using RiskManagement.API.RiskManagement.Models.Projects;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;
using RiskManagement.API.RiskManagement.Services;
using RiskManagement.API.RiskManagement.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class ProjectServiceTests
{
    private Mock<IProjectRepo> _mockProjectRepo;
    private Mock<IUserService> _mockUserService;
    private ProjectService _projectService;

    [SetUp]
    public void Setup()
    {
        _mockProjectRepo = new Mock<IProjectRepo>();
        _mockUserService = new Mock<IUserService>();
        _projectService = new ProjectService(_mockProjectRepo.Object, _mockUserService.Object);
    }
    
    [Test]
    public async Task GetAllProjectsService_NonAdminWithNoProjects_ReturnsNotFound()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _mockUserService.Setup(x => x.GetIsAdminFromHttpContext(httpContext)).Returns(false);
        _mockUserService.Setup(x => x.GetUserIdFromHttpContext(httpContext)).Returns(1);
        _mockProjectRepo.Setup(x => x.GetAllProjectsByUserId(1)).ReturnsAsync(new List<Project>());

        // Act
        var result = await _projectService.GetAllProjectsService(httpContext);

        // Assert
        Assert.AreEqual(404, result.Code);
        if (result.Content != null) Assert.IsEmpty(result.Content);
    }
    
    [Test]
    public async Task CreateProjectService_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var createProjectDto = new CreateProjectDto { ProjectName = "New Project", Methodology = "agile", Description = "Test project" };

        _mockUserService.Setup(x => x.GetCompanyIdFromHttpContext(httpContext)).Returns(1);
        _mockProjectRepo.Setup(x => x.AddProjectToDb(It.IsAny<Project>())).Returns(Task.CompletedTask);

        // Act
        var result = await _projectService.CreateProjectService(createProjectDto, httpContext);

        // Assert
        Assert.AreEqual(200, result.Code);
        Assert.AreEqual("عملیات با موفقیت انجام شد.", result.Content);
    }
    
    [Test]
    public async Task GetProjectByIdService_ProjectFoundWithAssignee_ReturnsProjectWithAssignee()
    {
        // Arrange
        var project = new Project { Id = 1, ProjectName = "Test Project", Methodology = "Agile", Description = "Test description", AssigneeUserId = 1 };
        var userDto = new UserDto { Id = 1, UserName = "testUser" };

        _mockProjectRepo.Setup(x => x.GetProjectById(1)).ReturnsAsync(project);
        _mockUserService.Setup(x => x.GetUserByIdService(1)).ReturnsAsync(new ResponseMessage<UserDto> { Code = 200, Content = userDto });

        // Act
        var result = await _projectService.GetProjectByIdService(1);

        // Assert
        Assert.AreEqual(200, result.Code);
        Assert.AreEqual("Test Project", result.Content?.ProjectName);
        Assert.IsNotNull(result.Content?.UserDto);
        if (result.Content?.UserDto != null) Assert.AreEqual(1, result.Content.UserDto.Id);
    }

    [Test]
    public async Task DeleteProjectService_ProjectNotFound_ReturnsNotFound()
    {
        // Arrange
        _mockProjectRepo.Setup(x => x.GetProjectById(1)).ReturnsAsync((Project)null!);

        // Act
        var result = await _projectService.DeleteProjectService(1);

        // Assert
        Assert.AreEqual(404, result.Code);
        Assert.AreEqual("پروژه مورد نظر یافت نشد.", result.Content);
    }

    [Test]
    public async Task UpdateProjectService_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var updateProjectDto = new UpdateProjectDto { Id = 1, ProjectName = "Updated Project", Methodology = "agile", Description = "Updated description" };

        _mockUserService.Setup(x => x.GetCompanyIdFromHttpContext(httpContext)).Returns(1);
        _mockProjectRepo.Setup(x => x.UpdateProject(It.IsAny<Project>())).Returns(Task.CompletedTask);

        // Act
        var result = await _projectService.UpdateProjectService(updateProjectDto, httpContext);

        // Assert
        Assert.AreEqual(200, result.Code);
        Assert.AreEqual("عملیات با موفقیت انجام شد", result.Content);
    }

    [Test]
    public async Task CreateProjectService_ExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var createProjectDto = new CreateProjectDto { ProjectName = "New Project", Methodology = "agile", Description = "Test project" };

        _mockUserService.Setup(x => x.GetCompanyIdFromHttpContext(httpContext)).Returns(1);
        _mockProjectRepo.Setup(x => x.AddProjectToDb(It.IsAny<Project>())).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _projectService.CreateProjectService(createProjectDto, httpContext);

        // Assert
        Assert.AreEqual(500, result.Code);
        Assert.AreEqual("عملیات با خطا مواجه شد.Database error", result.Content);
    }

    [Test]
    public async Task GetAllProjectsService_ExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _mockUserService.Setup(x => x.GetIsAdminFromHttpContext(httpContext)).Throws(new Exception("Unexpected error"));

        // Act
        var result = await _projectService.GetAllProjectsService(httpContext);

        // Assert
        Assert.AreEqual(500, result.Code);
        if (result.Content != null) Assert.IsEmpty(result.Content);
    }


}
