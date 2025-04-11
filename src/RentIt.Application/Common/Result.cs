namespace RentIt.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public List<string> Errors { get; } = [];

    private Result(bool isSuccess, T? value, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        if (errors != null) Errors.AddRange(errors);
    }

    public static Result<T> Success(T value) => new(true, value);
    public static Result<T> Failure(params string[] errors) => new(false, default, [.. errors]);
    public static Result<T> Failure(IEnumerable<string> errors) => new(false, default, [.. errors]);
}