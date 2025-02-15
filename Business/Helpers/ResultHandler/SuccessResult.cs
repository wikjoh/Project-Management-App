namespace Business.Helpers.ResultHandler;

public class SuccessResult : ResultHandler
{
    public SuccessResult(int statusCode)
    {
        Success = true;
        StatusCode = statusCode;
    }
}
