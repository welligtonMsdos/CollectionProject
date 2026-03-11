using Collection10Api.src.Domain.Entities;
using Dapper;

namespace Collection10Api.src.Infrastructure.Repositories.VinylRepo;

public class VinylDapperRepository : BaseRepository, IVinylDapperRepository
{ 
    public VinylDapperRepository(IConfiguration config) : base(config) { }   

    public async Task<IEnumerable<Vinyl>> GetAsync(string email)
    {
        using var connection = CreateConnection();       

        var query = @"SELECT ""Guid"",""Artist"",""Album"",""Year"",""Photo"",""Price"",""Active"" 
                      FROM ""Vinyl""
                      ORDER BY ""Year""";

        return await connection.QueryAsync<Vinyl>(query);
    }

    public async Task<Vinyl?> GetByGuidAsync(Guid guid)
    {
        using var connection = CreateConnection();      

        var query = @"SELECT ""Guid"",""Artist"",""Album"",""Year"",""Photo"",""Price"",""Active""
                      FROM ""Vinyl"" 
                      WHERE ""Guid"" = @Guid
                      ORDER BY ""Year""";

        return await connection.QueryFirstOrDefaultAsync<Vinyl>(query, new { Guid = guid });
    }
}
