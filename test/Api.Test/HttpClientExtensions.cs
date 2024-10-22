using System.Text.Json;

namespace Entities.Test
{
    public static class HttpClientExtensions
    {
        private static readonly JsonSerializerOptions Options = new ()
        {
            IncludeFields = true,
        };
        public static async Task<T> GetAndDeserialize<T>(this HttpClient client, 
        HttpRequestMessage 
        message)
        {
            var response = await client.SendAsync(message);
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(result, Options)!;
        }
    }
}