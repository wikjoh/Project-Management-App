namespace Business.Interfaces;

public interface IServiceResult
{
    bool Success { get; }
    int StatusCode { get; }
    string? ErrorMessage { get; }
}
