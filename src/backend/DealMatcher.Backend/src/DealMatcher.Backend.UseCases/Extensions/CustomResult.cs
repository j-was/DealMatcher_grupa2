namespace DealMatcher.Backend.UseCases.Extensions;

public partial class CustomResult : OneOfBase<Result, RedirectResult>
{
    protected CustomResult(OneOf<Result, RedirectResult> input) : base(input) { }
    public static implicit operator CustomResult(Result result) => new(result);
    public static implicit operator CustomResult(RedirectResult redirect) => new(redirect);

    public bool IsRedirect => IsT1;
    public bool IsSuccess => IsT0 && AsT0.IsSuccess;

    public static CustomResult Success() => Result.Success();
    public static CustomResult NotFound() => Result.NotFound();
    public static CustomResult Unauthorized() => Result.Unauthorized();
    public static CustomResult Forbidden() => Result.Forbidden();
    public static CustomResult Error(string error) => Result.Error(error);
    public static CustomResult Error(ErrorList errors) => Result.Error(errors);
    public static CustomResult Invalid(List<ValidationError> errors) => Result.Invalid(errors);
    public static CustomResult Conflict(params string[] errors) => Result.Conflict(errors);
    public static CustomResult CriticalError(params string[] errors) => Result.CriticalError(errors);
    public static CustomResult Unavailable(params string[] errors) => Result.Unavailable(errors);

    public static CustomResult Redirect(string url, bool permanent = false)
        => new RedirectResult(url, permanent);

    public static CustomResult Error(params string[] errors)
    {
        if (errors.Length == 0)
            return Error("An error occurred");
        if (errors.Length == 1)
            return Error(errors[0]);
        return Error(new ErrorList(errors));
    }

}

public partial class CustomResult<T> : OneOfBase<Result<T>, RedirectResult>
{
    protected CustomResult(OneOf<Result<T>, RedirectResult> input) : base(input) { }
    public static implicit operator CustomResult<T>(Result<T> result) => new(result);
    public static implicit operator CustomResult<T>(RedirectResult redirect) => new(redirect);

    public bool IsRedirect => IsT1;
    public bool IsSuccess => IsT0 && AsT0.IsSuccess;

    public static CustomResult<T> Success(T value) => Result<T>.Success(value);
    public static CustomResult<T> NotFound() => Result<T>.NotFound();
    public static CustomResult<T> Unauthorized() => Result<T>.Unauthorized();
    public static CustomResult<T> Forbidden() => Result<T>.Forbidden();
    public static CustomResult<T> Error(string error) => Result<T>.Error(error);
    public static CustomResult<T> Error(ErrorList errors) => Result<T>.Error(errors);
    public static CustomResult<T> Invalid(List<ValidationError> validationErrors) => Result<T>.Invalid(validationErrors);
    public static CustomResult<T> Conflict(string error) => Result<T>.Conflict(error);

    public static CustomResult<T> Error(params string[] errors)
    {
        if (errors.Length == 0)
            return Error("An error occurred");
        if (errors.Length == 1)
            return Error(errors[0]);
        return Error(new ErrorList(errors));
    }

    public static CustomResult<T> Redirect(string url, bool permanent = false)
        => new RedirectResult(url, permanent);

    public T ResultValue => AsT0.Value;
}
