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

namespace pg_sdk_dotnet.Common.Models;
public class PaymentFlowJsonConverter : JsonConverter<PaymentFlow>
{
    public override PaymentFlow? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var type = root.GetProperty("type").GetString();

        return type switch
        {
            "PG_CHECKOUT" => JsonSerializer.Deserialize<PgCheckoutPaymentFlow>(root.GetRawText(), options),
            "PG" => JsonSerializer.Deserialize<PgPaymentFlow>(root.GetRawText(), options),
            _ => throw new NotSupportedException()
        };
    }

    public override void Write(Utf8JsonWriter writer, PaymentFlow value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case PgCheckoutPaymentFlow pgCheckoutFlow:
                JsonSerializer.Serialize(writer, pgCheckoutFlow, options);
                break;

            case PgPaymentFlow pgPaymentFlow:
                JsonSerializer.Serialize(writer, pgPaymentFlow, options);
                break;

            default:
                throw new NotSupportedException();
        }
    }
}
