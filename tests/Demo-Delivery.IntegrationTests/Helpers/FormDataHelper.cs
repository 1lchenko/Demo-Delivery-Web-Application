using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace Demo_Delivery.IntegrationTests;

public static class FormDataHelper
{
    public static MultipartFormDataContent Create(IRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var properties = request.GetType().GetProperties();
        var formData = new MultipartFormDataContent();
        
        foreach (var propertyInfo in properties)
        {
            var propertyValue = propertyInfo.GetValue(request);
            if (propertyValue is IEnumerable<IFormFile> files)
            {
                foreach (var file in files)
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    formData.Add(fileContent, propertyInfo.Name, file.FileName);
                }
            }
            else if (propertyValue != null)
            {
                formData.Add(new StringContent(propertyValue.ToString()), propertyInfo.Name);
            }
        }

        return formData;
    }
}