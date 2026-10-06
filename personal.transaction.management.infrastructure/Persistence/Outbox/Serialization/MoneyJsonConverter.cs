using System.Text.Json;
using System.Text.Json.Serialization;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.infrastructure.Persistence.Outbox.Serialization;

internal sealed class MoneyJsonConverter : JsonConverter<Money>
{
    public override Money Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        var value = root.GetProperty("value").GetDecimal();
        var currencyCode = root.GetProperty("currency").GetString()!;

        return Money.Of(value, currencyCode);
    }

    public override void Write(Utf8JsonWriter writer, Money value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("value", value.Value);
        writer.WriteString("currency", value.Currency.Code);
        writer.WriteEndObject();
    }
}
