namespace NetNet.Gateway.Distributed.Configurations;

public class YarpDistributedConfig
{
    public string RedisConnectionString { get; set; }

    public TimeSpan HeartRate { get; set; } = TimeSpan.FromSeconds(10);
}
