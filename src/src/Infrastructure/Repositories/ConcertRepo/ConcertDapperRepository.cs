using Collection10Api.src.Domain.Entities;
using Dapper;
using Npgsql;
using System.Data;

namespace Collection10Api.src.Infrastructure.Repositories.ConcertRepo;

public class ConcertDapperRepository : IConcertDapperRepository
{
    private readonly IDbConnection _connection;

    public ConcertDapperRepository(IConfiguration config)
    {
        _connection = new NpgsqlConnection(config.GetConnectionString("CollectionConnection"));
    }

    public async Task<ICollection<Concert>> GetAllConcertsAsync()
    {
        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Concert""
                      WHERE ""Active"" = TRUE
                      ORDER BY ""ShowDate""";

        var result = await _connection.QueryAsync<Concert>(query);

        return result.ToList();
    }   

    public async Task<ICollection<Concert>> GetAllConcertsUpcomingAsync()
    {
        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Concert""
                      WHERE ""Active"" = TRUE AND
                            ""ShowDate"" >= NOW()
                      ORDER BY ""ShowDate"" ASC";

        var result = await _connection.QueryAsync<Concert>(query);

        return result.ToList();
    }

    public async Task<ICollection<Concert>> GetAllConcertsPastAsync()
    {
        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Concert""
                      WHERE ""Active"" = TRUE AND
                            ""ShowDate"" < NOW()
                      ORDER BY ""ShowDate"" DESC";

        var result = await _connection.QueryAsync<Concert>(query);

        return result.ToList();
    }

    public async Task<Concert> GetConcertByGuidAsync(Guid guid)
    {
        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Concert""
                      WHERE ""Active"" = TRUE AND
                            ""Guid"" = @Guid
                      ORDER BY ""ShowDate""";

        var result = await _connection.QueryFirstOrDefaultAsync<Concert>(query, new { Guid = guid });

        return result ?? new Concert();
    }
}
