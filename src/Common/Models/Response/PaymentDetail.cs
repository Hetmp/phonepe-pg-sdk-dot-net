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

namespace pg_sdk_dotnet.Common.Models.Response;

public class PaymentDetail
{
    public string? TransactionId { get; set; }
    public PgV2InstrumentType PaymentMode { get; set; }
    public long Timestamp { get; set; }
    public long Amount { get; set; }
    public string? State { get; set; }
    public string? ErrorCode { get; set; }
    public string? DetailedErrorCode { get; set; }
    public List<InstrumentCombo>? SplitInstruments { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}
