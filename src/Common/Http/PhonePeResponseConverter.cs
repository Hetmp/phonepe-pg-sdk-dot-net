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

namespace pg_sdk_dotnet.Common.Http;
public class PhonePeResponseConverter : JsonConverter<PhonePeResponse>
{
    public override PhonePeResponse Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var response = new PhonePeResponse
        {
            Code = "UNKNOWN_ERROR",
            Message = "No message provided",
            ErrorCode = "UNKNOWN_ERROR"
        };

        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;

            if (root.TryGetProperty("success", out var successElement))
            {
                response.Success = successElement.GetBoolean();
            }

            if (root.TryGetProperty("code", out var codeElement))
            {
                response.Code = codeElement.GetString() ?? "UNKNOWN_ERROR";
            }

            if (root.TryGetProperty("message", out var messageElement))
            {
                response.Message = messageElement.GetString() ?? "No message provided";
            }

            if (root.TryGetProperty("errorCode", out var errorCodeElement))
            {
                response.ErrorCode = errorCodeElement.GetString() ?? "UNKNOWN_ERROR";
            }

            if (root.TryGetProperty("data", out var dataElement))
            {
                response.Data = JsonSerializer.Deserialize<Dictionary<string, object>>(dataElement.GetRawText(), options) ?? [];
            }
        }

        return response;
    }

    public override void Write(Utf8JsonWriter writer, PhonePeResponse value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteBoolean("success", value.Success);
        writer.WriteString("code", value.Code);
        writer.WriteString("message", value.Message);
        writer.WriteString("errorCode", value.ErrorCode);

        if (value.Data != null && value.Data.Count > 0)
        {
            writer.WritePropertyName("data");
            JsonSerializer.Serialize(writer, value.Data, options);
        }

        writer.WriteEndObject();
    }

}
