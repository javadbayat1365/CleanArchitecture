using Application.Common;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Application.Tests.Extensions;

public static class AppliactionTestExtensions
{
    public static void WriteLineOperationResultErrors<TResult>(this ITestOutputHelper outputHelper,OperationResult<TResult> operationResult)
    {
        foreach (var item in operationResult.ErrorMessages)
        {
            outputHelper.WriteLine($"PropertyName: {item.Key}, Message:{item.Value}");
        }
    }
}
