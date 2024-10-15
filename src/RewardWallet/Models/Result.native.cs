
using System.Text.Json.Serialization;

namespace RewardWallet.Models;

public record class Result(string ErrorCode, string ErrorMessage)
{
    [JsonIgnore]
    public bool IsSuccess => string.IsNullOrEmpty(ErrorCode) || ErrorCode == "0";
}