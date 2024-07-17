using Microsoft.AspNetCore.Http;

namespace Demo_Delivery.Application.Common.Interfaces;

public interface IFileService
{
    public Task<string> UploadFileAsync(IFormFile file, string bucketName, string prefix,
        CancellationToken cancellationToken = default);

    public Task DeleteFilesAsync(IEnumerable<string> keys, string bucketName, 
        CancellationToken cancellationToken = default);
    
    public Task DeleteFileAsync(string key, string bucketName, 
        CancellationToken cancellationToken = default);
    
     
}