namespace RewardWallet.Features;

public class WriteUserLogHandler
{
    private readonly IDynamicRepository repository;

    public WriteUserLogHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task HandleAsync(WriteUserLogCriteria criteria, CancellationToken cancellationToken)
    {
        var writeUserLogCriteria = criteria.Adapt<WriteUserLogCriteria>();

        await repository.ExecuteAsync(writeUserLogCriteria);
    }
}
