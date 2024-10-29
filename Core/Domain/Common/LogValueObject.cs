namespace Domain.Common;

public record LogValueObject(DateTime Register,string Message,string? AdditionalMessage = null);
