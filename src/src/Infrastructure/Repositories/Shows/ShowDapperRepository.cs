using Collection10Api.src.Domain.Entities;
using Dapper;
using Npgsql;
using System.Data;

namespace Collection10Api.src.Infrastructure.Repositories.Shows;

public class ShowDapperRepository : IShowDapperRepository
{
    private readonly IDbConnection _connection;

    public ShowDapperRepository(IConfiguration config)
    {
        _connection = new NpgsqlConnection(config.GetConnectionString("CollectionConnection"));
    }

    public async Task<ICollection<Show>> GetAllShowsAsync()
    {
        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Show""
                      WHERE ""Active"" = TRUE
                      ORDER BY ""ShowDate""";

        var result = await _connection.QueryAsync<Show>(query);

        return result.ToList();
    }   

    public async Task<ICollection<Show>> GetAllShowsUpcomingAsync()
    {
        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Show""
                      WHERE ""Active"" = TRUE AND
                            ""ShowDate"" >= NOW()
                      ORDER BY ""ShowDate"" ASC";

        var result = await _connection.QueryAsync<Show>(query);

        return result.ToList();
    }

    public async Task<ICollection<Show>> GetAllShowsPastAsync()
    {
        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Show""
                      WHERE ""Active"" = TRUE AND
                            ""ShowDate"" < NOW()
                      ORDER BY ""ShowDate"" DESC";

        var result = await _connection.QueryAsync<Show>(query);

        return result.ToList();
    }

    public async Task<Show> GetShowByGuidAsync(Guid guid)
    {
        var query = @"SELECT ""Guid"", ""Artist"", ""Venue"", ""ShowDate"",""Photo""
                      FROM ""Show""
                      WHERE ""Active"" = TRUE AND
                            ""Guid"" = @Guid
                      ORDER BY ""ShowDate""";

        var result = await _connection.QueryFirstOrDefaultAsync<Show>(query, new { Guid = guid });

        return result ?? new Show();
    }
}
