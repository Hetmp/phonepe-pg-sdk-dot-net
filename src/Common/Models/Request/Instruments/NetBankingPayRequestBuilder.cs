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
public class NetBankingPayRequestBuilder
{
    private string _merchantOrderId = null!;
    private long _amount;
    private List<InstrumentConstraint>? _constraints;
    private MetaInfo? _metaInfo;
    private string _bankId = null!;
    private string? _merchantUserId;
    private string? _redirectUrl;
    private long? _expireAfter;

    /**
    * SETTERS
    */

    public NetBankingPayRequestBuilder SetMerchantOrderId(string merchantOrderId)
    {
        this._merchantOrderId = merchantOrderId;
        return this;
    }

    public NetBankingPayRequestBuilder SetAmount(long amount)
    {
        this._amount = amount;
        return this;
    }

    public NetBankingPayRequestBuilder SetMetaInfo(MetaInfo metaInfo)
    {
        this._metaInfo = metaInfo;
        return this;
    }

    public NetBankingPayRequestBuilder SetConstraints(List<InstrumentConstraint> constraints)
    {
        this._constraints = constraints;
        return this;
    }

    public NetBankingPayRequestBuilder SetBankId(string bankId)
    {
        this._bankId = bankId;
        return this;
    }

    public NetBankingPayRequestBuilder SetMerchantUserId(string merchantUserId)
    {
        this._merchantUserId = merchantUserId;
        return this;
    }

    public NetBankingPayRequestBuilder SetRedirectUrl(string redirectUrl)
    {
        this._redirectUrl = redirectUrl;
        return this;
    }

    public NetBankingPayRequestBuilder SetExpireAfter(long expireAfter)
    {
        this._expireAfter = expireAfter;
        return this;
    }

    public PgPaymentRequest Build()
    {
        var paymentInstrument = NetBankingPaymentV2Instrument.Builder()
            .SetBankId(this._bankId)
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