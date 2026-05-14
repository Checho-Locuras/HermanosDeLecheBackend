using HermanosDeLeche.Domain.Entities;
using HermanosDeLeche.Domain.Interfaces;
using HermanosDeLeche.Service.Database;
using Npgsql;

namespace HermanosDeLeche.Service.Repositories;

public sealed class CowRepository : ICowRepository
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public CowRepository(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Cow?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT id, milkman_id, nombre, foto_url, tamano, peso, color, edad, ciudad, descripcion, fecha_registro
            FROM cows
            WHERE id = @id
            """,
            conn);
        cmd.Parameters.AddWithValue("id", id);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct)) return null;
        return Map(reader);
    }

    public async Task<IReadOnlyList<Cow>> ListAsync(CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT id, milkman_id, nombre, foto_url, tamano, peso, color, edad, ciudad, descripcion, fecha_registro
            FROM cows
            ORDER BY fecha_registro DESC
            """,
            conn);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        var list = new List<Cow>();
        while (await reader.ReadAsync(ct))
            list.Add(Map(reader));
        return list;
    }

    public async Task<Cow> CreateAsync(Cow cow, CancellationToken ct = default)
    {
        cow.Id = cow.Id == Guid.Empty ? Guid.NewGuid() : cow.Id;
        cow.FechaRegistro = cow.FechaRegistro == default ? DateTimeOffset.UtcNow : cow.FechaRegistro;

        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            INSERT INTO cows (id, milkman_id, nombre, foto_url, tamano, peso, color, edad, ciudad, descripcion, fecha_registro)
            VALUES (@id, @milkman_id, @nombre, @foto_url, @tamano, @peso, @color, @edad, @ciudad, @descripcion, @fecha_registro)
            """,
            conn);
        cmd.Parameters.AddWithValue("id", cow.Id);
        cmd.Parameters.AddWithValue("milkman_id", cow.MilkmanId);
        cmd.Parameters.AddWithValue("nombre", cow.Nombre);
        cmd.Parameters.AddWithValue("foto_url", (object?)cow.FotoUrl ?? DBNull.Value);
        cmd.Parameters.AddWithValue("tamano", (object?)cow.Tamano ?? DBNull.Value);
        cmd.Parameters.AddWithValue("peso", (object?)cow.Peso ?? DBNull.Value);
        cmd.Parameters.AddWithValue("color", (object?)cow.Color ?? DBNull.Value);
        cmd.Parameters.AddWithValue("edad", (object?)cow.Edad ?? DBNull.Value);
        cmd.Parameters.AddWithValue("ciudad", (object?)cow.Ciudad ?? DBNull.Value);
        cmd.Parameters.AddWithValue("descripcion", (object?)cow.Descripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("fecha_registro", cow.FechaRegistro);
        await cmd.ExecuteNonQueryAsync(ct);
        return cow;
    }

    public async Task<bool> UpdateAsync(Cow cow, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            UPDATE cows
            SET nombre = @nombre,
                foto_url = @foto_url,
                tamano = @tamano,
                peso = @peso,
                color = @color,
                edad = @edad,
                ciudad = @ciudad,
                descripcion = @descripcion
            WHERE id = @id
            """,
            conn);
        cmd.Parameters.AddWithValue("id", cow.Id);
        cmd.Parameters.AddWithValue("nombre", cow.Nombre);
        cmd.Parameters.AddWithValue("foto_url", (object?)cow.FotoUrl ?? DBNull.Value);
        cmd.Parameters.AddWithValue("tamano", (object?)cow.Tamano ?? DBNull.Value);
        cmd.Parameters.AddWithValue("peso", (object?)cow.Peso ?? DBNull.Value);
        cmd.Parameters.AddWithValue("color", (object?)cow.Color ?? DBNull.Value);
        cmd.Parameters.AddWithValue("edad", (object?)cow.Edad ?? DBNull.Value);
        cmd.Parameters.AddWithValue("ciudad", (object?)cow.Ciudad ?? DBNull.Value);
        cmd.Parameters.AddWithValue("descripcion", (object?)cow.Descripcion ?? DBNull.Value);
        var affected = await cmd.ExecuteNonQueryAsync(ct);
        return affected > 0;
    }

    public async Task<IReadOnlyList<Cow>> ListFedByMilkmanAsync(Guid milkmanId, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT DISTINCT ON (c.id) c.id, c.milkman_id, c.nombre, c.foto_url, c.tamano, c.peso, c.color, c.edad, c.ciudad, c.descripcion, c.fecha_registro
            FROM cows c
            INNER JOIN cow_milk_intakes i ON i.cow_id = c.id AND i.milkman_id = @milkman_id
            ORDER BY c.id, c.fecha_registro DESC
            """,
            conn);
        cmd.Parameters.AddWithValue("milkman_id", milkmanId);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        var list = new List<Cow>();
        while (await reader.ReadAsync(ct))
            list.Add(Map(reader));
        return list;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand("DELETE FROM cows WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", id);
        var affected = await cmd.ExecuteNonQueryAsync(ct);
        return affected > 0;
    }

    private static Cow Map(NpgsqlDataReader reader) => new()
    {
        Id = reader.GetGuid(reader.GetOrdinal("id")),
        MilkmanId = reader.GetGuid(reader.GetOrdinal("milkman_id")),
        Nombre = reader.GetString(reader.GetOrdinal("nombre")),
        FotoUrl = reader.IsDBNull(reader.GetOrdinal("foto_url")) ? null : reader.GetString(reader.GetOrdinal("foto_url")),
        Tamano = reader.IsDBNull(reader.GetOrdinal("tamano")) ? null : reader.GetString(reader.GetOrdinal("tamano")),
        Peso = reader.IsDBNull(reader.GetOrdinal("peso")) ? null : reader.GetDecimal(reader.GetOrdinal("peso")),
        Color = reader.IsDBNull(reader.GetOrdinal("color")) ? null : reader.GetString(reader.GetOrdinal("color")),
        Edad = reader.IsDBNull(reader.GetOrdinal("edad")) ? null : reader.GetInt32(reader.GetOrdinal("edad")),
        Ciudad = reader.IsDBNull(reader.GetOrdinal("ciudad")) ? null : reader.GetString(reader.GetOrdinal("ciudad")),
        Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
        FechaRegistro = reader.GetFieldValue<DateTimeOffset>(reader.GetOrdinal("fecha_registro"))
    };
}
