namespace APIClassRoom.Service;

using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ChatService
{
    private readonly HttpClient _httpClient;
    private readonly string apiKey = "PCmHo5axwwBneSCIXtcAj2PiFxW4fHcM";
    private readonly string apiUrl = "https://api.mistral.ai/v1/chat/completions";

    public ChatService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public async Task<string> GetBotResponse(string userMessage)
    {
        var requestBody = new
        {
            model = "mistral-tiny",
            messages = new[]
            {
                new { role = "user", content = userMessage }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(apiUrl, content);
        var resultJson = await response.Content.ReadAsStringAsync();

        try
        {
            using JsonDocument doc = JsonDocument.Parse(resultJson);
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
        catch
        {
            return "Error: Unable to process response.";
        }
    }
}