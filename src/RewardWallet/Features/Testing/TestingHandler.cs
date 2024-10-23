namespace RewardWallet.Features.Testing;

public class TestingHandler
{
    public TestingResponse HandleAsync()
    {
        return new TestingResponse
        {
            Value = DateTimeOffset.Now.ToString()
        };
    }
}