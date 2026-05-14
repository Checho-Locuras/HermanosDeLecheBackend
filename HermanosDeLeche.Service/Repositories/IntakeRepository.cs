using HermanosDeLeche.Domain.Entities;
using HermanosDeLeche.Domain.Interfaces;
using HermanosDeLeche.Service.Database;
using Npgsql;

namespace HermanosDeLeche.Service.Repositories;

public sealed class IntakeRepository : IIntakeRepository
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public IntakeRepository(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<CowMilkIntake> CreateAsync(CowMilkIntake intake, CancellationToken ct = default)
    {
        intake.Id = intake.Id == Guid.Empty ? Guid.NewGuid() : intake.Id;
        intake.Fecha = intake.Fecha == default ? DateTimeOffset.UtcNow : intake.Fecha;

        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            INSERT INTO cow_milk_intakes (id, cow_id, milkman_id, cantidad_litros, fecha, observaciones)
            VALUES (@id, @cow_id, @milkman_id, @cantidad_litros, @fecha, @observaciones)
            """,
            conn);
        cmd.Parameters.AddWithValue("id", intake.Id);
        cmd.Parameters.AddWithValue("cow_id", intake.CowId);
        cmd.Parameters.AddWithValue("milkman_id", intake.MilkmanId);
        cmd.Parameters.AddWithValue("cantidad_litros", intake.CantidadLitros);
        cmd.Parameters.AddWithValue("fecha", intake.Fecha);
        cmd.Parameters.AddWithValue("observaciones", (object?)intake.Observaciones ?? DBNull.Value);
        await cmd.ExecuteNonQueryAsync(ct);
        return intake;
    }

    public async Task<IReadOnlyList<CowMilkIntake>> ListAsync(CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT id, cow_id, milkman_id, cantidad_litros, fecha, observaciones
            FROM cow_milk_intakes
            ORDER BY fecha DESC
            """,
            conn);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        return await ReadList(reader, ct);
    }

    public async Task<IReadOnlyList<CowMilkIntake>> ListByCowAsync(Guid cowId, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT id, cow_id, milkman_id, cantidad_litros, fecha, observaciones
            FROM cow_milk_intakes
            WHERE cow_id = @cow_id
            ORDER BY fecha DESC
            """,
            conn);
        cmd.Parameters.AddWithValue("cow_id", cowId);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        return await ReadList(reader, ct);
    }

    public async Task<IReadOnlyList<CowMilkIntake>> ListByMilkmanAsync(Guid milkmanId, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT id, cow_id, milkman_id, cantidad_litros, fecha, observaciones
            FROM cow_milk_intakes
            WHERE milkman_id = @milkman_id
            ORDER BY fecha DESC
            """,
            conn);
        cmd.Parameters.AddWithValue("milkman_id", milkmanId);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        return await ReadList(reader, ct);
    }

    private static async Task<IReadOnlyList<CowMilkIntake>> ReadList(NpgsqlDataReader reader, CancellationToken ct)
    {
        var list = new List<CowMilkIntake>();
        while (await reader.ReadAsync(ct))
            list.Add(Map(reader));
        return list;
    }

    private static CowMilkIntake Map(NpgsqlDataReader reader) => new()
    {
        Id = reader.GetGuid(reader.GetOrdinal("id")),
        CowId = reader.GetGuid(reader.GetOrdinal("cow_id")),
        MilkmanId = reader.GetGuid(reader.GetOrdinal("milkman_id")),
        CantidadLitros = reader.GetDecimal(reader.GetOrdinal("cantidad_litros")),
        Fecha = reader.GetFieldValue<DateTimeOffset>(reader.GetOrdinal("fecha")),
        Observaciones = reader.IsDBNull(reader.GetOrdinal("observaciones")) ? null : reader.GetString(reader.GetOrdinal("observaciones"))
    };
}
