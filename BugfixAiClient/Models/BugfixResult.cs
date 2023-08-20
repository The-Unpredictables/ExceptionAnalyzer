using Newtonsoft.Json;

namespace BugfixAiClient.Models;

public class BugfixResult
{
    [JsonProperty("bugDescription")]
    public string? BugDescription { get; set; }
    
    [JsonProperty("bugfixExplanation")]
    public string? BugfixExplanation { get; set; }
    
    [JsonProperty("codePointers")]
    public List<CodePointer>? CodePointers { get; set; }
    
    [JsonProperty("additionalInformation")]
    public string? AdditionalInformation { get; set; }
    

}