using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static OutcomeResult Success() => new OutcomeResult(true, IsError.None);
    public static OutcomeResult Failure(IsError error) => new OutcomeResult(false, error);

       public static OutcomeResult Failure(string v) => throw new NotImplementedException();


}
