using Newtonsoft.Json;

namespace CheckDrive.Web.Exceptions;

public static class ApiResponseHandler
{
    public static async Task<T> HandleApiResponse<T>(HttpResponseMessage response, string errorMessage)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(errorMessage);
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(jsonResponse)
               ?? throw new Exception("Failed to deserialize API response.");
    }
}
