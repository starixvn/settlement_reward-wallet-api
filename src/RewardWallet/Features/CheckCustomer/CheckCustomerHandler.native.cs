namespace RewardWallet.Features.CheckCustomer;

public class CheckCustomerHandler(IDynamicRepository repository)
{
    public bool IsCustIdsExists(string cusIds, short SiteDbGroupId, out string inValidCusIds)
    {
        var criteria = new CheckCustomerCriteria
        {
            CustIds = cusIds,
            SiteDbGroupId = SiteDbGroupId
        };
        var results = repository.Fetch<string>(criteria).ToList();

        if (results.Any())
        {
            inValidCusIds = string.Join(",", results.ToArray());
            return false;
        }

        inValidCusIds = string.Empty;
        return true;
    }
}