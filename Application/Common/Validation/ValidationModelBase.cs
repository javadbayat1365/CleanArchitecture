using FluentValidation;

namespace Application.Common.Validation;

/// <summary>
/// Marker Validator class
/// </summary>
/// <typeparam name="TRequestModel">A reqeust model in form of command and query</typeparam>
public class ValidationModelBase<TRequestModel>:AbstractValidator<TRequestModel> where TRequestModel : class
{
}
