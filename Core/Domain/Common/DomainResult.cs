namespace Domain.Common;

public record DomainResult(bool IsSuccess,string Message)
{
    public static DomainResult None = new DomainResult(true,string.Empty);
}
