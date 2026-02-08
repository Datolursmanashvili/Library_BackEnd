namespace Domain.Shared.RedisModel;

public class RedisOptions
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Password { get; set; } = null!;
    public int DefaultDatabase { get; set; } = 0;
    public int SyncTimeout { get; set; } = 5000;
}