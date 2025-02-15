using Business.Interfaces;

namespace Business.Helpers.ResultHandler;

public abstract class ResultHandler : IResultHandler
{
    public bool Success { get; protected set; }

    public int StatusCode { get; protected set; }

    public string? ErrorMessage { get; protected set; }


    public static ResultHandler Ok()
    {
        return new SuccessResult(200);
    }

    public static ResultHandler BadRequest(string message)
    {
        return new ErrorResult(400, message);
    }

    public static ResultHandler NotFound(string message)
    {
        return new ErrorResult(404, message);
    }

    public static ResultHandler AlreadyExists(string message)
    {
        return new ErrorResult(409, message);
    }
}


public class Result<T> : ResultHandler
{
    public T? Data { get; private set; }

    public static Result<T> Ok(T? data)
    {
        return new Result<T>
        {
            Success = true,
            StatusCode = 200,
            Data = data
        };
    }

    public static Result<T> Created(T? data)
    {
        return new Result<T>
        {
            Success = true,
            StatusCode = 201,
            Data = data
        };
    }
}