namespace Business.Models.ServiceResult;

public class SuccessResult : ServiceResult
{
    public SuccessResult(int statusCode)
    {
        Success = true;
        StatusCode = statusCode;
    }
}
