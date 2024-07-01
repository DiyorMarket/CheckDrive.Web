using CheckDrive.Web.Exceptions;

namespace CheckDrive.Web.Service
{
    public class ApiClient
    {
        private const string baseUrl = "https://miraziz-001-site1.ctempurl.com/api";

        private readonly HttpClient _client = new();
        private readonly IHttpContextAccessor _contextAccessor;

        public ApiClient(IHttpContextAccessor contextAccessor)
        {
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            string token = string.Empty;
            var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress?.AbsolutePath + "/" + url);
            _contextAccessor.HttpContext?.Request.Cookies.TryGetValue("tasty-cookies", out token);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException(response.StatusCode, $"Error fetching url: {url}");
            }

            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, string data)
        {
            string token = string.Empty;
            var request = new HttpRequestMessage(HttpMethod.Post, _client.BaseAddress?.AbsolutePath + "/" + url)
            {
                Content = new StringContent(data, System.Text.Encoding.UTF8, "application/json")
            };
            _contextAccessor.HttpContext?.Request.Cookies.TryGetValue("tasty-cookies", out token);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException(response.StatusCode, $"Error fetching url: {url}");
            }

            return response;
        }

        public async Task<HttpResponseMessage> PutAsync(string url, string data)
        {
            string token = string.Empty;
            var request = new HttpRequestMessage(HttpMethod.Put, _client.BaseAddress?.AbsolutePath + "/" + url)
            {
                Content = new StringContent(data, System.Text.Encoding.UTF8, "application/json")
            };
            _contextAccessor.HttpContext?.Request.Cookies.TryGetValue("tasty-cookies", out token);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException(response.StatusCode, $"Error fetching url: {url}");
            }

            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            string token = string.Empty;
            var request = new HttpRequestMessage(HttpMethod.Delete, _client.BaseAddress?.AbsolutePath + "/" + url);
            _contextAccessor.HttpContext?.Request.Cookies.TryGetValue("tasty-cookies", out token);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException(response.StatusCode, $"Error fetching url: {url}");
            }

            return response;
        }
    }
}
