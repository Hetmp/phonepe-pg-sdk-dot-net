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
public class PaymentRailTests : BaseSetupWithOAuth
{
    [Test, Order(1)]
    public void PaymentRail_ShouldInitializeAndSetProperties()
    {
        var upiPaymentRail = new UpiPaymentRail
        {
            Type = PaymentRailType.UPI,
            Utr = "UTR123456",
            UpiTransactionId = "UPITXN7890",
            Vpa = "user@bank"
        };

        Assert.That(upiPaymentRail.Type, Is.EqualTo(PaymentRailType.UPI));
        Assert.That(upiPaymentRail.Utr, Is.EqualTo("UTR123456"));
        Assert.That(upiPaymentRail.UpiTransactionId, Is.EqualTo("UPITXN7890"));
        Assert.That(upiPaymentRail.Vpa, Is.EqualTo("user@bank"));
    }

    [Test, Order(2)]
    public void PpiEgvPaymentRail_ShouldInitializeAndSetProperties()
    {
        var ppiEgvPaymentRail = new PpiEgvPaymentRail
        {
            Type = PaymentRailType.PPI_EGV
        };

        Assert.That(ppiEgvPaymentRail.Type, Is.EqualTo(PaymentRailType.PPI_EGV));
    }

    [Test, Order(3)]
    public void PpiWalletPaymentRail_ShouldInitializeAndSetProperties()
    {
        var ppiWalletPaymentRail = new PpiWalletPaymentRail
        {
            Type = PaymentRailType.PPI_WALLET
        };

        Assert.That(ppiWalletPaymentRail.Type, Is.EqualTo(PaymentRailType.PPI_WALLET));
    }

    [Test, Order(4)]
    public void PgPaymentRail_ShouldInitializeAndSetProperties()
    {
    var pgPaymentrail = new PgPaymentRail
    {
        Type = PaymentRailType.PG,
        TransactionId = "TRID123",
        AuthorizationCode = "123",
        ServiceTransactionId = "123"
    };

        Assert.That(pgPaymentrail.Type, Is.EqualTo(PaymentRailType.PG));
        Assert.That(pgPaymentrail.TransactionId, Is.EqualTo("TRID123"));
        Assert.That(pgPaymentrail.AuthorizationCode, Is.EqualTo("123"));
        Assert.That(pgPaymentrail.ServiceTransactionId, Is.EqualTo("123"));
    }

    [Test, Order(5)]
    public void Deserialize_UpiPaymentRail_WithCustomConverter()
    {
        var json = """
        {
            "type": "UPI",
            "utr": "UTR123456",
            "upiTransactionId": "UPITXN7890",
            "vpa": "user@bank"
        }
        """;

        var result = JsonSerializer.Deserialize<PaymentRail>(json, JsonOptions.PaymentRailDeserialization);

        Assert.That(result, Is.InstanceOf<UpiPaymentRail>());
        var upi = (UpiPaymentRail)result!;
        Assert.That(upi.Type, Is.EqualTo(PaymentRailType.UPI));
        Assert.That(upi.Utr, Is.EqualTo("UTR123456"));
        Assert.That(upi.UpiTransactionId, Is.EqualTo("UPITXN7890"));
        Assert.That(upi.Vpa, Is.EqualTo("user@bank"));
    }

    [Test, Order(6)]
    public void Deserialize_InvalidType_ThrowsJsonException()
    {
        var json = """
        {
            "type": "INVALID_TYPE"
        }
        """;

        Assert.Throws<JsonException>(() =>
        {
            JsonSerializer.Deserialize<PaymentRail>(json, JsonOptions.PaymentRailDeserialization);
        });
    }

}