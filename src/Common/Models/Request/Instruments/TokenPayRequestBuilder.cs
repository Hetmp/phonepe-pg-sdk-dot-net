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

public class TokenPayRequestBuilder
{
    private string _merchantOrderId = null!;
    private long _amount;
    private long _encryptionKeyId;
    private string _encryptedToken = null!;
    private string _encryptedCvv = null!;
    private string _cryptogram = null!;
    private string _panSuffix = null!;
    private string _expiryMonth = null!;
    private string _expiryYear = null!;
    private string? _authMode;
    private string? _redirectUrl;
    private string? _cardHolderName;
    private string? _merchantUserId;
    private MetaInfo? _metaInfo;
    private List<InstrumentConstraint>? _constraints;
    private long? _expireAfter;

    /**
        * SETTERS
        */

    public TokenPayRequestBuilder SetMerchantOrderId(string merchantOrderId)
    {
        this._merchantOrderId = merchantOrderId;
        return this;
    }

    public TokenPayRequestBuilder SetAmount(long amount)
    {
        this._amount = amount;
        return this;
    }

    public TokenPayRequestBuilder SetMetaInfo(MetaInfo metaInfo)
    {
        this._metaInfo = metaInfo;
        return this;
    }

    public TokenPayRequestBuilder SetConstraints(List<InstrumentConstraint> constraints)
    {
        this._constraints = constraints;
        return this;
    }

    public TokenPayRequestBuilder SetEncryptedToken(string encryptedToken)
    {
        this._encryptedToken = encryptedToken;
        return this;
    }

    public TokenPayRequestBuilder SetAuthMode(string? authMode)
    {
        this._authMode = authMode;
        return this;
    }

    public TokenPayRequestBuilder SetEncryptionKeyId(long encryptionKeyId)
    {
        this._encryptionKeyId = encryptionKeyId;
        return this;
    }

    public TokenPayRequestBuilder SetPanSuffix(string panSuffix)
    {
        this._panSuffix = panSuffix;
        return this;
    }

    public TokenPayRequestBuilder SetCryptogram(string cryptogram)
    {
        this._cryptogram = cryptogram;
        return this;
    }

    public TokenPayRequestBuilder SetEncryptedCvv(string encryptedCvv)
    {
        this._encryptedCvv = encryptedCvv;
        return this;
    }

    public TokenPayRequestBuilder SetExpiryMonth(string expiryMonth)
    {
        this._expiryMonth = expiryMonth;
        return this;
    }

    public TokenPayRequestBuilder SetExpiryYear(string expiryYear)
    {
        this._expiryYear = expiryYear;
        return this;
    }

    public TokenPayRequestBuilder SetRedirectUrl(string? redirectUrl)
    {
        this._redirectUrl = redirectUrl;
        return this;
    }

    public TokenPayRequestBuilder SetCardHolderName(string? cardHolderName)
    {
        this._cardHolderName = cardHolderName;
        return this;
    }

    public TokenPayRequestBuilder SetMerchantUserId(string? merchantUserId)
    {
        this._merchantUserId = merchantUserId;
        return this;
    }

    public TokenPayRequestBuilder SetExpireAfter(long? expireAfter)
    {
        this._expireAfter = expireAfter;
        return this;
    }

    public PgPaymentRequest Build()
    {
        var expiry = Expiry.Builder()
            .SetExpiryMonth(this._expiryMonth)
            .SetExpiryYear(this._expiryYear)
            .Build();

        var tokenDetails = TokenDetails.Builder()
            .SetEncryptedToken(this._encryptedToken)
            .SetEncryptedCvv(this._encryptedCvv)
            .SetEncryptionKeyId(this._encryptionKeyId)
            .SetExpiry(expiry)
            .SetCryptogram(this._cryptogram)
            .SetPanSuffix(this._panSuffix)
            .SetCardHolderName(this._cardHolderName)
            .Build();

        var paymentInstrument = TokenPaymentV2Instrument.Builder()
            .SetTokenDetails(tokenDetails)
            .SetAuthMode(this._authMode)
            .SetMerchantUserId(this._merchantUserId)
            .Build();

        var merchantUrls = MerchantUrls.Builder()
            .SetRedirectUrl(this._redirectUrl)
            .Build();

        var paymentFlow = PgPaymentFlow.Builder()
            .SetPaymentMode(paymentInstrument)
            .SetMerchantUrls(merchantUrls)
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