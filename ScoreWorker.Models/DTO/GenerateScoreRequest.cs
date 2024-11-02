using System.Text.Json.Serialization;

namespace ScoreWorker.Models.DTO;

public class GenerateScoreRequest
{
    [JsonPropertyName("prompt")]
    public required string Prompt { get; set; }

    [JsonPropertyName("apply_chat_template")]
    public bool ApplyChatTemplate { get; set; }

    [JsonPropertyName("system_prompt")]
    public required string SystemPrompt { get; set; }

    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    [JsonPropertyName("n")]
    public int N { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
}
