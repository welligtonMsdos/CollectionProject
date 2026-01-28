using Microsoft.Extensions.Caching.Memory;

namespace Collection10Api.src.Infrastructure.Utils;

public static class Util
{
    private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

    public static Task<string> GetSecretWithCacheAsync()
    {
		try
		{
			string cachekey = "jwtSecret";

			if(_cache.TryGetValue(cachekey, out string secret)) return Task.FromResult(secret);

			var key = "fedaf7d8863b48e197b9287d492b708e";

			_cache.Set(cachekey, key, TimeSpan.FromHours(120));

            return Task.FromResult(key);
		}
		catch (Exception)
		{
			return Task.FromResult("key not found");
		}
    }
}
