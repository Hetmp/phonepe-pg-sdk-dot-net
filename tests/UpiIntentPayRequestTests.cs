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
public class UpiIntentPayRequestBuilderTests
{
    [Test, Order(1)]
    public void Build_WithRequiredFields_ShouldReturnPgPaymentRequest()
    {
        var request = PgPaymentRequest.UpiIntentPayRequestBuilder()
            .SetMerchantOrderId("order-intent-1")
            .SetAmount(250)
            .SetTargetApp("PHONEPE")
            .Build();

        Assert.That(request, Is.Not.Null);
        Assert.That(request.MerchantOrderId, Is.EqualTo("order-intent-1"));
        Assert.That(request.Amount, Is.EqualTo(250));
        Assert.That(request.PaymentFlow.Type, Is.EqualTo(PaymentFlowType.PG));
        Assert.That(request.PaymentFlow.PaymentMode.Type, Is.EqualTo(PgV2InstrumentType.UPI_INTENT));

        var intentInstrument = request.PaymentFlow.PaymentMode as IntentPaymentV2Instrument;
        Assert.That(intentInstrument, Is.Not.Null);
        Assert.That(intentInstrument!.TargetApp, Is.EqualTo("PHONEPE"));
    }

    [Test, Order(2)]
    public void Build_WithOptionalFields_ShouldIncludeMetaInfoConstraintsDeviceContext()
    {
        var metaInfo = MetaInfo.Builder()
            .SetUdf1("udf1")
            .Build();

        var constraints = new List<InstrumentConstraint>();

        var builder = PgPaymentRequest.UpiIntentPayRequestBuilder()
            .SetMerchantOrderId("order-intent-2")
            .SetAmount(199)
            .SetMetaInfo(metaInfo)
            .SetConstraints(constraints)
            .SetTargetApp("PHONEPE")
            .SetDeviceOS("iOS")
            .SetMerchantCallBackScheme("merchantapp")
            .SetExpireAfter(180);

        var result = builder.Build();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.MerchantOrderId, Is.EqualTo("order-intent-2"));
        Assert.That(result.Amount, Is.EqualTo(199));
        Assert.That(result.MetaInfo, Is.EqualTo(metaInfo));
        Assert.That(result.Constraints, Is.EqualTo(constraints));
        Assert.That(result.ExpireAfter, Is.EqualTo(180));
        Assert.That(result.DeviceContext, Is.Not.Null);
        Assert.That(result.DeviceContext!.DeviceOS, Is.EqualTo("iOS"));
        Assert.That(result.DeviceContext.MerchantCallBackScheme, Is.EqualTo("merchantapp"));
        Assert.That(result.PaymentFlow.PaymentMode.Type, Is.EqualTo(PgV2InstrumentType.UPI_INTENT));

        var instrument = result.PaymentFlow.PaymentMode as IntentPaymentV2Instrument;
        Assert.That(instrument, Is.Not.Null);
        Assert.That(instrument!.TargetApp, Is.EqualTo("PHONEPE"));
    }
}