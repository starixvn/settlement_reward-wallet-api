using RewardWallet.Models;
using RewardWallet.Requests;

namespace RewardWallet.Services;

public interface RewardService
{
    Task<Result> Insert(InsertRewardRequest model);
}

public class RewardServiceImp : RewardService
{
    public async Task<Result> Insert(InsertRewardRequest model)
    {
        return new Result();
    }
}