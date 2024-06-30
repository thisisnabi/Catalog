namespace Catalog;

public sealed class CatalogOptions
{
    public BrokerOptions BrokerOptions { get; set; } = null!;
    public MediaOptions MediaOptions { get; set; } = null!;
}


public sealed class BrokerOptions
{
    public const string SectionName = "BrokerOptions";

    public required string Host { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
  
public sealed class MediaOptions
{
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
    public required string BucketName { get; set; }
    public required string Endpoint { get; set; }
}


