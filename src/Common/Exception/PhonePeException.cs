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

namespace pg_sdk_dotnet.Common.Exception;

public class PhonePeException : System.Exception
{
    public int HttpStatusCode { get; set; }
    public string Message { get; set; }
    public string Code { get; set; } = string.Empty;

    [JsonIgnore]
    public Dictionary<string, object>? AdditionalData { get; set; }

    public PhonePeException(string message) : base(message)
    {
        Message = message;
    }

    public PhonePeException(int httpStatusCode, string message) : base(message)
    {
        HttpStatusCode = httpStatusCode;
        Message = message;
    }

    public PhonePeException(int httpStatusCode, string message, PhonePeResponse phonePeResponse) : base(message)
    {
        HttpStatusCode = httpStatusCode;
        Message = phonePeResponse?.Message ?? message;
        AdditionalData = phonePeResponse?.Data;
        Code = phonePeResponse?.Code ?? string.Empty;
        if (AdditionalData != null)
        {
            foreach (var exPropertyPair in AdditionalData)
            {
                Data[exPropertyPair.Key] = exPropertyPair.Value;
            }
        }
    }

    public override string ToString()
    {
        var additionalDataString = AdditionalData != null
            ? JsonSerializer.Serialize(AdditionalData, new JsonSerializerOptions { WriteIndented = false })
            : "{}";

        return $"{{\n" +
            $"  \"httpStatusCode\": {HttpStatusCode},\n" +
            $"  \"message\": \"{Message}\",\n" +
            $"  \"data\": {additionalDataString},\n" +
            $"  \"code\": \"{Code}\"\n" +
            $"}}";
    }
}