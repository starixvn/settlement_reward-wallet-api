using Fanex.Caching;

namespace RewardWallet.Features.Testing;

public class TestingHandler(ICache cache)
{
    public async Task<TestingResponse> HandleAsync(CancellationToken cancellationToken)
    {
        var (_, value) = await cache.TryGetAsync<string>("Settlement-Broadcast-API-Product-Type");

        return new TestingResponse
        {
            Value = value
        };
    }
}