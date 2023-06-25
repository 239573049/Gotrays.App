using System.Net.Http.Json;

namespace GotraysApp.Services;
public abstract class CallerBase 
{
    private IHttpClientFactory _httpClientFactory;

    private const string BaseUrl = "https://open666.cn/api/";

    protected CallerBase(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpClient> CreateServiceHttp()
    {
        var http = _httpClientFactory.CreateClient();

        if (http.BaseAddress == null)
        {
            http.BaseAddress = new Uri(BaseUrl);
        }

        string token = await SecureStorage.Default.GetAsync("token");

        if (!string.IsNullOrEmpty(token))
        {
            http.DefaultRequestHeaders.Remove("Authorization");
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        return http;
    }

    public async Task<string> GetStringAsync(string url, object? value = null)
    {
        var client = await CreateServiceHttp().ConfigureAwait(false);
        var response = await client.GetAsync(url + value?.GetPargmeter());
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<T> GetAsync<T>(string url, object? value = null)
    {
        var client = await CreateServiceHttp().ConfigureAwait(false);
        var response = await client.GetAsync(url + value?.GetPargmeter());
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<TValue> PostAsync<TData, TValue>(string url, TData data)
    {

        var client = await CreateServiceHttp().ConfigureAwait(false);
        var response = await client.PostAsJsonAsync(url, data);
        return await response.Content.ReadFromJsonAsync<TValue>();
    }

    public async Task<string> PostStringAsync(string url)
    {
        var client = await CreateServiceHttp().ConfigureAwait(false);
        var response = await client.PostAsync(url, null);

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PostStringAsync<T>(string url, T data)
    {
        var client = await CreateServiceHttp().ConfigureAwait(false);
        var response = await client.PostAsJsonAsync(url, data);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        throw new Exception(await response.Content.ReadAsStringAsync());
    }

    public async Task<byte[]> PostByteAsync<T>(string url, T data)
    {
        var client = await CreateServiceHttp().ConfigureAwait(false);
        var response = await client.PostAsJsonAsync(url, data);

        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<Stream> PostStreamAsync<T>(string url, T data)
    {
        var client = await CreateServiceHttp().ConfigureAwait(false);
        var response = await client.PostAsJsonAsync(url, data);
        return await response.Content.ReadAsStreamAsync();
    }

    public async Task<T> PutAsync<T>(string url, T data)
    {
        var client = await CreateServiceHttp().ConfigureAwait(false);
        var response = await client.PutAsJsonAsync(url, data);
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> DeleteAsync<T>(string url, object value)
    {
        var client = await CreateServiceHttp().ConfigureAwait(false);
        var response = await client.DeleteAsync(url + value.GetPargmeter());
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> DeleteAsync<T>(string url)
    {
        var client = await CreateServiceHttp().ConfigureAwait(false);
        var response = await client.DeleteAsync(url);
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> PatchAsync<T>(string url, T data)
    {
        var client = await CreateServiceHttp().ConfigureAwait(false);
        var response = await client.PatchAsJsonAsync(url, data);
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> PatchAsync<T>(string url, object value)
    {
        var client = await CreateServiceHttp().ConfigureAwait(false);
        var response = await client.PatchAsJsonAsync(url, value);
        return await response.Content.ReadFromJsonAsync<T>();
    }

}
