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
public class UpiIntentPayRequestBuilder
{
    private string _merchantOrderId = null!;
    private long _amount;
    private MetaInfo? _metaInfo;
    private List<InstrumentConstraint>? _constraints;
    private string? _deviceOS;
    private string? _merchantCallBackScheme;
    private string? _targetApp = null;
    private long? _expireAfter;

    /**
    * SETTERS
    */
    public UpiIntentPayRequestBuilder SetMerchantOrderId(string merchantOrderId)
    {
        this._merchantOrderId = merchantOrderId;
        return this;
    }

    public UpiIntentPayRequestBuilder SetAmount(long amount)
    {
        this._amount = amount;
        return this;
    }

    public UpiIntentPayRequestBuilder SetMetaInfo(MetaInfo metaInfo)
    {
        this._metaInfo = metaInfo;
        return this;
    }

    public UpiIntentPayRequestBuilder SetConstraints(List<InstrumentConstraint> constraints)
    {
        this._constraints = constraints;
        return this;
    }

    public UpiIntentPayRequestBuilder SetDeviceOS(string deviceOS)
    {
        this._deviceOS = deviceOS;
        return this;
    }

    public UpiIntentPayRequestBuilder SetMerchantCallBackScheme(string merchantCallBackScheme)
    {
        this._merchantCallBackScheme = merchantCallBackScheme;
        return this;
    }

    public UpiIntentPayRequestBuilder SetTargetApp(string targetApp)
    {
        this._targetApp = targetApp;
        return this;
    }

    public UpiIntentPayRequestBuilder SetExpireAfter(long expireAfter)
    {
        this._expireAfter = expireAfter;
        return this;
    }

    public PgPaymentRequest Build()
    {
        var paymentInstrument = IntentPaymentV2Instrument.Builder()
            .SetTargetApp(this._targetApp)
            .Build();
            
        var paymentFlow = PgPaymentFlow.Builder()
            .SetPaymentMode(paymentInstrument)
            .Build();

        var deviceContext = DeviceContext.Builder()
            .SetDeviceOS(this._deviceOS)
            .SetMerchantCallBackScheme(this._merchantCallBackScheme)
            .Build();

        return new PgPaymentRequest(
            this._merchantOrderId,
            this._amount,
            paymentFlow,
            this._expireAfter,
            this._metaInfo,
            this._constraints,
            deviceContext
        );
    }
}