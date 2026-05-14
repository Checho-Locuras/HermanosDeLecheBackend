namespace HermanosDeLeche.Domain.DTOs.Milkman;

public sealed class MilkBrotherResponse
{
    public Guid MilkmanId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Ciudad { get; set; }
    public string? FotoUrl { get; set; }
    public int VacasCompartidas { get; set; }
    public long IngestasEnVacasCompartidas { get; set; }
}
