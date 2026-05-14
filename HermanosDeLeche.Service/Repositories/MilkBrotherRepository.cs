using HermanosDeLeche.Domain.DTOs.Milkman;
using HermanosDeLeche.Domain.Interfaces;
using HermanosDeLeche.Service.Database;
using Npgsql;

namespace HermanosDeLeche.Service.Repositories;

public sealed class MilkBrotherRepository : IMilkBrotherRepository
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public MilkBrotherRepository(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<MilkBrotherResponse>> ListForMilkmanAsync(Guid milkmanId, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT
                m.id AS milkman_id,
                m.nombre,
                m.username,
                m.ciudad,
                m.foto_url,
                COUNT(DISTINCT i.cow_id) AS vacas_compartidas,
                COUNT(i.id) AS ingestas_en_vacas_compartidas
            FROM milkmen m
            INNER JOIN cow_milk_intakes i ON i.milkman_id = m.id
            WHERE i.cow_id IN (
                SELECT DISTINCT cow_id FROM cow_milk_intakes WHERE milkman_id = @me
            )
              AND m.id <> @me
            GROUP BY m.id, m.nombre, m.username, m.ciudad, m.foto_url
            ORDER BY vacas_compartidas DESC, ingestas_en_vacas_compartidas DESC, m.nombre ASC
            """,
            conn);
        cmd.Parameters.AddWithValue("me", milkmanId);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        var list = new List<MilkBrotherResponse>();
        while (await reader.ReadAsync(ct))
        {
            list.Add(new MilkBrotherResponse
            {
                MilkmanId = reader.GetGuid(reader.GetOrdinal("milkman_id")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Username = reader.GetString(reader.GetOrdinal("username")),
                Ciudad = reader.IsDBNull(reader.GetOrdinal("ciudad")) ? null : reader.GetString(reader.GetOrdinal("ciudad")),
                FotoUrl = reader.IsDBNull(reader.GetOrdinal("foto_url")) ? null : reader.GetString(reader.GetOrdinal("foto_url")),
                VacasCompartidas = reader.GetInt32(reader.GetOrdinal("vacas_compartidas")),
                IngestasEnVacasCompartidas = reader.GetInt64(reader.GetOrdinal("ingestas_en_vacas_compartidas"))
            });
        }

        return list;
    }
}
