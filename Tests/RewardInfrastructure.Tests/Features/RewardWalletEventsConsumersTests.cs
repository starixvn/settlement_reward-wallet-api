using Core.Models;
using MassTransit;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RewardInfrastructure.Constants;
using RewardInfrastructure.Features;
using RewardInfrastructure.Services;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Tests.Features;

public class RewardWalletEventsConsumersTests
{
    private readonly ISabaWalletTicketService sabaWalletTicketService;
    private readonly InsertMessageService insertMessageService;
    private readonly UploadBatchMessageService uploadBatchMessageService;
    private readonly ConsumeContext<RewardWalletUpdated> consumeContext;
    private readonly RewardWalletEventsConsumers consumer;

    public RewardWalletEventsConsumersTests()
    {
        sabaWalletTicketService = Substitute.For<ISabaWalletTicketService>();
        insertMessageService = Substitute.For<InsertMessageService>();
        uploadBatchMessageService = Substitute.For<UploadBatchMessageService>();
        consumeContext = Substitute.For<ConsumeContext<RewardWalletUpdated>>();

        consumer = new RewardWalletEventsConsumers(
            sabaWalletTicketService,
            insertMessageService,
            uploadBatchMessageService);
    }

    [Fact]
    public async Task Consume_WhenIsComplaintIsTrueAndBeforeIsComplaintIsNull_ShouldProcessMessage()
    {
        // Arrange
        var message = new RewardWalletUpdated(123, 456L, true, null);
        consumeContext.Message.Returns(message);

        var ticket = new SabaWalletTicketItem
        {
            TransId = 456L,
            Amount = 100.50m,
            ProgramId = 1,
            CustId = 123,
            Username = "testuser",
            AgentId = 789,
            AgentUsername = "agent1",
            MasterId = 101,
            MasterUsername = "master1",
            SuperId = 102,
            SuperUsername = "super1"
        };

        var tickets = new List<SabaWalletTicketItem> { ticket };
        sabaWalletTicketService.GetFeedback(123, 456L).Returns(tickets);

        insertMessageService.CreateMessageAsync(ticket, Levels.Agent).Returns(1);
        insertMessageService.CreateMessageAsync(ticket, Levels.Super).Returns(2);

        uploadBatchMessageService.HandleAsync(ticket, 1, Levels.Agent).Returns(new Result("0", "Success"));
        uploadBatchMessageService.HandleAsync(ticket, 2, Levels.Super).Returns(new Result("0", "Success"));

        // Act
        await consumer.Consume(consumeContext);

        // Assert
        await sabaWalletTicketService.Received(1).GetFeedback(123, 456L);
        await insertMessageService.Received(1).CreateMessageAsync(ticket, Levels.Agent);
        await insertMessageService.Received(1).CreateMessageAsync(ticket, Levels.Super);
        await uploadBatchMessageService.Received(1).HandleAsync(ticket, 1, Levels.Agent);
        await uploadBatchMessageService.Received(1).HandleAsync(ticket, 2, Levels.Super);
    }

