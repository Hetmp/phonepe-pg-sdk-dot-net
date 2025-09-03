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

using System.Text.Json;
using NUnit.Framework;

namespace pg_sdk_dotnet.tests;

[NonParallelizable]
[TestFixture]
public class PaymentModeConstraintTests : BaseSetupWithOAuth
{
    
    [Test, Order(1)]
    public void Should_Deserialize_CardPaymentModeConstraint()
    {
        var json = @"{""type"":""CARD"",""cardNetworks"":[""VISA"",""MASTERCARD""]}";
        var result = JsonSerializer.Deserialize<PaymentModeConstraint>(json, JsonOptions.PaymentModeConstraintDeserialization);
        Assert.That(result, Is.TypeOf<CardPaymentModeConstraint>());
    }

    [Test, Order(2)]
    public void Should_Deserialize_NetBankingPaymentModeConstraint()
    {
        var json = @"{""type"":""NET_BANKING"",""allowedBanks"":[""HDFC"",""ICICI""]}";
        var result = JsonSerializer.Deserialize<PaymentModeConstraint>(json, JsonOptions.PaymentModeConstraintDeserialization);
        Assert.That(result, Is.TypeOf<NetBankingPaymentModeConstraint>());
    }

    [Test, Order(3)]
    public void Should_Deserialize_UpiIntentPaymentModeConstraint()
    {
        var json = @"{""type"":""UPI_INTENT"",""allowedApps"":[""GPay"",""PhonePe""]}";
        var result = JsonSerializer.Deserialize<PaymentModeConstraint>(json, JsonOptions.PaymentModeConstraintDeserialization);
        Assert.That(result, Is.TypeOf<UpiIntentPaymentModeConstraint>());
    }

    [Test, Order(4)]
    public void Should_Deserialize_UpiQrPaymentModeConstraint()
    {
        var json = @"{""type"":""UPI_QR"",""qrCodeType"":""STATIC""}";
        var result = JsonSerializer.Deserialize<PaymentModeConstraint>(json, JsonOptions.PaymentModeConstraintDeserialization);
        Assert.That(result, Is.TypeOf<UpiQrPaymentModeConstraint>());
    }

    [Test, Order(5)]
    public void Should_Deserialize_UpiCollectPaymentModeConstraint()
    {
        var json = @"{""type"":""UPI_COLLECT"",""upiHandles"":[""user@upi""]}";
        var result = JsonSerializer.Deserialize<PaymentModeConstraint>(json, JsonOptions.PaymentModeConstraintDeserialization);
        Assert.That(result, Is.TypeOf<UpiCollectPaymentModeConstraint>());
    }

    [Test, Order(6)]
    public void Should_ThrowException_When_TypeIsUnknown()
    {
        var json = @"{""type"":""UNKNOWN_TYPE"",""someField"":""value""}";
        var ex = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<PaymentModeConstraint>(json, JsonOptions.PaymentModeConstraintDeserialization));
        Assert.That(ex.Message, Does.Contain("unknown type"));
    }
}

