namespace RiskManagement.API.RiskManagement.Models;

public class ResponseMessage<T>
{
    public int Code { get; set; }
    public T? Content { get; set; }
}