    [Fact]
    public async Task Consume_WhenIsComplaintIsFalse_ShouldNotProcessMessage()
    {
        // Arrange
        var message = new RewardWalletUpdated(123, 456L, false, null);
        consumeContext.Message.Returns(message);

        // Act
        await consumer.Consume(consumeContext);

        // Assert
        await sabaWalletTicketService.DidNotReceive().GetFeedback(Arg.Any<int>(), Arg.Any<long>());
        await insertMessageService.DidNotReceive().CreateMessageAsync(Arg.Any<SabaWalletTicketItem>(), Arg.Any<string>());
        await uploadBatchMessageService.DidNotReceive().HandleAsync(Arg.Any<SabaWalletTicketItem>(), Arg.Any<int>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Consume_WhenBeforeIsComplaintIsNotNull_ShouldNotProcessMessage()
    {
        // Arrange
        var message = new RewardWalletUpdated(123, 456L, true, false);
        consumeContext.Message.Returns(message);

        // Act
        await consumer.Consume(consumeContext);

        // Assert
        await sabaWalletTicketService.DidNotReceive().GetFeedback(Arg.Any<int>(), Arg.Any<long>());
        await insertMessageService.DidNotReceive().CreateMessageAsync(Arg.Any<SabaWalletTicketItem>(), Arg.Any<string>());
        await uploadBatchMessageService.DidNotReceive().HandleAsync(Arg.Any<SabaWalletTicketItem>(), Arg.Any<int>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Consume_WhenIsComplaintIsNull_ShouldNotProcessMessage()
    {
        // Arrange
        var message = new RewardWalletUpdated(123, 456L, null, null);
        consumeContext.Message.Returns(message);

        // Act
        await consumer.Consume(consumeContext);

        // Assert
        await sabaWalletTicketService.DidNotReceive().GetFeedback(Arg.Any<int>(), Arg.Any<long>());
        await insertMessageService.DidNotReceive().CreateMessageAsync(Arg.Any<SabaWalletTicketItem>(), Arg.Any<string>());
        await uploadBatchMessageService.DidNotReceive().HandleAsync(Arg.Any<SabaWalletTicketItem>(), Arg.Any<int>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Consume_WhenTicketsListIsEmpty_ShouldThrowException()
    {
        // Arrange
        var message = new RewardWalletUpdated(123, 456L, true, null);
        consumeContext.Message.Returns(message);

        var tickets = new List<SabaWalletTicketItem>();
        sabaWalletTicketService.GetFeedback(123, 456L).Returns(tickets);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            consumer.Consume(consumeContext));
    }

    [Fact]
    public async Task Consume_WhenGetFeedbackThrowsException_ShouldPropagateException()
    {
        // Arrange
        var message = new RewardWalletUpdated(123, 456L, true, null);
        consumeContext.Message.Returns(message);

        sabaWalletTicketService.GetFeedback(123, 456L).ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            consumer.Consume(consumeContext));
    }

    [Fact]
    public async Task Consume_WhenCreateMessageAsyncThrowsException_ShouldPropagateException()
    {
        // Arrange
        var message = new RewardWalletUpdated(123, 456L, true, null);
        consumeContext.Message.Returns(message);

        var ticket = new SabaWalletTicketItem
        {
            TransId = 456L,
            Amount = 100.50m,
            ProgramId = 1,
            CustId = 123,
            Username = "testuser",
            AgentId = 789,
            AgentUsername = "agent1",
            MasterId = 101,
            MasterUsername = "master1",
            SuperId = 102,
            SuperUsername = "super1"
        };

        var tickets = new List<SabaWalletTicketItem> { ticket };
        sabaWalletTicketService.GetFeedback(123, 456L).Returns(tickets);

        insertMessageService.CreateMessageAsync(ticket, Levels.Agent).ThrowsAsync(new InvalidOperationException("Message creation failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            consumer.Consume(consumeContext));
    }

    [Fact]
    public async Task Consume_WhenHandleAsyncThrowsException_ShouldPropagateException()
    {
        // Arrange
        var message = new RewardWalletUpdated(123, 456L, true, null);
        consumeContext.Message.Returns(message);

        var ticket = new SabaWalletTicketItem
        {
            TransId = 456L,
            Amount = 100.50m,
            ProgramId = 1,
            CustId = 123,
            Username = "testuser",
            AgentId = 789,
            AgentUsername = "agent1",
            MasterId = 101,
            MasterUsername = "master1",
            SuperId = 102,
            SuperUsername = "super1"
        };

        var tickets = new List<SabaWalletTicketItem> { ticket };
        sabaWalletTicketService.GetFeedback(123, 456L).Returns(tickets);

        insertMessageService.CreateMessageAsync(ticket, Levels.Agent).Returns(1);

        uploadBatchMessageService.HandleAsync(ticket, 1, Levels.Agent).ThrowsAsync(new InvalidOperationException("Upload failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            consumer.Consume(consumeContext));
    }

    [Theory]
    [InlineData(true, null, true)]
    [InlineData(false, null, false)]
    [InlineData(null, null, false)]
    [InlineData(true, false, false)]
    [InlineData(true, true, false)]
    public async Task Consume_WithDifferentMessageValues_ShouldProcessCorrectly(bool? isComplaint, bool? beforeIsComplaint, bool shouldProcess)
    {
        // Arrange
        var message = new RewardWalletUpdated(123, 456L, isComplaint, beforeIsComplaint);
        consumeContext.Message.Returns(message);

        if (shouldProcess)
        {
            var ticket = new SabaWalletTicketItem
            {
                TransId = 456L,
                Amount = 100.50m,
                ProgramId = 1,
                CustId = 123,
                Username = "testuser",
                AgentId = 789,
                AgentUsername = "agent1",
                MasterId = 101,
                MasterUsername = "master1",
                SuperId = 102,
                SuperUsername = "super1"
            };

            var tickets = new List<SabaWalletTicketItem> { ticket };
            sabaWalletTicketService.GetFeedback(123, 456L).Returns(tickets);

            insertMessageService.CreateMessageAsync(ticket, Levels.Agent).Returns(1);
            insertMessageService.CreateMessageAsync(ticket, Levels.Super).Returns(2);

            uploadBatchMessageService.HandleAsync(ticket, 1, Levels.Agent).Returns(new Result("0", "Success"));
            uploadBatchMessageService.HandleAsync(ticket, 2, Levels.Super).Returns(new Result("0", "Success"));
        }

        // Act
        await consumer.Consume(consumeContext);

        // Assert
        if (shouldProcess)
        {
            await sabaWalletTicketService.Received(1).GetFeedback(123, 456L);
            await insertMessageService.Received(1).CreateMessageAsync(Arg.Any<SabaWalletTicketItem>(), Levels.Agent);
            await insertMessageService.Received(1).CreateMessageAsync(Arg.Any<SabaWalletTicketItem>(), Levels.Super);
        }
        else
        {
            await sabaWalletTicketService.DidNotReceive().GetFeedback(Arg.Any<int>(), Arg.Any<long>());
            await insertMessageService.DidNotReceive().CreateMessageAsync(Arg.Any<SabaWalletTicketItem>(), Arg.Any<string>());
        }
    }

    [Fact]
    public async Task Consume_WhenMultipleTicketsReturned_ShouldUseFirstTicket()
    {
        // Arrange
        var message = new RewardWalletUpdated(123, 456L, true, null);
        consumeContext.Message.Returns(message);

        var ticket1 = new SabaWalletTicketItem
        {
            TransId = 456L,
            Amount = 100.50m,
            ProgramId = 1,
            CustId = 123,
            Username = "testuser1",
            AgentId = 789,
            AgentUsername = "agent1",
            MasterId = 101,
            MasterUsername = "master1",
            SuperId = 102,
            SuperUsername = "super1"
        };

        var ticket2 = new SabaWalletTicketItem
        {
            TransId = 456L,
            Amount = 200.75m,
            ProgramId = 2,
            CustId = 123,
            Username = "testuser2",
            AgentId = 790,
            AgentUsername = "agent2",
            MasterId = 102,
            MasterUsername = "master2",
            SuperId = 103,
            SuperUsername = "super2"
        };

        var tickets = new List<SabaWalletTicketItem> { ticket1, ticket2 };
        sabaWalletTicketService.GetFeedback(123, 456L).Returns(tickets);

        insertMessageService.CreateMessageAsync(ticket1, Levels.Agent).Returns(1);
        insertMessageService.CreateMessageAsync(ticket1, Levels.Super).Returns(2);

        uploadBatchMessageService.HandleAsync(ticket1, 1, Levels.Agent).Returns(new Result("0", "Success"));
        uploadBatchMessageService.HandleAsync(ticket1, 2, Levels.Super).Returns(new Result("0", "Success"));

        // Act
        await consumer.Consume(consumeContext);

        // Assert
        await sabaWalletTicketService.Received(1).GetFeedback(123, 456L);
        await insertMessageService.Received(1).CreateMessageAsync(ticket1, Levels.Agent);
        await insertMessageService.Received(1).CreateMessageAsync(ticket1, Levels.Super);
        await insertMessageService.DidNotReceive().CreateMessageAsync(ticket2, Arg.Any<string>());
    }

    [Fact]
    public async Task Consume_WhenMessageHasValidData_ShouldCallServicesInCorrectOrder()
    {
        // Arrange
        var message = new RewardWalletUpdated(123, 456L, true, null);
        consumeContext.Message.Returns(message);

        var ticket = new SabaWalletTicketItem
        {
            TransId = 456L,
            Amount = 100.50m,
            ProgramId = 1,
            CustId = 123,
            Username = "testuser",
            AgentId = 789,
            AgentUsername = "agent1",
            MasterId = 101,
            MasterUsername = "master1",
            SuperId = 102,
            SuperUsername = "super1"
        };

        var tickets = new List<SabaWalletTicketItem> { ticket };
        sabaWalletTicketService.GetFeedback(123, 456L).Returns(tickets);

        insertMessageService.CreateMessageAsync(ticket, Levels.Agent).Returns(1);
        insertMessageService.CreateMessageAsync(ticket, Levels.Super).Returns(2);

        uploadBatchMessageService.HandleAsync(ticket, 1, Levels.Agent).Returns(new Result("0", "Success"));
        uploadBatchMessageService.HandleAsync(ticket, 2, Levels.Super).Returns(new Result("0", "Success"));

        // Act
        await consumer.Consume(consumeContext);

        // Assert
        Received.InOrder(() =>
        {
            sabaWalletTicketService.GetFeedback(123, 456L);
            insertMessageService.CreateMessageAsync(ticket, Levels.Agent);
            uploadBatchMessageService.HandleAsync(ticket, 1, Levels.Agent);
            insertMessageService.CreateMessageAsync(ticket, Levels.Super);
            uploadBatchMessageService.HandleAsync(ticket, 2, Levels.Super);
        });
    }

    [Fact]
    public async Task Consume_WhenUploadBatchMessageServiceReturnsError_ShouldContinueProcessing()
    {
        // Arrange
        var message = new RewardWalletUpdated(123, 456L, true, null);
        consumeContext.Message.Returns(message);

        var ticket = new SabaWalletTicketItem
        {
            TransId = 456L,
            Amount = 100.50m,
            ProgramId = 1,
            CustId = 123,
            Username = "testuser",
            AgentId = 789,
            AgentUsername = "agent1",
            MasterId = 101,
            MasterUsername = "master1",
            SuperId = 102,
            SuperUsername = "super1"
        };

        var tickets = new List<SabaWalletTicketItem> { ticket };
        sabaWalletTicketService.GetFeedback(123, 456L).Returns(tickets);

        insertMessageService.CreateMessageAsync(ticket, Levels.Agent).Returns(1);
        insertMessageService.CreateMessageAsync(ticket, Levels.Super).Returns(2);

        uploadBatchMessageService.HandleAsync(ticket, 1, Levels.Agent).Returns(new Result("1", "Error"));
        uploadBatchMessageService.HandleAsync(ticket, 2, Levels.Super).Returns(new Result("0", "Success"));

        // Act
        await consumer.Consume(consumeContext);

        // Assert
        await sabaWalletTicketService.Received(1).GetFeedback(123, 456L);
        await insertMessageService.Received(1).CreateMessageAsync(ticket, Levels.Agent);
        await insertMessageService.Received(1).CreateMessageAsync(ticket, Levels.Super);
        await uploadBatchMessageService.Received(1).HandleAsync(ticket, 1, Levels.Agent);
        await uploadBatchMessageService.Received(1).HandleAsync(ticket, 2, Levels.Super);
    }
}