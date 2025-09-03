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
public class UpiQrPayRequestTests
{
    [Test]
    public void Build_WithValidInputs_ShouldReturnValidPgPaymentRequest()
    {
        var metaInfo = MetaInfo.Builder()
            .SetUdf1("QR Test Info")
            .Build();
            
        var constraints = new List<InstrumentConstraint>();

        var request = new UpiQrRequestBuilder()
            .SetMerchantOrderId("qr-order-789")
            .SetAmount(250)
            .SetMetaInfo(metaInfo)
            .SetConstraints(constraints)
            .SetExpireAfter(300)
            .Build();

        Assert.That(request, Is.Not.Null);
        Assert.That(request.MerchantOrderId, Is.EqualTo("qr-order-789"));
        Assert.That(request.Amount, Is.EqualTo(250));
        Assert.That(request.MetaInfo, Is.EqualTo(metaInfo));
        Assert.That(request.Constraints, Is.EqualTo(constraints));
        Assert.That(request.ExpireAfter, Is.EqualTo(300));

        var paymentFlow = request.PaymentFlow;
        Assert.That(paymentFlow, Is.Not.Null);
        Assert.That(paymentFlow.PaymentMode.Type, Is.EqualTo(PgV2InstrumentType.UPI_QR));
        Assert.That(paymentFlow.PaymentMode, Is.TypeOf<UpiQrPaymentV2Instrument>());
    }
}
