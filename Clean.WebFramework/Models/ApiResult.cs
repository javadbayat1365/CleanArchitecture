using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Clean.WebFramework.Models;

public record ApiResult(
    bool IsSuccess,
    string Message,
    ApiResultStatusCode StatusCode)
{
    public string RequestId => Activity.Current?.TraceId.ToHexString() ?? string.Empty;
}

public record ApiResult<TResult>(
    bool IsSuccess, 
    string Message,
    ApiResultStatusCode StatusCode,
    TResult Data) 
    :ApiResult(IsSuccess, Message, StatusCode);

public enum ApiResultStatusCode
{
    [Display(Name ="Success")]
    Ok=200,
    [Display(Name = "NotFound")]
    NotFound =404,
    [Display(Name = "BadRequest")]
    BadRequest =400,
    [Display(Name = "ServerError")]
    ServerError =500,
    [Display(Name = "Dorbidden")]
    Forbidden =403,
    [Display(Name = "Unauthorized")]
    Unauthorized = 401
}