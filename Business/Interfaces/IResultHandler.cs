namespace Business.Interfaces;

public interface IResultHandler
{
    bool Success { get; }
    int StatusCode { get; }
    string? ErrorMessage { get; }
}
