using Carter;
using Microsoft.AspNetCore.Authorization;
using RiskManagement.API.RiskManagement.Models.Risks;
using RiskManagement.API.RiskManagement.Services.Interfaces;

namespace RiskManagement.API.RiskManagement.Modules;

public class ReportModule : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/report/getReport", [Authorize] async (IReportService service, long id) =>
        {
            try
            {
                var risks = new int[][] { };
                var opportunities = new int[][] { };
                var status = new Dictionary<int, int> { };
                var rup = new Dictionary<string, int> { };
                var amount = new List<RiskAmountSummary>();
                
                var amountService = await service.GetRiskAmountSummariesAsync(id);
                if (amountService.Code == 200)
                {
                    amount = amountService.Content;
                }
                
                var risksService = await service.GetRiskMatrixById(id);
                if (risksService.Code == 200)
                {
                    risks = ConvertToJaggedArray(risksService.Content);
                }

                var oppService = await service.GetOpportunityMatrixById(id);
                if (oppService.Code == 200)
                {
                    opportunities = ConvertToJaggedArray(oppService.Content);
                }

                var statusService = await service.GetRiskCountsByStatus(id);
                if (statusService.Code == 200)
                {
                    status = statusService.Content;
                }

                var rupService = await service.GetRiskCountsByRupPhases(id);
                if (rupService.Code == 200)
                {
                    rup = rupService.Content;
                }

                return Results.Ok(new
                {
                    Risks = risks,
                    Opportunities = opportunities,
                    Status = status,
                    Rup = rup,
                    Amount = amount
                });

            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });
    }

    private static int[][] ConvertToJaggedArray(int[,] multiDimensionalArray)
    {
        int rows = multiDimensionalArray.GetLength(0);
        int cols = multiDimensionalArray.GetLength(1);
        var jaggedArray = new int[rows][];

        for (int i = 0; i < rows; i++)
        {
            jaggedArray[i] = new int[cols];
            for (int j = 0; j < cols; j++)
            {
                jaggedArray[i][j] = multiDimensionalArray[i, j];
            }
        }

        return jaggedArray;
    }
}