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

public class CardPaymentV2Instrument(
    NewCardDetails cardDetails,
    string? authMode,
    string? merchantUserId,
    bool? saveCard
    ) : PaymentV2Instrument(PgV2InstrumentType.CARD)
{
    public NewCardDetails CardDetails { get; private set;} = cardDetails;

    public string? AuthMode { get; private set;} = authMode;

    public bool? SaveCard { get; private set;} = saveCard;

    public string? MerchantUserId { get; private set;} = merchantUserId;

    public static CardPaymentV2InstrumentBuilder Builder()
    {
        return new CardPaymentV2InstrumentBuilder();
    }

    public class CardPaymentV2InstrumentBuilder
    {
        private NewCardDetails? _cardDetails;
        private string? _authMode;
        private bool? _saveCard;
        private string? _merchantUserId;

        public CardPaymentV2InstrumentBuilder SetCardDetails(NewCardDetails cardDetails)
        {
            this._cardDetails = cardDetails;
            return this;
        }

        public CardPaymentV2InstrumentBuilder SetAuthMode(string? authMode)
        {
            this._authMode = authMode;
            return this;
        }

        public CardPaymentV2InstrumentBuilder SetSaveCard(bool? saveCard)
        {
            this._saveCard = saveCard;
            return this;
        }

        public CardPaymentV2InstrumentBuilder SetMerchantUserId(string? merchantUserId)
        {
            this._merchantUserId = merchantUserId;
            return this;
        }

        public CardPaymentV2Instrument Build()
        {
            return new CardPaymentV2Instrument(
                this._cardDetails!,
                this._authMode,
                this._merchantUserId,
                this._saveCard
            );
        }
    }
}