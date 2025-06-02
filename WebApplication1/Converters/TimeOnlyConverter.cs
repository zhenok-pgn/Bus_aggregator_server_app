using System.Text.Json.Serialization;
using System.Text.Json;

namespace App.WEB.Converters
{
    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        private const string Format = "HH:mm"; // Формат "часы:минуты"

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Пытаемся распарсить время в формате "HH:mm"
            if (TimeOnly.TryParseExact(reader.GetString(), Format, null, System.Globalization.DateTimeStyles.None, out var time))
            {
                return time;
            }

            throw new JsonException($"Неверный формат времени. Ожидается: {Format}");
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
