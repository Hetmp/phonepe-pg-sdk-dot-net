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

public class UpiQrRequestBuilder
{
    private string _merchantOrderId = null!;
    private long _amount;
    private MetaInfo? _metaInfo;
    private List<InstrumentConstraint>? _constraints;
    private long? _expireAfter;

    /**
    * SETTERS
    */

    public UpiQrRequestBuilder SetMerchantOrderId(string merchantOrderId)
    {
        this._merchantOrderId = merchantOrderId;
        return this;
    }

    public UpiQrRequestBuilder SetAmount(long amount)
    {
        this._amount = amount;
        return this;
    }

    public UpiQrRequestBuilder SetMetaInfo(MetaInfo metaInfo)
    {
        this._metaInfo = metaInfo;
        return this;
    }

    public UpiQrRequestBuilder SetConstraints(List<InstrumentConstraint> constraints)
    {
        this._constraints = constraints;
        return this;
    }

    public UpiQrRequestBuilder SetExpireAfter(long? expireAfter)
    {
        this._expireAfter = expireAfter;
        return this;
    }

    public PgPaymentRequest Build()
    {
        var paymentFlow = PgPaymentFlow.Builder()
            .SetPaymentMode(UpiQrPaymentV2Instrument.Builder().Build())
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