using HermanosDeLeche.Domain.Entities;
using HermanosDeLeche.Domain.Interfaces;
using HermanosDeLeche.Service.Database;
using Npgsql;

namespace HermanosDeLeche.Service.Repositories;

public sealed class MilkmanRepository : IMilkmanRepository
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public MilkmanRepository(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Milkman?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT id, nombre, username, email, password_hash, ciudad, foto_url, fecha_registro
            FROM milkmen
            WHERE id = @id
            """,
            conn);
        cmd.Parameters.AddWithValue("id", id);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct)) return null;
        return Map(reader);
    }

    public async Task<Milkman?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT id, nombre, username, email, password_hash, ciudad, foto_url, fecha_registro
            FROM milkmen
            WHERE lower(username) = lower(@username)
            """,
            conn);
        cmd.Parameters.AddWithValue("username", username);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct)) return null;
        return Map(reader);
    }

    public async Task<Milkman?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT id, nombre, username, email, password_hash, ciudad, foto_url, fecha_registro
            FROM milkmen
            WHERE lower(email) = lower(@email)
            """,
            conn);
        cmd.Parameters.AddWithValue("email", email);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct)) return null;
        return Map(reader);
    }

    public async Task<IReadOnlyList<Milkman>> ListAsync(CancellationToken ct = default)
    {
        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            SELECT id, nombre, username, email, password_hash, ciudad, foto_url, fecha_registro
            FROM milkmen
            ORDER BY fecha_registro DESC
            """,
            conn);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        var list = new List<Milkman>();
        while (await reader.ReadAsync(ct))
            list.Add(Map(reader));
        return list;
    }

    public async Task<Milkman> CreateAsync(Milkman milkman, CancellationToken ct = default)
    {
        milkman.Id = milkman.Id == Guid.Empty ? Guid.NewGuid() : milkman.Id;
        milkman.FechaRegistro = milkman.FechaRegistro == default ? DateTimeOffset.UtcNow : milkman.FechaRegistro;

        await using var conn = _connectionFactory.Create();
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(
            """
            INSERT INTO milkmen (id, nombre, username, email, password_hash, ciudad, foto_url, fecha_registro)
            VALUES (@id, @nombre, @username, @email, @password_hash, @ciudad, @foto_url, @fecha_registro)
            """,
            conn);
        cmd.Parameters.AddWithValue("id", milkman.Id);
        cmd.Parameters.AddWithValue("nombre", milkman.Nombre);
        cmd.Parameters.AddWithValue("username", milkman.Username);
        cmd.Parameters.AddWithValue("email", milkman.Email);
        cmd.Parameters.AddWithValue("password_hash", milkman.PasswordHash);
        cmd.Parameters.AddWithValue("ciudad", (object?)milkman.Ciudad ?? DBNull.Value);
        cmd.Parameters.AddWithValue("foto_url", (object?)milkman.FotoUrl ?? DBNull.Value);
        cmd.Parameters.AddWithValue("fecha_registro", milkman.FechaRegistro);
        await cmd.ExecuteNonQueryAsync(ct);
        return milkman;
    }

    private static Milkman Map(NpgsqlDataReader reader) => new()
    {
        Id = reader.GetGuid(reader.GetOrdinal("id")),
        Nombre = reader.GetString(reader.GetOrdinal("nombre")),
        Username = reader.GetString(reader.GetOrdinal("username")),
        Email = reader.GetString(reader.GetOrdinal("email")),
        PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
        Ciudad = reader.IsDBNull(reader.GetOrdinal("ciudad")) ? null : reader.GetString(reader.GetOrdinal("ciudad")),
        FotoUrl = reader.IsDBNull(reader.GetOrdinal("foto_url")) ? null : reader.GetString(reader.GetOrdinal("foto_url")),
        FechaRegistro = reader.GetFieldValue<DateTimeOffset>(reader.GetOrdinal("fecha_registro"))
    };
}
