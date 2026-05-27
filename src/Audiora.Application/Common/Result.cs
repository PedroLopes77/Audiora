namespace Audiora.Application.Common;

public class Result<T>
{
    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public string? ErrorMessage { get; private set; }
    public string? ErrorCode { get; private set; }
    public List<string> Errors { get; private set; } = new();

    private Result() { }

    public static Result<T> Ok(T data) => new()
    {
        Success = true,
        Data = data
    };

    public static Result<T> Fail(string message, string code = "ERROR") => new()
    {
        Success = false,
        ErrorMessage = message,
        ErrorCode = code
    };

    public static Result<T> Fail(List<string> errors) => new()
    {
        Success = false,
        Errors = errors,
        ErrorMessage = "Validation failed."
    };
}

public class Result
{
    public bool Success { get; private set; }
    public string? ErrorMessage { get; private set; }
    public string? ErrorCode { get; private set; }

    private Result() { }

    public static Result Ok() => new() { Success = true };

    public static Result Fail(string message, string code = "ERROR") => new()
    {
        Success = false,
        ErrorMessage = message,
        ErrorCode = code
    };
}