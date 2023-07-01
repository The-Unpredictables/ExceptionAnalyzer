using System.Text.Json.Serialization;

namespace BugfixAIProvider.Models;

public class BugfixResult
{
    [JsonPropertyName("bugDescription")]
    public string? BugDescription { get; set; }
    [JsonPropertyName("bugfixExplanation")]
    public string? BugfixExplanation { get; set; }
    [JsonPropertyName("gitPatchContent")]
    public string? GitPatchContent { get; set; }
    [JsonPropertyName("additionalInformation")]
    public string? AdditionalInformation { get; set; }
}