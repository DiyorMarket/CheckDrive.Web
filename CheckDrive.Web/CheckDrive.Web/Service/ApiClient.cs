using System.Net.Http.Headers;

namespace CheckDrive.Web.Service;

public class ApiClient
{
    private readonly HttpClient _client = new();
    private readonly IHttpContextAccessor _contextAccessor;

    public ApiClient(IHttpContextAccessor contextAccessor, IConfiguration configuration)
    {
        var baseUrl = configuration.GetValue<string>("ApiUrl");

        if (string.IsNullOrEmpty(baseUrl))
        {
            throw new InvalidOperationException("API url is not supplied.");
        }

        _client.BaseAddress = new Uri(baseUrl);
        _client.DefaultRequestHeaders.Add("Accept", "application/json");

        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        AddToken();

        var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress?.AbsolutePath + url);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<HttpResponseMessage> PostAsync(string url, string data)
    {
        AddToken();

        var request = new HttpRequestMessage(HttpMethod.Post, _client.BaseAddress?.AbsolutePath + url)
        {
            Content = new StringContent(data, System.Text.Encoding.UTF8, "application/json")
        };

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return response;
    }

    private void AddToken()
    {
        if (_contextAccessor.HttpContext is null)
        {
            return;
        }

        if (_contextAccessor.HttpContext.Request.Cookies.TryGetValue("auth-token", out var token))
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
