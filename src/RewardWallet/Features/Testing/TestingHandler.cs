namespace RewardWallet.Features.Testing;

public class TestingHandler
{
    public async Task<TestingResponse> HandleAsync(CancellationToken cancellationToken)
    {
        return new TestingResponse
        {
            Value = DateTimeOffset.Now.ToString()
        };
    }
}