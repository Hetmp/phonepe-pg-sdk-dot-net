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

namespace pg_sdk_dotnet.Payments.v2.Models.Request;
public class PgCheckoutPaymentFlow(
    string? message = null,
    MerchantUrls? merchantUrls = null,
    PaymentModeConfig? paymentModeConfig = null) : PaymentFlow(PaymentFlowType.PG_CHECKOUT)
{
    public string? Message { get; private set; } = message;
    public MerchantUrls? MerchantUrls { get; private set; } = merchantUrls;
    public PaymentModeConfig? PaymentModeConfig { get; private set; } = paymentModeConfig;

    public static PgCheckoutPaymentFlowBuilder Builder()
    {
        return new PgCheckoutPaymentFlowBuilder();
    }
}

public class PgCheckoutPaymentFlowBuilder
{
    private string? _message;
    private MerchantUrls? _merchantUrls;
    private PaymentModeConfig? _paymentModeConfig;

    /**
    * SETTERS FOR PG_CHECKOUT PAYMENT FLOW
    */
    public PgCheckoutPaymentFlowBuilder SetMessage(string? message)
    {
        this._message = message;
        return this;
    }

    public PgCheckoutPaymentFlowBuilder SetMerchantUrls(MerchantUrls? merchantUrls)
    {
        this._merchantUrls = merchantUrls;
        return this;
    }
    
    public PgCheckoutPaymentFlowBuilder SetPaymentModeConfig(PaymentModeConfig? paymentModeConfig)
    {
        this._paymentModeConfig = paymentModeConfig;
        return this;
    }

    public PgCheckoutPaymentFlow Build()
    {
        return new PgCheckoutPaymentFlow(this._message, this._merchantUrls, this._paymentModeConfig);
    }
}
