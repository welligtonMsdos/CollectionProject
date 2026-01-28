using Collection10Api.src.Domain.Entities;
using Dapper;
using Npgsql;
using System.Data;
using static Dapper.SqlMapper;

namespace Collection10Api.src.Infrastructure.Repositories.Vinils;

public class VinilDapperRepository : IVinilDapperRepository
{    
    private readonly IDbConnection _connection;

    public VinilDapperRepository(IConfiguration config)
    {       
        _connection = new NpgsqlConnection(config.GetConnectionString("CollectionConnection"));
    }

    public async Task<ICollection<Vinyl>> GetAllVinilsAsync()
    {
        var query = @"SELECT ""Id"", ""Artist"", ""Album"", ""Year"",""Photo"",""Price""
                      FROM ""Vinil""
                      WHERE ""Active"" = TRUE
                      ORDER BY ""Year""";

        var result = await _connection.QueryAsync<Vinyl> (query);

        return result.ToList();
    }

    public async Task<Vinyl> GetVinilByIdAsync(int id)
    {
        var query = @"SELECT ""Id"", ""Artist"", ""Album"", ""Year"",""Photo"",""Price""
                      FROM ""Vinil""
                      WHERE ""Active"" = TRUE AND
                            ""Id"" = @Id
                      ORDER BY ""Year""";

        var result = await _connection.QueryFirstOrDefaultAsync<Vinyl>(query, new {Id = id});

        return result ?? new Vinyl();
    }
}
