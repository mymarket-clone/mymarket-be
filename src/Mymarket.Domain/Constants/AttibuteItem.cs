using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mymarket.Domain.Constants;

public sealed record AttributeItem(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("value")] JsonElement Value
);