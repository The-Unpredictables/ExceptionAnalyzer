using Newtonsoft.Json;

namespace BugfixAIProvider.Models;

public class CodePointer
{
    [JsonProperty("fullName")]
    public string? FullName { get; set; }
    
    [JsonProperty("parameterTypes")]
    public List<string>? ParameterTypeFullNames { get; set; }
    
    [JsonProperty("codeType")]
    public CodeType CodeType { get; set; }
    
    [JsonProperty("clientId")]
    public Guid ClientId { get; set; }
    
    [JsonProperty("oldSourceCode")]
    public string? OldSourceCode { get; set; }
    
    [JsonProperty("newSourceCode")]
    public string? NewSourceCode { get; set; }
    
}