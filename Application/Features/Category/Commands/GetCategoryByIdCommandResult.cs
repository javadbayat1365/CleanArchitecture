namespace Application.Features.Category.Commands;

public record GetCategoryByIdCommandResult(Guid CategoryId,string CategoryName);