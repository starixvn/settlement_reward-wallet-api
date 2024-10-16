namespace Core.Models;

public class AppSettings
{
    public bool EnableLogRequest { get; set; }

    public string SecurityToken { get; init; }
}