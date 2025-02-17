using Business.Interfaces;

namespace Business.Models.ServiceResult;

public abstract class ServiceResult : IServiceResult
{
    public bool Success { get; protected set; }

    public int StatusCode { get; protected set; }

    public string? ErrorMessage { get; protected set; }


    public static ServiceResult Ok()
    {
        return new SuccessResult(200);
    }

    public static ServiceResult BadRequest(string message)
    {
        return new ErrorResult(400, message);
    }

    public static ServiceResult NotFound(string message)
    {
        return new ErrorResult(404, message);
    }

    public static ServiceResult AlreadyExists(string message)
    {
        return new ErrorResult(409, message);
    }

    public static ServiceResult InternalServerError(string message)
    {
        return new ErrorResult(500, message);
    }
}


public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; private set; }

    public static ServiceResult<T> Ok(T? data)
    {
        return new ServiceResult<T>
        {
            Success = true,
            StatusCode = 200,
            Data = data
        };
    }

    public static ServiceResult<T> Created(T? data)
    {
        return new ServiceResult<T>
        {
            Success = true,
            StatusCode = 201,
            Data = data
        };
    }
}