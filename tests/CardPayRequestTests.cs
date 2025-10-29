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
public class CardPayRequestBuilderTests
{
    [Test]
    public void Build_WithRequiredFields_ShouldReturnPgPaymentRequest()
    {
        var request = new CardPayRequestBuilder()
            .SetMerchantOrderId("card_order_001")
            .SetAmount(500)
            .SetEncryptionKeyId(123)
            .SetEncryptedCardNumber("encCard123")
            .SetEncryptedCvv("encCvv123")
            .SetExpiryMonth("12")
            .SetExpiryYear("2026")
            .Build();

        Assert.That(request, Is.Not.Null);
        Assert.That(request.MerchantOrderId, Is.EqualTo("card_order_001"));
        Assert.That(request.Amount, Is.EqualTo(500));
        Assert.That(request.PaymentFlow.PaymentMode.Type, Is.EqualTo(PgV2InstrumentType.CARD));
    }

    [Test]
    public void Build_WithOptionalFields_ShouldSetAllProperties()
    {
        var metaInfo = MetaInfo.Builder().SetUdf1("meta").Build();
        var constraints = new List<InstrumentConstraint>();

        var request = new CardPayRequestBuilder()
            .SetMerchantOrderId("card_order_002")
            .SetAmount(1234)
            .SetEncryptionKeyId(321)
            .SetEncryptedCardNumber("number123")
            .SetEncryptedCvv("cvv321")
            .SetExpiryMonth("11")
            .SetExpiryYear("2030")
            .SetCardHolderName("John Doe")
            .SetAuthMode("PIN")
            .SetMerchantUserId("merchantX")
            .SetSaveCard(true)
            .SetMetaInfo(metaInfo)
            .SetRedirectUrl("https://callback.url")
            .SetConstraints(constraints)
            .SetExpireAfter(1200)
            .Build();

        Assert.That(request, Is.Not.Null);
        Assert.That(request.MerchantOrderId, Is.EqualTo("card_order_002"));
        Assert.That(request.Amount, Is.EqualTo(1234));
        Assert.That(request.MetaInfo, Is.EqualTo(metaInfo));
        Assert.That(request.Constraints, Is.EqualTo(constraints));
        Assert.That(request.ExpireAfter, Is.EqualTo(1200));

        var paymentFlow = request.PaymentFlow;
        Assert.That(paymentFlow.MerchantUrls?.RedirectUrl, Is.EqualTo("https://callback.url"));

        var cardInstrument = paymentFlow.PaymentMode as CardPaymentV2Instrument;
        Assert.That(cardInstrument, Is.Not.Null);
        Assert.That(cardInstrument?.AuthMode, Is.EqualTo("PIN"));
        Assert.That(cardInstrument?.MerchantUserId, Is.EqualTo("merchantX"));
        Assert.That(cardInstrument?.SaveCard, Is.True);
        Assert.That(cardInstrument?.CardDetails.CardHolderName, Is.EqualTo("John Doe"));
    }
}
