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
public class TokenPayRequestBuilderTest
{
    [Test, Order(1)]
    public void TokenDetails_Build_WithAllFields_Succeeds()
    {
        var expiry = Expiry.Builder()
            .SetExpiryMonth("12")
            .SetExpiryYear("2029")
            .Build();

        var tokenDetails = TokenDetails.Builder()
            .SetEncryptedToken("abc123")
            .SetEncryptedCvv("123")
            .SetEncryptionKeyId(1)
            .SetExpiry(expiry)
            .SetCryptogram("xyz456")
            .SetPanSuffix("1234")
            .SetCardHolderName("John Doe")
            .SetEci("05")
            .SetAtc("01")
            .Build();

        Assert.That(tokenDetails.EncryptedToken, Is.EqualTo("abc123"));
        Assert.That(tokenDetails.EncryptedCvv, Is.EqualTo("123"));
        Assert.That(tokenDetails.EncryptionKeyId, Is.EqualTo(1));
        Assert.That(tokenDetails.Expiry, Is.EqualTo(expiry));
        Assert.That(tokenDetails.Cryptogram, Is.EqualTo("xyz456"));
        Assert.That(tokenDetails.PanSuffix, Is.EqualTo("1234"));
        Assert.That(tokenDetails.CardHolderName, Is.EqualTo("John Doe"));
        Assert.That(tokenDetails.Eci, Is.EqualTo("05"));
        Assert.That(tokenDetails.Atc, Is.EqualTo("01"));
    }

    [Test, Order(2)]
    public void TokenPaymentV2Instrument_Build_WithValidTokenDetails_Succeeds()
    {
        var expiry = Expiry.Builder().SetExpiryMonth("01").SetExpiryYear("2028").Build();

        var tokenDetails = TokenDetails.Builder()
            .SetEncryptedToken("token123")
            .SetEncryptedCvv("cvv")
            .SetEncryptionKeyId(42)
            .SetExpiry(expiry)
            .SetCryptogram("crypt")
            .SetPanSuffix("4321")
            .Build();

        var instrument = TokenPaymentV2Instrument.Builder()
            .SetTokenDetails(tokenDetails)
            .SetAuthMode("PIN")
            .SetMerchantUserId("user123")
            .Build();

        Assert.That(instrument.TokenDetails, Is.Not.Null);
        Assert.That(instrument.AuthMode, Is.EqualTo("PIN"));
        Assert.That(instrument.MerchantUserId, Is.EqualTo("user123"));
    }

    [Test, Order(3)]
    public void TokenPayRequestBuilder_Build_WithAllFields_Succeeds()
    {
        var request = new TokenPayRequestBuilder()
            .SetMerchantOrderId("order123")
            .SetAmount(100)
            .SetEncryptionKeyId(1)
            .SetEncryptedToken("abc123")
            .SetEncryptedCvv("123")
            .SetCryptogram("xyz")
            .SetPanSuffix("1234")
            .SetExpiryMonth("08")
            .SetExpiryYear("2027")
            .SetAuthMode("PIN")
            .SetCardHolderName("Jane Doe")
            .SetMerchantUserId("user123")
            .Build();

        Assert.That(request.MerchantOrderId, Is.EqualTo("order123"));
        Assert.That(request.Amount, Is.EqualTo(100.0));
        Assert.That(request.PaymentFlow.PaymentMode, Is.TypeOf<TokenPaymentV2Instrument>());
    }
}
