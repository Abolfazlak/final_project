namespace RiskManagement.Test.Solution;

using NUnit.Framework;
using Moq;
using RiskManagement.API.RiskManagement.Models.Risks;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;
using RiskManagement.API.RiskManagement.Services;
using RiskManagement.API.RiskManagement.Helpers;
using System.Threading.Tasks;

[TestFixture]
public class SolutionServiceTest
{
    private Mock<IRiskRepo> _mockRepo;
    private RiskService _riskService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IRiskRepo>();
        _riskService = new RiskService(_mockRepo.Object, null!);
    }

    [Test]
    public async Task AddSolution_ShouldReturnSuccess_WhenSolutionIsAddedSuccessfully()
    {
        // Arrange
        var solutionDto = new InputSolutionDto { Description = "New Solution", Amount = 100, RiskId = 1 };

        _mockRepo.Setup(x => x.AddSolution(It.IsAny<API.RiskManagement.DataProvide.Solution>())).Returns(Task.CompletedTask);

        // Act
        var result = await _riskService.AddSolution(solutionDto);

        // Assert
        Assert.AreEqual(200, result.Code);
        Assert.AreEqual("عملیات با موفقیت انجام شد", result.Content);
    }

    [Test]
    public async Task UpdateSolution_ShouldReturnSuccess_WhenSolutionIsUpdatedSuccessfully()
    {
        // Arrange
        var solutionDto = new UpdateSolutionDto { Id = 1, Description = "Updated Solution", Amount = 200 };

        _mockRepo.Setup(x => x.GetSolutionsBySolutionId(1)).ReturnsAsync(new API.RiskManagement.DataProvide.Solution { Id = 1 });
        _mockRepo.Setup(x => x.UpdateSolution(It.IsAny<API.RiskManagement.DataProvide.Solution>())).Returns(Task.CompletedTask);

        // Act
        var result = await _riskService.UpdateSolution(solutionDto);

        // Assert
        Assert.AreEqual(200, result.Code);
        Assert.AreEqual("عملیات با موفقیت انجام شد", result.Content);
    }

    [Test]
    public async Task DeleteSolution_ShouldReturnSuccess_WhenSolutionIsDeletedSuccessfully()
    {
        // Arrange
        var solutionId = 1;

        _mockRepo.Setup(x => x.GetSolutionsBySolutionId(solutionId)).ReturnsAsync(new API.RiskManagement.DataProvide.Solution { Id = solutionId });
        _mockRepo.Setup(x => x.RemoveSolution(It.IsAny<API.RiskManagement.DataProvide.Solution>())).Returns(Task.CompletedTask);

        // Act
        var result = await _riskService.DeleteSolution(solutionId);

        // Assert
        Assert.AreEqual(200, result.Code);
        Assert.AreEqual("عملیات با موفقیت انجام شد", result.Content);
    }

    [Test]
    public async Task GetSolutionById_ShouldReturnSolution_WhenSolutionIsFound()
    {
        // Arrange
        const int solutionId = 1;
        var solution = new API.RiskManagement.DataProvide.Solution { Id = solutionId, Description = "Solution", Amount = 100, RiskId = 1 };

        _mockRepo.Setup(x => x.GetSolutionsBySolutionId(solutionId)).ReturnsAsync(solution);

        // Act
        var result = await _riskService.GetSolutionById(solutionId);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Code, Is.EqualTo(200));
        });
    }

    [Test]
    public async Task GetSolutionById_ShouldReturnError_WhenSolutionIsNotFound()
    {
        // Arrange
        var solutionId = 1;

        _mockRepo.Setup(x => x.GetSolutionsBySolutionId(solutionId)).ReturnsAsync((API.RiskManagement.DataProvide.Solution?)null);

        // Act
        var result = await _riskService.GetSolutionById(solutionId);

        // Assert
        Assert.That(result.Code, Is.EqualTo(404));
    }
}
