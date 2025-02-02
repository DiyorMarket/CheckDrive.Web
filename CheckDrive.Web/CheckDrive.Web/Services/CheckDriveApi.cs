using Newtonsoft.Json;

namespace CheckDrive.Web.Services;

public class CheckDriveApi(HttpClient client, ILogger<CheckDriveApi> logger)
{
    public async Task<TResult> GetAsync<TResult>(string url)
    {
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TResult>(json);

        if (result is null)
        {
            logger.LogWarning(
                "Response deserialization returned null for type {Type} from resource {Url} and method GET",
                typeof(TResult),
                url);

            throw new InvalidCastException("Could not deserialize response");
        }

        return result;
    }

    public async Task<TResult> PostAsync<TBody, TResult>(string url, TBody data)
    {
        var response = await client.PostAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TResult>(json);

        if (result is null)
        {
            logger.LogWarning(
                "Response deserialization returned null for type {Type} from resource {Url} and method POST",
                typeof(TResult),
                url);

            throw new InvalidCastException("Could not deserialize response");
        }

        return result;
    }

    public async Task PutAsync<TBody>(string url, TBody data)
    {
        var response = await client.PutAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string url)
    {
        var response = await client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }
}
