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

public class IntentPaymentV2Instrument(
    string? targetApp
    ) : PaymentV2Instrument(PgV2InstrumentType.UPI_INTENT)
{
    public string? TargetApp { get; private set;} = targetApp;

    public static IntentPaymentV2InstrumentBuilder Builder()
    {
        return new IntentPaymentV2InstrumentBuilder();
    } 

    public class IntentPaymentV2InstrumentBuilder
    {
        private string? _targetApp;

        public IntentPaymentV2InstrumentBuilder SetTargetApp(string? targetApp)
        {
            this._targetApp = targetApp;
            return this;
        }

        public IntentPaymentV2Instrument Build()
        {
            return new IntentPaymentV2Instrument(this._targetApp);
        }
    }
}
