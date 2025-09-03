/*
* Copyright (c) 2025 Original Author(s), PhonePe India Pvt. Ltd.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System.Text.Json;
using System.Text.Json.Serialization;

namespace pg_sdk_dotnet.Common.Models.Request.JsonConverters;
public class PaymentV2InstrumentConverter : JsonConverter<PaymentV2Instrument>
{
    public override PaymentV2Instrument? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            if (root.TryGetProperty("Type", out var typeProperty))
            {
                string type = typeProperty.GetString();
                return type switch
                {
                    "CardPaymentV2Instrument" => JsonSerializer.Deserialize<CardPaymentV2Instrument>(root.GetRawText(), options),
                    "IntentPaymentV2Instrument" => JsonSerializer.Deserialize<IntentPaymentV2Instrument>(root.GetRawText(), options),
                    "CollectPaymentV2Instrument" => JsonSerializer.Deserialize<CollectPaymentV2Instrument>(root.GetRawText(), options),
                    "NetBankingPaymentV2Instrument" => JsonSerializer.Deserialize<NetBankingPaymentV2Instrument>(root.GetRawText(), options),
                    "TokenPaymentV2Instrument" => JsonSerializer.Deserialize<TokenPaymentV2Instrument>(root.GetRawText(), options),
                    _ => throw new JsonException($"Unknown type: {type}")

                };
            }
        }
        throw new JsonException("Invalid JSON format for PaymentV2Instrument.");
    }

    public override void Write(Utf8JsonWriter writer, PaymentV2Instrument value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        string json = JsonSerializer.Serialize(value, value.GetType(), options);
        using JsonDocument doc = JsonDocument.Parse(json);
        writer.WriteStartObject();

        foreach (var property in doc.RootElement.EnumerateObject())
        {
            property.WriteTo(writer);
        }

        writer.WriteEndObject();
    }
}
