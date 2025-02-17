namespace Business.Models.ServiceResult;

public class ErrorResult : ServiceResult
{
    public ErrorResult(int statusCode, string errorMessage)
    {
        Success = false;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}