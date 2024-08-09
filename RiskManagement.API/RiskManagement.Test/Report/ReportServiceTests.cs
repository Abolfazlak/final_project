namespace RiskManagement.Test.Report;

using Moq;
using NUnit.Framework;
using RiskManagement.API.RiskManagement.Models;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;
using RiskManagement.API.RiskManagement.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[TestFixture]
public class ReportServiceTests
{
    private Mock<IReportRepo> _mockRepo;
    private ReportService _reportService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IReportRepo>();
        _reportService = new ReportService(_mockRepo.Object);
    }
    
    [Test]
    public async Task GetRiskMatrixById_ValidId_ReturnsRiskMatrix()
    {
        // Arrange
        const int id = 1;
        var matrix = new[,] { { 1, 2 }, { 3, 4 } };
        _mockRepo.Setup(x => x.GetRiskCountsGrid(id)).ReturnsAsync(matrix);

        // Act
        var result = await _reportService.GetRiskMatrixById(id);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Code, Is.EqualTo(200));
            Assert.That(result.Content, Is.EqualTo(matrix));
        });
    }
    
    [Test]
    public async Task GetRiskMatrixById_ExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        const int id = 1;
        _mockRepo.Setup(x => x.GetRiskCountsGrid(id)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _reportService.GetRiskMatrixById(id);

        // Assert
        Assert.That(result.Code, Is.EqualTo(500));
        if (result.Content != null) Assert.IsEmpty(result.Content);
    }
    
    [Test]
    public async Task GetOpportunityMatrixById_ValidId_ReturnsOpportunityMatrix()
    {
        // Arrange
        const int id = 1;
        var matrix = new int[,] { { 5, 6 }, { 7, 8 } };
        _mockRepo.Setup(x => x.GetOpportunityCountsGrid(id)).ReturnsAsync(matrix);

        // Act
        var result = await _reportService.GetOpportunityMatrixById(id);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Code, Is.EqualTo(200));
            Assert.That(result.Content, Is.EqualTo(matrix));
        });
    }
    
    
    [Test]
    public async Task GetRiskCountsByStatus_ValidId_ReturnsRiskCountsByStatus()
    {
        // Arrange
        var id = 1;
        var riskCounts = new Dictionary<int, int> { { 1, 10 }, { 2, 20 } };
        _mockRepo.Setup(x => x.GetRiskCountsByStatus(id)).ReturnsAsync(riskCounts);

        // Act
        var result = await _reportService.GetRiskCountsByStatus(id);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Code, Is.EqualTo(200));
            Assert.That(result.Content, Is.EqualTo(riskCounts));
        });
    }
    
    
    [Test]
    public async Task GetRiskCountsByStatus_ExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        const int id = 1;
        _mockRepo.Setup(x => x.GetRiskCountsByStatus(id)).ThrowsAsync(new Exception("Error fetching data"));

        // Act
        var result = await _reportService.GetRiskCountsByStatus(id);

        // Assert
        Assert.That(result.Code, Is.EqualTo(500));
        if (result.Content != null) Assert.That(result.Content, Is.Empty);
    }


    [Test]
    public async Task GetRiskCountsByRupPhases_ValidId_ReturnsRiskCountsByRupPhases()
    {
        // Arrange
        const int id = 1;
        var riskCounts = new Dictionary<string, int> { { "Inception", 5 }, { "Elaboration", 10 } };
        _mockRepo.Setup(x => x.GetRiskCountsByRupPhase(id)).ReturnsAsync(riskCounts);

        // Act
        var result = await _reportService.GetRiskCountsByRupPhases(id);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Code, Is.EqualTo(200));
            Assert.That(result.Content, Is.EqualTo(riskCounts));
        });
    }

    [Test]
    public async Task GetRiskCountsByRupPhases_NullResponse_ReturnsNotFound()
    {
        // Arrange
        const int id = 1;
        _mockRepo.Setup(x => x.GetRiskCountsByRupPhase(id)).ReturnsAsync((Dictionary<string, int>?)null);

        // Act
        var result = await _reportService.GetRiskCountsByRupPhases(id);

        // Assert
        Assert.That(result.Code, Is.EqualTo(404));
        if (result.Content != null) Assert.That(result.Content, Is.Empty);
    }
    
    [Test]
    public async Task GetRiskCountsByRupPhases_ExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        const int id = 1;
        _mockRepo.Setup(x => x.GetRiskCountsByRupPhase(id)).ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _reportService.GetRiskCountsByRupPhases(id);

        // Assert
        Assert.That(result.Code, Is.EqualTo(500));
        if (result.Content != null) Assert.That(result.Content, Is.Empty);
    }
}
