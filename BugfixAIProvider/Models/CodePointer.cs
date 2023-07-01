using System.Text.Json.Serialization;

namespace BugfixAIProvider.Models;

public class CodePointer
{
    [JsonPropertyName("fullName")]
    public string? FullName { get; set; }
    [JsonPropertyName("codeType")]
    public CodeType CodeType { get; set; }
    [JsonPropertyName("clientId")]
    public Guid ClientId { get; set; }
}