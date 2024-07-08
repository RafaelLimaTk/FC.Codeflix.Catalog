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
        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(
        string route
    ) where TOutput : class
    {
        var response = await _httpClient.GetAsync(route);
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
        return (response, output);
    }
}
