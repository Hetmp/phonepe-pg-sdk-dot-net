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

public class NetBankingPaymentV2Instrument(
    string bankId, 
    string? merchantUserId
    ) : PaymentV2Instrument(PgV2InstrumentType.NET_BANKING)
{
    public string BankId { get; private set;} = bankId;
    public string? MerchantUserId { get; private set;} = merchantUserId;

    public static NetBankingPaymentV2InstrumentBuilder Builder(){
        return new NetBankingPaymentV2InstrumentBuilder();
    }

    public class NetBankingPaymentV2InstrumentBuilder
    {
        private string? _bankId;
        private string? _merchantUserId;

        public NetBankingPaymentV2InstrumentBuilder SetBankId(string bankId)
        {
            this._bankId = bankId;
            return this;
        }

        public NetBankingPaymentV2InstrumentBuilder SetMerchantUserId(string? merchantUserId)
        {
            this._merchantUserId = merchantUserId;
            return this;
        }

        public NetBankingPaymentV2Instrument Build()
        {
            return new NetBankingPaymentV2Instrument(this._bankId!, this._merchantUserId);
        }
    }
}
