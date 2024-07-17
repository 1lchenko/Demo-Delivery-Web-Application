using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Demo_Delivery.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IAmazonS3 _amazonS3;

    public FileService(IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string bucketName, string prefix,
        CancellationToken cancellationToken = default)
    {
        await CheckBucketExistence(bucketName);
        return await AddFileAsync(file, bucketName, prefix, cancellationToken);
    }

    private async Task<string> AddFileAsync(IFormFile file, string bucketName, string prefix,
        CancellationToken cancellationToken = default)
    {
      

        var key = $"{prefix}/{file.FileName}";
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            InputStream = file.OpenReadStream(),
            CannedACL = S3CannedACL.PublicRead
        };
        await _amazonS3.PutObjectAsync(putObjectRequest, cancellationToken);

        return key;
    }

    public async Task DeleteFilesAsync(IEnumerable<string> keys, string bucketName,
        CancellationToken cancellationToken = default)
    {
        
        await _amazonS3.DeletesAsync (bucketName, keys, null, cancellationToken);
    }

    public async Task DeleteFileAsync(string key, string bucketName, CancellationToken cancellationToken = default)
    {
         await _amazonS3.DeleteObjectAsync(bucketName, key, cancellationToken);
    }

    private async Task CheckBucketExistence(string bucketName)
    {
        var bucketExist = await AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
        if (!bucketExist) throw new NotFoundException("Bucket", bucketName);
    }
}