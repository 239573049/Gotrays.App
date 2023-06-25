using GotraysApp.Services;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;

namespace GotraysApp.Helper;

public class ApiClientHelper : CallerBase
{
    public ApiClientHelper(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task CreateChatGptClient(object value, Action<string> result)
    {
        var response = await HttpRequestRaw("https://open666.cn/api/v1/Chats/SendMessage", value);
        var json = await response.Content.ReadAsStreamAsync();
        var strings = JsonSerializer.DeserializeAsyncEnumerable<string>(json);
        try
        {
            await foreach (var str in strings)
            {
                result.Invoke(str);
            }
        }
        catch
        {
        }

    }

    public async Task<string> ImageAI(string prompt)
    {
        var result = await PostStringAsync("https://open666.cn/api/v1/Chats/SDGeneration?prompt="+ prompt);

        return result;

    }


    public async Task<HttpResponseMessage> HttpRequestRaw(string url, object postData = null, bool streaming = true)
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
                string jsonContent = JsonSerializer.Serialize(postData, new JsonSerializerOptions
                {
                    IgnoreNullValues = true
                });
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                req.Content = stringContent;
            }
        }
        HttpResponseMessage response = await (await CreateServiceHttp()).SendAsync(req, streaming ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        else
        {
            string resultAsString;
            try
            {
                resultAsString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                resultAsString = "Additionally, the following error was thrown when attemping to read the response content: " + e;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new AuthenticationException("OpenAI rejected your authorization, most likely due to an invalid API Key.  Try checking your API Key and see https://github.com/OkGoDoIt/OpenAI-API-dotnet#authentication for guidance.  Full API response follows: " + resultAsString);
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException("OpenAI had an internal server error, which can happen occasionally.  Please retry your request. " + resultAsString);
            }
            else
            {
                throw new HttpRequestException(resultAsString);
            }
        }
    }
}
