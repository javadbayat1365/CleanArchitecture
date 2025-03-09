﻿namespace Application.Common;

public interface IOperatoinResult
{
    bool IsSuccess { get; set; }
    bool IsNotFound { get; set; }
    List<KeyValuePair<string,string>> ErrorMessages { get; set; }
}

public class OperationResult<TResult> : IOperatoinResult
{
    public TResult? Result { get; set; }
    public bool IsSuccess { get; set ; }
    public bool IsNotFound { get; set ; }
    public List<KeyValuePair<string, string>> ErrorMessages { get; set; } = new();

    public static OperationResult<TResult> SuccessResult(TResult result)
    {
        return new OperationResult<TResult> { Result = result, IsSuccess = true };
    }
    public static OperationResult<TResult> FailureResult(string propertyName,string message="")
    {
        var result = new OperationResult<TResult>()
        {
            Result = default,
            IsSuccess = false
        };
        result.ErrorMessages.Add(new KeyValuePair<string, string>(propertyName,message));
        return result;
    }

    public static OperationResult<TResult> FailureResult(List<KeyValuePair<string,string>> Failures)
    {
        return new OperationResult<TResult>()
        {
            Result = default,
            IsSuccess = false,
            ErrorMessages = Failures
        };
    }

    public static OperationResult<TResult> NotFoundResult(string propertyName, string message)
    {
        var result = new OperationResult<TResult>()
        {
            Result = default,
            IsNotFound = true
        };
        result.ErrorMessages.Add(new KeyValuePair<string, string>(propertyName, message));
        return result;
    }
}
