using Collection10Api.src.Domain.Entities;
using Dapper;
using Npgsql;
using System.Data;
using static Dapper.SqlMapper;

namespace Collection10Api.src.Infrastructure.Repositories.VinylRepo;

public class VinylDapperRepository : IVinylDapperRepository
{    
    private readonly IDbConnection _connection;

    public VinylDapperRepository(IConfiguration config)
    {       
        _connection = new NpgsqlConnection(config.GetConnectionString("CollectionConnection"));
    }

    public async Task<ICollection<Vinyl>> GetAllVinylsAsync()
    {
        var query = @"SELECT ""Id"", ""Artist"", ""Album"", ""Year"",""Photo"",""Price""
                      FROM ""Vinil""
                      WHERE ""Active"" = TRUE
                      ORDER BY ""Year""";

        var result = await _connection.QueryAsync<Vinyl> (query);

        return result.ToList();
    }

    public async Task<Vinyl> GetVinylByIdAsync(int id)
    {
        var query = @"SELECT ""Id"", ""Artist"", ""Album"", ""Year"",""Photo"",""Price""
                      FROM ""Vinil""
                      WHERE ""Active"" = TRUE AND
                            ""Id"" = @Id
                      ORDER BY ""Year""";

        var result = await _connection.QueryFirstOrDefaultAsync<Vinyl>(query, new {Id = id});

        return result ?? throw new KeyNotFoundException($"Vinyl with ID {id} not found.");
    }
}
