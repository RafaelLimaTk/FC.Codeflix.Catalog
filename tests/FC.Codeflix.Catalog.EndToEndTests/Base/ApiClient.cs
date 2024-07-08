using System.Text;
using System.Text.Json;

namespace FC.Codeflix.Catalog.EndToEndTests.Base;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
        => _httpClient = httpClient;

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(
        string url,
        object input)
        where TOutput : class
    {
        var response = await _httpClient.PostAsync(
            url,
            new StringContent(
                JsonSerializer.Serialize(input),
                Encoding.UTF8,
                "application/json"
            )
        );

        var output = await GetOutput<TOutput>(response);
        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(
        string route
    ) where TOutput : class
    {
        var response = await _httpClient.GetAsync(route);
        var output = await GetOutput<TOutput>(response);
        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Delete<TOutput>(
        string route
    ) where TOutput : class
    {
        var response = await _httpClient.DeleteAsync(route);
        var output = await GetOutput<TOutput>(response);
        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Put<TOutput>(
        string route,
        object input
    ) where TOutput : class
    {
        var response = await _httpClient.PutAsync(
            route,
            new StringContent(
                JsonSerializer.Serialize(input),
                Encoding.UTF8,
                "application/json"
            )
        );

        var output = await GetOutput<TOutput>(response);
        return (response, output);
    }

    private async Task<TOutput?> GetOutput<TOutput>(HttpResponseMessage response)
    where TOutput : class
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        TOutput? output = null;
        if (string.IsNullOrWhiteSpace(responseContent) == false)
            output = JsonSerializer.Deserialize<TOutput>(
                responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

        return output;
    }
}
