namespace Business.Helpers.ResultHandler;

public class ErrorResult : ResultHandler
{
    public ErrorResult(int statusCode, string errorMessage)
    {
        Success = false;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}