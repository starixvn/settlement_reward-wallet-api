using System.Text.Json.Serialization;

namespace Core.Models;

public record Result(string ErrorCode, string ErrorMessage)
{
    [JsonIgnore]
    public bool IsSuccess => string.IsNullOrEmpty(ErrorCode) || ErrorCode == "0";
}