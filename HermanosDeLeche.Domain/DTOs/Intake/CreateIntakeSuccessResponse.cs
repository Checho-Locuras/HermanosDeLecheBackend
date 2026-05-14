namespace HermanosDeLeche.Domain.DTOs.Intake;

public sealed class CreateIntakeSuccessResponse
{
    public string Message { get; set; } = string.Empty;
    public IntakeResponse Intake { get; set; } = null!;
}
