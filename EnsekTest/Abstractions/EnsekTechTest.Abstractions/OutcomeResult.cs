namespace EnsekTechTest.Abstractions;

public class OutcomeResult
{
    private OutcomeResult(bool isSuccess, IsError isError)
    {
        if (isSuccess && isError != IsError.None ||
            !isSuccess && isError == IsError.None)
            throw new ArgumentException("A successful result cannot have an error", nameof(isError));

        IsSuccess = isSuccess;
        IsError = isError;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IsError IsError { get; }

    public static OutcomeResult Success() => new(true, IsError.None);
    public static OutcomeResult Failure(IsError error) => new(false, error);

    public static OutcomeResult Failure(string v) => throw new NotImplementedException();


}
