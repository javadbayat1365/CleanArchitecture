namespace Domain.Common.ValueObjects;

public record LogValueObject(DateTime RegisterDate, string Message, string? AdditionalMessage = null);
