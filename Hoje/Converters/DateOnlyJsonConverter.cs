using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hoje.Converters;
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Formato = "dd/MM/yyyy";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var valor = reader.GetString();

            if (string.IsNullOrWhiteSpace(valor))
                throw new JsonException("A data não pode ser vazia.");

            if (!DateOnly.TryParseExact(valor, Formato, CultureInfo.InvariantCulture, DateTimeStyles.None, out var data))
                throw new JsonException($"Data inválida: '{valor}'. Use o formato {Formato} (ex.: 21/03/1995).");

            return data;
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Formato, CultureInfo.InvariantCulture));
        }
    }