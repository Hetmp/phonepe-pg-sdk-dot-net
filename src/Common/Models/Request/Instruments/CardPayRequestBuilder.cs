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
public class CardPayRequestBuilder
{
    private string _merchantOrderId = null!;
    private long _amount;
    private long _encryptionKeyId;
    private string _encryptedCardNumber = null!;
    private string _encryptedCvv = null!;
    private string _expiryMonth = null!;
    private string _expiryYear = null!;
    private string? _authMode;
    private string? _cardHolderName;
    private string? _merchantUserId;
    private MetaInfo? _metaInfo;
    private string? _redirectUrl;
    private bool? _saveCard;
    private List<InstrumentConstraint>? _constraints;
    private long? _expireAfter;

    /**
    * SETTERS
    */
    public CardPayRequestBuilder SetMerchantOrderId(string merchantOrderId)
    {
        this._merchantOrderId = merchantOrderId;
        return this;
    }

    public CardPayRequestBuilder SetAmount(long amount)
    {
        this._amount = amount;
        return this;
    }

    public CardPayRequestBuilder SetMetaInfo(MetaInfo metaInfo)
    {
        this._metaInfo = metaInfo;
        return this;
    }

    public CardPayRequestBuilder SetConstraints(List<InstrumentConstraint> constraints)
    {
        this._constraints = constraints;
        return this;
    }

    public CardPayRequestBuilder SetEncryptedCardNumber(string encryptedCardNumber)
    {
        this._encryptedCardNumber = encryptedCardNumber;
        return this;
    }

    public CardPayRequestBuilder SetAuthMode(string authMode)
    {
        this._authMode = authMode;
        return this;
    }

    public CardPayRequestBuilder SetEncryptionKeyId(long encryptionKeyId)
    {
        this._encryptionKeyId = encryptionKeyId;
        return this;
    }

    public CardPayRequestBuilder SetEncryptedCvv(string encryptedCvv)
    {
        this._encryptedCvv = encryptedCvv;
        return this;
    }

    public CardPayRequestBuilder SetExpiryMonth(string expiryMonth)
    {
        this._expiryMonth = expiryMonth;
        return this;
    }

    public CardPayRequestBuilder SetExpiryYear(string expiryYear)
    {
        this._expiryYear = expiryYear;
        return this;
    }

    public CardPayRequestBuilder SetRedirectUrl(string redirectUrl)
    {
        this._redirectUrl = redirectUrl;
        return this;
    }

    public CardPayRequestBuilder SetCardHolderName(string cardHolderName)
    {
        this._cardHolderName = cardHolderName;
        return this;
    }

    public CardPayRequestBuilder SetSaveCard(bool saveCard)
    {
        this._saveCard = saveCard;
        return this;
    }

    public CardPayRequestBuilder SetMerchantUserId(string merchantUserId)
    {
        this._merchantUserId = merchantUserId;
        return this;
    }

    public CardPayRequestBuilder SetExpireAfter(long expireAfter)
    {
        this._expireAfter = expireAfter;
        return this;
    }

    public PgPaymentRequest Build()
    {
        NewCardDetails newCardDetails = NewCardDetails.Builder()
            .SetCardHolderName(this._cardHolderName)
            .SetExpiry(
                Expiry.Builder()
                    .SetExpiryMonth(this._expiryMonth)
                    .SetExpiryYear(this._expiryYear)
                    .Build()
            )
            .SetEncryptionKeyId(this._encryptionKeyId)
            .SetEncryptedCardNumber(this._encryptedCardNumber)
            .SetEncryptedCvv(this._encryptedCvv)
            .Build();

        var _cardDetails = CardPaymentV2Instrument.Builder()
            .SetAuthMode(this._authMode)
            .SetMerchantUserId(this._merchantUserId)
            .SetSaveCard(this._saveCard)
            .SetCardDetails(newCardDetails)
            .Build();

        var merchantUrls = MerchantUrls.Builder()
            .SetRedirectUrl(this._redirectUrl)
            .Build();

        PgPaymentFlow paymentFlow = PgPaymentFlow.Builder()
            .SetMerchantUrls(merchantUrls)
            .SetPaymentMode(_cardDetails) 
            .Build();

        return new PgPaymentRequest(
            this._merchantOrderId,
            this._amount,
            paymentFlow,
            this._expireAfter,
            this._metaInfo,
            this._constraints
        );
    }
}