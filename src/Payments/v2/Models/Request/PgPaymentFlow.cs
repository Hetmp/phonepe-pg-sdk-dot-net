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
public class PgPaymentFlow(
    PaymentV2Instrument paymentMode, 
    MerchantUrls? merchantUrls = null
    ) : PaymentFlow(PaymentFlowType.PG)
{
    public PaymentV2Instrument PaymentMode { get; private set;} = paymentMode;
    public MerchantUrls? MerchantUrls { get; private set;} = merchantUrls;

    public static PgPaymentFlowBuilder Builder() 
    {
        return new PgPaymentFlowBuilder();
    }
}

public class PgPaymentFlowBuilder
{
    private PaymentV2Instrument? _paymentMode;
    private MerchantUrls? _merchantUrls;

    /**
    * SETTERS FOR PG PAYMENT FLOW
    */
    public PgPaymentFlowBuilder SetPaymentMode(PaymentV2Instrument paymentMode)
    {
        this._paymentMode = paymentMode;
        return this;
    }

    public PgPaymentFlowBuilder SetMerchantUrls(MerchantUrls? merchantUrls)
    {
        this._merchantUrls = merchantUrls;
        return this;
    }


    public PgPaymentFlow Build()
    {
        return new PgPaymentFlow(this._paymentMode!, this._merchantUrls);
    }
}
