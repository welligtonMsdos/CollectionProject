using Collection10Api.src.Domain.Entities;
using Dapper;

namespace Collection10Api.src.Infrastructure.Repositories.ConcertRepo;

public class ConcertDapperRepository : BaseRepository, IConcertDapperRepository
{
    public ConcertDapperRepository(IConfiguration config): base(config){}

    public async Task<ICollection<Concert>> GetAllConcertsUpcomingAsync()
    {
        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Concert""
                      WHERE ""Active"" = TRUE AND
                            ""ShowDate"" >= NOW()
                      ORDER BY ""ShowDate"" ASC";

        using var connection = CreateConnection();

        var result = await connection.QueryAsync<Concert>(query);

        return result.ToList();
    }

    public async Task<ICollection<Concert>> GetAllConcertsPastAsync()
    {
        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Concert""
                      WHERE ""Active"" = TRUE AND
                            ""ShowDate"" < NOW()
                      ORDER BY ""ShowDate"" DESC";

        using var connection = CreateConnection();

        var result = await connection.QueryAsync<Concert>(query);

        return result.ToList();
    }    

    public async Task<Concert?> GetByGuidAsync(Guid guid)
    {
        return await GetByIdAsync<Concert>(guid);
    }

    public async Task<IEnumerable<Concert>> GetAllAsync()
    {
        return await GetAllAsync<Concert>();
    }
}
