using Core.Models;
using Fanex.Data.Repository;
using Fanex.Settlement.Converter.Services;
using RewardInfrastructure.Constants;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Features;

public class AddTargetMsgHandler
{
    private readonly IDynamicRepository repository;
    private readonly XmlConvertService xmlConvertService;

    public AddTargetMsgHandler(IDynamicRepository repository, XmlConvertService xmlConvertService)
    {
        this.repository = repository;
        this.xmlConvertService = xmlConvertService;
    }

    public async Task<Result> HandleAsync(SabaWalletTicketItem item, int messageId, string level)
    {
        var message = CreateMessageFromTicket(item, messageId, level);

        var criteria = new AddTargetMsgCriteria
        {
            XMLMessage = xmlConvertService.Serialize(message),
        };

        await repository.ExecuteAsync(criteria);

        return new Result(criteria.ErrorCode.ToString(), criteria.ErrorMsg);
    }

    private Message CreateMessageFromTicket(SabaWalletTicketItem item, int messageId, string level)
    {
        List<Customer> participants = level == Levels.Agent
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
