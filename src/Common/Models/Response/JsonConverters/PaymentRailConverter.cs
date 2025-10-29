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

namespace pg_sdk_dotnet.Common.Models.Response.JsonConverters;

public class PaymentRailConverter : JsonConverter<PaymentRail>
{
    public override PaymentRail? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (!root.TryGetProperty("type", out var typeProperty))
        {
            throw new JsonException("Missing type property for PaymentRail");
        }

        var type = typeProperty.GetString();
        return type switch
        {
            "UPI" => JsonSerializer.Deserialize<UpiPaymentRail>(root.GetRawText(), options),
            "PPI_EGV" => JsonSerializer.Deserialize<PpiEgvPaymentRail>(root.GetRawText(), options),
            "PPI_WALLET" => JsonSerializer.Deserialize<PpiWalletPaymentRail>(root.GetRawText(), options),
            "PG" => JsonSerializer.Deserialize<PgPaymentRail>(root.GetRawText(), options),
            _ => throw new JsonException($"Deseralization not possible because unknown type : {type} in PaymentRailConverter")
        };
    }

    public override void Write(Utf8JsonWriter writer, PaymentRail value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
