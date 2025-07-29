using Core.Models;
using Fanex.Data.Repository;
using Fanex.Settlement.Converter.Services;
using RewardInfrastructure.Criteria;
using RewardInfrastructure.Models;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Services;

public interface UploadBatchMessageService
{
    Task<Result> HandleAsync(SabaWalletTicketItem item, int messageId, string level);
}

public class UploadBatchMessageServiceImp(
    IDynamicRepository repository,
    XmlConvertService xmlConvertService) : UploadBatchMessageService
{
    public async Task<Result> HandleAsync(SabaWalletTicketItem item, int messageId, string level)
    {
        var message = CreateMessageFromTicket(item, messageId, level);

        var criteria = new UploadBatchMessageCriteria
        {
            XMLMessage = xmlConvertService.Serialize(message)
        };

        await repository.ExecuteAsync(criteria);

        return new Result(criteria.ErrorCode.ToString(), criteria.ErrorMsg);
    }

    private static Message CreateMessageFromTicket(SabaWalletTicketItem item, int messageId, string level)
    {
        List<Customer> participants = level == Constants.Levels.Agent
            ? [new Customer(item.AgentId, item.AgentUsername)]
            : [new Customer(item.MasterId, item.MasterUsername), new Customer(item.SuperId, item.SuperUsername)];

        var messageItems = participants
            .Where(p => p.Id != 0)
            .Select(p => new MessageItem
            {
                TargetId = p.Id,
                TargetName = p.Name,
                MessageId = messageId,
                BetTransId = item.TransId.ToString()
            })
            .ToList();

        return new Message
        {
            Items = messageItems
        };
    }
}