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

[NonParallelizable]
[TestFixture]
public class PayTest : BaseSetupWithOAuth
{
    private StandardCheckoutClient standardCheckoutClient;

    [SetUp]
    public void Setup()
    {
        var env = Env.TESTING;

        standardCheckoutClient = StandardCheckoutClient.GetInstance(
            ClientId,
            ClientSecret,
            ClientVersion,
            env
        );
    }


    [Test, Order(1)]
    public async Task TestPayReturnSuccess()
    {
        string url = "/checkout/v2/pay";
        string redirectUrl = "https://www.google.com/";

        OAuthSetup();

        var metaInfo = MetaInfo.Builder()
                    .SetUdf1("udf1")
                    .SetUdf2("udf2")
                    .SetUdf3("udf3")
                    .SetUdf4("udf4")
                    .SetUdf5("udf5")
                    .SetUdf6("udf6")
                    .SetUdf7("udf7")
                    .SetUdf8("udf8")
                    .SetUdf9("udf9")
                    .SetUdf10("udf10")
                    .SetUdf11("udf11")
                    .SetUdf12("udf12")
                    .SetUdf13("udf13")
                    .SetUdf14("udf14")
                    .SetUdf15("udf15")
                    .Build();

        var enabledPaymentModes = new List<PaymentModeConstraint>
        {
            new UpiIntentPaymentModeConstraint(),
            new UpiCollectPaymentModeConstraint(),
            new UpiQrPaymentModeConstraint(),
            new NetBankingPaymentModeConstraint(),
            new CardPaymentModeConstraint(new HashSet<CardType> { CardType.DEBIT_CARD }),
            new CardPaymentModeConstraint(new HashSet<CardType> { CardType.CREDIT_CARD })
        };

        var paymentModeConfig = new PaymentModeConfig
        {
            EnabledPaymentModes = enabledPaymentModes,
            DisabledPaymentModes = new List<PaymentModeConstraint>()
        };

        var standardCheckoutPayRequest = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("merchantOrderId")
            .SetAmount(100)
            .SetMetaInfo(metaInfo)
            .SetRedirectUrl(redirectUrl)
            .SetExpireAfter(12)
            .SetPaymentModeConfig(paymentModeConfig)
            .SetMessage("message")
            .Build();

        var standardCheckoutResponse = new StandardCheckoutPayResponse
        {
            OrderId = "orderId",
            State = "PENDING",
            ExpireAt = 300,
            RedirectUrl = "https://google.com"
        };

        var headers = GetHeadersForPostReq();

        AddStubForPostRequest(url, headers, standardCheckoutPayRequest, 200, new Dictionary<string, string>(), standardCheckoutResponse);

        var result = await standardCheckoutClient.Pay(standardCheckoutPayRequest);
        Assert.That(result.OrderId, Is.EqualTo("orderId"));
        Assert.That(result.State, Is.EqualTo("PENDING"));
        Assert.That(result.RedirectUrl, Is.EqualTo("https://google.com"));

    }

    [Test, Order(2)]
    public async Task TestPayBadRequest()
    {
        string url = "/checkout/v2/pay";

        string redirectUrl = "https://www.google.com/";

        OAuthSetup();

        var metaInfo = MetaInfo.Builder()
                   .SetUdf1("udf1")
                   .Build();

        var enabledPaymentModes = new List<PaymentModeConstraint>
        {
            new UpiIntentPaymentModeConstraint(),
            new UpiCollectPaymentModeConstraint(),
            new UpiQrPaymentModeConstraint(),
            new NetBankingPaymentModeConstraint(),
            new CardPaymentModeConstraint(new HashSet<CardType> { CardType.DEBIT_CARD }),
            new CardPaymentModeConstraint(new HashSet<CardType> { CardType.CREDIT_CARD })
        };

        var paymentModeConfig = new PaymentModeConfig
        {
            EnabledPaymentModes = enabledPaymentModes,
            DisabledPaymentModes = new List<PaymentModeConstraint>()
        };

        var standardCheckoutPayRequest = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("merchantOrderId")
            .SetAmount(100)
            .SetMetaInfo(metaInfo)
            .SetRedirectUrl(redirectUrl)
            .SetExpireAfter(12)
            .SetPaymentModeConfig(paymentModeConfig)
            .SetMessage("message")
            .Build();


        var errorResponse = new
        {
            code = "BAD_REQUEST",
            message = "Invalid order ID",
            data = new
            {
                a = "b"
            }
        };

        var headers = GetHeadersForPostReq();

        AddStubForPostRequest(
            url,
            headers,
            standardCheckoutPayRequest,
            400,
            new Dictionary<string, string>(),
            errorResponse
        );

        var ex = Assert.ThrowsAsync<BadRequest>(async () => await standardCheckoutClient.Pay(standardCheckoutPayRequest));

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex.HttpStatusCode, Is.EqualTo(400));
        Assert.That(ex.Code, Is.EqualTo("BAD_REQUEST"));
        Assert.That(ex.Message, Is.EqualTo("Invalid order ID"));
        Assert.That(ex.AdditionalData["a"]?.ToString(), Is.EqualTo("b"));

    }

}
