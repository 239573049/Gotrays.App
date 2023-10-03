using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace GotraysApp.Services;

public class ChatService : CallerBase
{
    public ChatService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }


    public async Task<HttpResponseMessage> HttpRequestRaw(string url, object postData = null,
        bool streaming = true)
    {
        HttpRequestMessage req = new(HttpMethod.Post, url);

        if (postData != null)
        {
            if (postData is HttpContent)
            {
                req.Content = postData as HttpContent;
            }
            else
            {
                var jsonContent = JsonSerializer.Serialize(postData, new JsonSerializerOptions
                {
                    IgnoreNullValues = true
                });
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                req.Content = stringContent;
            }
        }

        HttpResponseMessage response = await (await CreateServiceHttp()).SendAsync(req,
            streaming ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        string resultAsString;
        try
        {
            resultAsString = await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            resultAsString =
                "Additionally, the following error was thrown when attemping to read the response content: " + e;
        }

        throw new HttpRequestException(resultAsString);
    }
}