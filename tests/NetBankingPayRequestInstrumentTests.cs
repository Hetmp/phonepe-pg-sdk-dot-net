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

using NUnit.Framework;

namespace pg_sdk_dotnet.tests;

[TestFixture]
public class NetBankingPayRequestBuilderTests
{
    [Test, Order(1)]
    public void Build_WithRequiredFields_ShouldReturnPgPaymentRequest()
    {
        var request =  PgPaymentRequest.NetBankingPayRequestBuilder()
            .SetMerchantOrderId("order123")
            .SetAmount(100)
            .SetBankId("HDFC")
            .Build();

        Assert.That(request, Is.Not.Null);
        Assert.That(request.MerchantOrderId, Is.EqualTo("order123"));
        Assert.That(request.Amount, Is.EqualTo(100));
        Assert.That(request.PaymentFlow.Type, Is.EqualTo(PaymentFlowType.PG));
        Assert.That(request.PaymentFlow.PaymentMode.Type, Is.EqualTo(PgV2InstrumentType.NET_BANKING));

        var mode = request.PaymentFlow.PaymentMode as NetBankingPaymentV2Instrument;
        Assert.That(mode, Is.Not.Null);
        Assert.That(mode!.BankId, Is.EqualTo("HDFC"));
    }

    [Test, Order(2)]
    public void Build_WithOptionalFields_ShouldSetAllProperties()
    {
        var metaInfo = MetaInfo.Builder()
            .SetUdf1("udf1")
            .Build();

        var constraints = new List<InstrumentConstraint>();

        var builder = PgPaymentRequest.NetBankingPayRequestBuilder()
            .SetMerchantOrderId("order000")
            .SetAmount(99)
            .SetBankId("YESBANK")
            .SetMetaInfo(metaInfo)
            .SetConstraints(constraints)
            .SetMerchantUserId("user123")
            .SetRedirectUrl("https://callback.url")
            .SetExpireAfter(300);

        var request = builder.Build();

        Assert.That(request, Is.Not.Null);
        Assert.That(request.MerchantOrderId, Is.EqualTo("order000"));
        Assert.That(request.Amount, Is.EqualTo(99));
        Assert.That(request.MetaInfo, Is.EqualTo(metaInfo));
        Assert.That(request.Constraints, Is.EqualTo(constraints));
        Assert.That(request.ExpireAfter, Is.EqualTo(300));
        Assert.That(request.PaymentFlow.Type, Is.EqualTo(PaymentFlowType.PG));
        Assert.That(request.PaymentFlow.PaymentMode.Type, Is.EqualTo(PgV2InstrumentType.NET_BANKING));

        var paymentFlow = request.PaymentFlow;
        Assert.That(paymentFlow.MerchantUrls?.RedirectUrl, Is.EqualTo("https://callback.url"));

        var mode = paymentFlow.PaymentMode as NetBankingPaymentV2Instrument;
        Assert.That(mode, Is.Not.Null);
        Assert.That(mode!.BankId, Is.EqualTo("YESBANK"));
        Assert.That(mode.MerchantUserId, Is.EqualTo("user123"));

    }
}
