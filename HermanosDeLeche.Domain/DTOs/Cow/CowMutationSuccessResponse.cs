namespace HermanosDeLeche.Domain.DTOs.Cow;

public sealed class CowMutationSuccessResponse
{
    public string Message { get; set; } = string.Empty;
    public CowResponse Cow { get; set; } = null!;
}
