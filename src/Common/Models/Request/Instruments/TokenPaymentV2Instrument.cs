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

public class TokenPaymentV2Instrument(
    TokenDetails tokenDetails,
    string? authMode = null,
    string? merchantUserId = null) : PaymentV2Instrument(PgV2InstrumentType.TOKEN)
{
    public TokenDetails TokenDetails { get; private set;} = tokenDetails;
    public string? AuthMode { get; private set;} = authMode;
    public string? MerchantUserId { get; private set;} = merchantUserId;

    public static TokenPaymentV2InstrumentBuilder Builder()
    {
        return new TokenPaymentV2InstrumentBuilder();
    }

    public class TokenPaymentV2InstrumentBuilder
    {
        private TokenDetails? _tokenDetails;
        private string? _authMode;
        private string? _merchantUserId;

        public TokenPaymentV2InstrumentBuilder SetTokenDetails(TokenDetails tokenDetails)
        {
            this._tokenDetails = tokenDetails;
            return this;
        }

        public TokenPaymentV2InstrumentBuilder SetAuthMode(string? authMode)
        {
            this._authMode = authMode;
            return this;
        }

        public TokenPaymentV2InstrumentBuilder SetMerchantUserId(string? merchantUserId)
        {
            this._merchantUserId = merchantUserId;
            return this;
        }

        public TokenPaymentV2Instrument Build()
        {
            return new TokenPaymentV2Instrument(this._tokenDetails!, this._authMode, this._merchantUserId);
        }
    }
}

