using HermanosDeLeche.Domain.DTOs.Stats;
using HermanosDeLeche.Domain.Interfaces;
using HermanosDeLeche.Service.Database;
using Npgsql;

namespace HermanosDeLeche.Service.Repositories;

public sealed class StatsRepository : IStatsRepository
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public StatsRepository(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<DashboardStatsResponse> GetDashboardAsync(CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT
                (SELECT COUNT(*) FROM cows) AS total_vacas,
                (SELECT COUNT(*) FROM milkmen) AS total_lecheros,
                (SELECT COALESCE(SUM(cantidad_litros), 0) FROM cow_milk_intakes) AS total_litros,
                (SELECT COUNT(*) FROM cow_milk_intakes) AS total_ingestas
            """,
            conn);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        await reader.ReadAsync(ct);
        return new DashboardStatsResponse
        {
            TotalVacas = reader.GetInt64(reader.GetOrdinal("total_vacas")),
            TotalLecheros = reader.GetInt64(reader.GetOrdinal("total_lecheros")),
            TotalLitrosConsumidos = reader.GetDecimal(reader.GetOrdinal("total_litros")),
            TotalRegistrosIngestas = reader.GetInt64(reader.GetOrdinal("total_ingestas"))
        };
    }

    public async Task<IReadOnlyList<RankedCowCountResponse>> GetTopThirstyCowsAsync(int take, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT c.id AS cow_id, c.nombre, COUNT(i.id) AS veces
            FROM cows c
            INNER JOIN cow_milk_intakes i ON i.cow_id = c.id
            GROUP BY c.id, c.nombre
            ORDER BY veces DESC, c.nombre ASC
            LIMIT @take
            """,
            conn);
        cmd.Parameters.AddWithValue("take", take);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        var list = new List<RankedCowCountResponse>();
        while (await reader.ReadAsync(ct))
        {
            list.Add(new RankedCowCountResponse
            {
                CowId = reader.GetGuid(reader.GetOrdinal("cow_id")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Veces = reader.GetInt64(reader.GetOrdinal("veces"))
            });
        }

        return list;
    }

    public async Task<IReadOnlyList<RankedCowLitersResponse>> GetTopMilkConsumptionAsync(int take, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT c.id AS cow_id, c.nombre, COALESCE(SUM(i.cantidad_litros), 0) AS total_litros
            FROM cows c
            INNER JOIN cow_milk_intakes i ON i.cow_id = c.id
            GROUP BY c.id, c.nombre
            ORDER BY total_litros DESC, c.nombre ASC
            LIMIT @take
            """,
            conn);
        cmd.Parameters.AddWithValue("take", take);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        var list = new List<RankedCowLitersResponse>();
        while (await reader.ReadAsync(ct))
        {
            list.Add(new RankedCowLitersResponse
            {
                CowId = reader.GetGuid(reader.GetOrdinal("cow_id")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                TotalLitros = reader.GetDecimal(reader.GetOrdinal("total_litros"))
            });
        }

        return list;
    }

    public async Task<IReadOnlyList<RankedMilkmanLitersResponse>> GetTopGenerousMilkmensAsync(int take, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT m.id AS milkman_id, m.nombre, m.username, COALESCE(SUM(i.cantidad_litros), 0) AS total_litros
            FROM milkmen m
            INNER JOIN cow_milk_intakes i ON i.milkman_id = m.id
            GROUP BY m.id, m.nombre, m.username
            ORDER BY total_litros DESC, m.nombre ASC
            LIMIT @take
            """,
            conn);
        cmd.Parameters.AddWithValue("take", take);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        var list = new List<RankedMilkmanLitersResponse>();
        while (await reader.ReadAsync(ct))
        {
            list.Add(new RankedMilkmanLitersResponse
            {
                MilkmanId = reader.GetGuid(reader.GetOrdinal("milkman_id")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Username = reader.GetString(reader.GetOrdinal("username")),
                TotalLitros = reader.GetDecimal(reader.GetOrdinal("total_litros"))
            });
        }

        return list;
    }
}
