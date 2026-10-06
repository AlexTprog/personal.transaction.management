using System.Text.Json;
using System.Text.Json.Serialization;

namespace personal.transaction.management.infrastructure.Persistence.Outbox.Serialization;

internal static class OutboxJsonSerializerOptions
{
    public static readonly JsonSerializerOptions Instance = Create();

    private static JsonSerializerOptions Create()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        options.Converters.Add(new MoneyJsonConverter());
        options.Converters.Add(new CurrencyJsonConverter());

        return options;
    }
}
