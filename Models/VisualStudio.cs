using System.Text.Json.Serialization;

namespace VSLauncher.Models;

public class VisualStudio
{
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("productPath")]
    public string ProductPath { get; set; } = string.Empty;

    [JsonPropertyName("catalog")]
    public VisualStudioCatalog Catalog { get; set; } = new();
}

public class VisualStudioCatalog
{
    [JsonPropertyName("productDisplayVersion")]
    public string ProductDisplayVersion { get; set; } = string.Empty;
}
