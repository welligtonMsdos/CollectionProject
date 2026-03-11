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
}
