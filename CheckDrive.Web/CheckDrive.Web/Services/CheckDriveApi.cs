using Newtonsoft.Json;

namespace CheckDrive.Web.Services;

public class CheckDriveApi
{
    private readonly HttpClient _client;
    private readonly ILogger<CheckDriveApi> _logger;

    public CheckDriveApi(HttpClient client, ILogger<CheckDriveApi> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResult> GetAsync<TResult>(string url)
    {
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TResult>(json);

        if (result is null)
        {
            _logger.LogWarning(
                "Response deserialization returned null for type {Type} from resource {Url} and method GET",
                typeof(TResult),
                url);

            throw new InvalidCastException("Could not deserialize response");
        }

        return result;
    }

    public async Task<TResult> PostAsync<TBody, TResult>(string url, TBody data)
    {
        var response = await _client.PostAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TResult>(json);

        if (result is null)
        {
            _logger.LogWarning(
                "Response deserialization returned null for type {Type} from resource {Url} and method POST",
                typeof(TResult),
                url);

            throw new InvalidCastException("Could not deserialize response");
        }

        return result;
    }

    public async Task PutAsync<TBody>(string url, TBody data)
    {
        var response = await _client.PutAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string url)
    {
        var response = await _client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }
}
