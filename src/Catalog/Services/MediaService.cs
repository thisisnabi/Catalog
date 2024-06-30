using Amazon.S3;
using Amazon.S3.Model;
using MassTransit;
using MassTransit.Caching.Internals;

namespace Catalog.Services;

public class MediaService(IOptions<CatalogOptions> options)
{
    private readonly MediaOptions _options = options.Value.MediaOptions;

    public async Task<string> UploadStream(Stream fileStream)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = _options.Endpoint,
            ForcePathStyle = true,
            SignatureVersion = "4"
        };
        var credentials = new Amazon.Runtime.BasicAWSCredentials(_options.AccessKey, _options.SecretKey);
        using var client = new AmazonS3Client(credentials, config);

        var fileName = Guid.NewGuid().ToString();

        PutObjectRequest request = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = fileName,
            InputStream = fileStream
        };

        var response = await client.PutObjectAsync(request);

        Console.WriteLine($"File '{fileName}' uploaded successfully.");

        GetPreSignedUrlRequest urlRequest = new GetPreSignedUrlRequest
        {
            BucketName = _options.BucketName,
            Key = fileName,
        };

        return await client.GetPreSignedURLAsync(urlRequest);
    }
}

 
