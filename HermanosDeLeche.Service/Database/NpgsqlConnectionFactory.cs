using Npgsql;

namespace HermanosDeLeche.Service.Database;

public interface INpgsqlConnectionFactory
{
    NpgsqlConnection Create();
}

public sealed class NpgsqlConnectionFactory : INpgsqlConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public NpgsqlConnection Create() => new(_connectionString);
}
