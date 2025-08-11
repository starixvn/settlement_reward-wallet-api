namespace RewardWallet.Features.FetchPendingBalance;

public class FetchPendingBalanceHandler(IDynamicRepository repository)
{
    public async Task<FetchPendingBalanceResponse> HandleAsync(FetchPendingBalanceQuery query, CancellationToken cancellationToken)
    {
        var criteria = query.Adapt<FetchPendingBalanceCriteria>();
        var result = await repository.GetAsync<FetchPendingBalanceResponse>(criteria, cancellationToken: cancellationToken);
        result.ProgramIds = GetProgramIds(result.Programs);

        return result;
    }

    private static int[] GetProgramIds(string programs)
    {
        if (string.IsNullOrWhiteSpace(programs))
        {
            return [];
        }

        return programs
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.TryParse(x, out var v) ? v : (int?)null)
            .Where(x => x.HasValue)
            .Select(x => x.Value)
            .ToArray();
    }
}