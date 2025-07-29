using RewardInfrastructure.Models;
using System.Xml.Serialization;

namespace RewardWallet.Tests.Infrastructure.Features;

public class MessageTests
{
    [Fact]
    public void Message_WhenDeserialized_ContainsCorrectData()
    {
        // Arrange
        const string xml = """
                           <?xml version="1.0" encoding="utf-8"?>
                           <Messages>
                               <Item MessageId="1" TargetId="100" TargetName="TestUser" BetTransId="123" />
                               <Item MessageId="2" TargetId="200" TargetName="TestUser2" BetTransId="456" />
                           </Messages>
                           """;

        var serializer = new XmlSerializer(typeof(Message));

        // Act
        using var reader = new StringReader(xml);
        var message = (Message)serializer.Deserialize(reader);

        // Assert
        Assert.NotNull(message);
        Assert.NotNull(message.Items);
        Assert.Equal(2, message.Items.Count);

        var firstItem = message.Items[0];
        Assert.Equal(1, firstItem.MessageId);
        Assert.Equal(100, firstItem.TargetId);
        Assert.Equal("TestUser", firstItem.TargetName);
        Assert.Equal("123", firstItem.BetTransId);

        var secondItem = message.Items[1];
        Assert.Equal(2, secondItem.MessageId);
        Assert.Equal(200, secondItem.TargetId);
        Assert.Equal("TestUser2", secondItem.TargetName);
        Assert.Equal("456", secondItem.BetTransId);
    }

    [Fact]
    public void Message_WhenSerialized_ProducesCorrectXml()
    {
        // Arrange
        var message = new Message
        {
            Items =
            [
                new MessageItem { MessageId = 1, TargetId = 100, TargetName = "TestUser", BetTransId = "123" },
                new MessageItem { MessageId = 2, TargetId = 200, TargetName = "TestUser2", BetTransId = "456" }
            ]
        };

        var serializer = new XmlSerializer(typeof(Message));

        // Act
        using var writer = new StringWriter();
        serializer.Serialize(writer, message);
        var xml = writer.ToString();

        // Assert
        Assert.True(xml.Contains("<Messages>") || xml.Contains("<Messages "), "XML should contain <Messages> root element");
        Assert.Contains("<Item MessageId=\"1\"", xml);
        Assert.Contains("TargetId=\"100\"", xml);
        Assert.Contains("TargetName=\"TestUser\"", xml);
        Assert.Contains("BetTransId=\"123\"", xml);
        Assert.Contains("<Item MessageId=\"2\"", xml);
        Assert.Contains("TargetId=\"200\"", xml);
        Assert.Contains("TargetName=\"TestUser2\"", xml);
        Assert.Contains("BetTransId=\"456\"", xml);
    }
}