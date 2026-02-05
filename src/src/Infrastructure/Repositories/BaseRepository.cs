using Dapper;
using Npgsql;
using System.Data;

namespace Collection10Api.src.Infrastructure.Repositories;

public abstract class BaseRepository
{
    protected readonly string ConnectionString;

    protected BaseRepository(IConfiguration config)
    {        
        ConnectionString = config.GetConnectionString("CollectionConnection")
            ?? throw new ArgumentNullException("Connection string 'CollectionConnection' is missing.");
    }
  
    protected IDbConnection CreateConnection() => new NpgsqlConnection(ConnectionString);

    protected async Task<IEnumerable<T>> GetAllAsync<T>()
    {
        using var connection = CreateConnection();

        var tableName = typeof(T).Name;
        
        var query = $@"SELECT * FROM ""{tableName}""";
        
        return await connection.QueryAsync<T>(query);
    }

    protected async Task<T?> GetByIdAsync<T>(Guid id)
    {
        using var connection = CreateConnection();

        var tableName = typeof(T).Name;

        var query = $@"SELECT * FROM ""{tableName}"" WHERE ""Guid"" = @Id";

        return await connection.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
    }
}
