using FluentValidation;

namespace Application.Common.Validation;

public interface IValidatableModel<TRequestApplicationModel> where TRequestApplicationModel : class
{
    IValidator<TRequestApplicationModel> Validate(ValidationModelBase<TRequestApplicationModel> validator);
} 
