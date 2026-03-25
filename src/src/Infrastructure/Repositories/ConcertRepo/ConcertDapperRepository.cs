using Collection10Api.src.Domain.Entities;
using Dapper;

namespace Collection10Api.src.Infrastructure.Repositories.ConcertRepo;

public class ConcertDapperRepository : BaseRepository, IConcertDapperRepository
{
    public ConcertDapperRepository(IConfiguration config): base(config){}

    public async Task<ICollection<Concert>> GetUpcomingAsync(string userId)
    {
        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Concert""
                      WHERE ""Active"" = TRUE AND
                            ""ShowDate"" >= NOW() AND
                            ""UserId"" = @UserId 
                      ORDER BY ""ShowDate"" ASC";

        using var connection = CreateConnection();

        var result = await connection.QueryAsync<Concert>(query, new {UserId = userId});

        return result.ToList();
    }

    public async Task<ICollection<Concert>> GetPastAsync(string userId)
    {
        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Concert""
                      WHERE ""Active"" = TRUE AND
                            ""ShowDate"" < NOW() AND
                            ""UserId"" = @UserId
                      ORDER BY ""ShowDate"" DESC";

        using var connection = CreateConnection();

        var result = await connection.QueryAsync<Concert>(query, new {UserId = userId});

        return result.ToList();
    }    

    public async Task<Concert?> GetByGuidAsync(Guid guid)
    {
        using var connection = CreateConnection();

        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Concert""
                      WHERE ""Active"" = TRUE AND
                            ""Guid"" = @Guid";

        return await connection.QueryFirstOrDefaultAsync<Concert>(query, new { Guid = guid });
    }

    public async Task<IEnumerable<Concert>> GetAsync(string userId)
    {
        using var connection = CreateConnection();

        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Concert""
                      WHERE ""Active"" = TRUE AND
                            ""UserId"" = @UserId";

        return await connection.QueryAsync<Concert>(query , new { UserId = userId });
    }
}
