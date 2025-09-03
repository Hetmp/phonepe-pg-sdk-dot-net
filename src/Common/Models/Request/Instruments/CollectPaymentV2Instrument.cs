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

namespace pg_sdk_dotnet.Common.Models.Request.Instruments;

public class CollectPaymentV2Instrument(
    CollectPaymentDetails details, 
    string? message
    ) : PaymentV2Instrument(PgV2InstrumentType.UPI_COLLECT)
{
    public CollectPaymentDetails Details { get; private set;} = details;

    public string? Message { get; private set;} = message;

    public static CollectPaymentV2InstrumentBuilder Builder() => new();

    public class CollectPaymentV2InstrumentBuilder
    {
        private CollectPaymentDetails? _details;
        private string? _message;

        public CollectPaymentV2InstrumentBuilder SetDetails(CollectPaymentDetails details)
        {
            this._details = details;
            return this;
        }

        public CollectPaymentV2InstrumentBuilder SetMessage(string? message)
        {
            this._message = message;
            return this;
        }

        public CollectPaymentV2Instrument Build()
        {
            return new CollectPaymentV2Instrument(this._details!, this._message);
        }
    }
}
