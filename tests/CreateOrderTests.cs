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
public class CreateOrderTests : BaseSetupWithOAuth
{
    private StandardCheckoutClient standardCheckoutClient;
    private const string OrderId = "orderId";

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
    public async Task StandardCheckout_CreateOrder_Successful()
    {
        string url = "/checkout/v2/sdk/order";

        OAuthSetup();

        var createOrderRequest = CreateSdkOrderRequest.StandardCheckoutBuilder()
            .SetAmount(100)
            .SetMerchantOrderId("merchantOrderId")
            .Build();

        var createOrderResponse = new CreateSdkOrderResponse
        {
            OrderId = OrderId,
            State = "PENDING",
            ExpireAt = 238462442,
            Token = "token"
        };

        var headers = GetHeadersForPostReq();

        AddStubForPostRequest(
            url,
            headers,
            createOrderRequest,
            200,
            new Dictionary<string, string>(),
            createOrderResponse
        );

        var actual = await standardCheckoutClient.CreateSdkOrder(createOrderRequest);

        Assert.That(actual.OrderId, Is.EqualTo(OrderId));
        Assert.That(actual.State, Is.EqualTo("PENDING"));
        Assert.That(actual.Token, Is.EqualTo("token"));
    }

    [Test, Order(2)]
    public async Task StandardCheckout_CreateOrder_WithDisablePaymentRetry_True()
    {
        string url = "/checkout/v2/sdk/order";

        OAuthSetup();

        var createOrderRequest = CreateSdkOrderRequest.StandardCheckoutBuilder()
            .SetAmount(100)
            .SetMerchantOrderId("merchantOrderId")
            .SetDisablePaymentRetry(true)
            .Build();

        var createOrderResponse = new CreateSdkOrderResponse
        {
            OrderId = OrderId,
            State = "PENDING",
            ExpireAt = 238462442,
            Token = "token"
        };

        var headers = GetHeadersForPostReq();

        AddStubForPostRequest(
            url,
            headers,
            createOrderRequest,
            200,
            new Dictionary<string, string>(),
            createOrderResponse
        );

        var actual = await standardCheckoutClient.CreateSdkOrder(createOrderRequest);

        Assert.That(actual.OrderId, Is.EqualTo(OrderId));
        Assert.That(actual.State, Is.EqualTo("PENDING"));
        Assert.That(actual.Token, Is.EqualTo("token"));
        Assert.That(createOrderRequest.DisablePaymentRetry, Is.True);
    }

    [Test, Order(3)]
    public async Task StandardCheckout_CreateOrder_WithDisablePaymentRetry_False()
    {
        string url = "/checkout/v2/sdk/order";

        OAuthSetup();

        var createOrderRequest = CreateSdkOrderRequest.StandardCheckoutBuilder()
            .SetAmount(100)
            .SetMerchantOrderId("merchantOrderId")
            .SetDisablePaymentRetry(false)
            .Build();

        var createOrderResponse = new CreateSdkOrderResponse
        {
            OrderId = OrderId,
            State = "PENDING",
            ExpireAt = 238462442,
            Token = "token"
        };

        var headers = GetHeadersForPostReq();

        AddStubForPostRequest(
            url,
            headers,
            createOrderRequest,
            200,
            new Dictionary<string, string>(),
            createOrderResponse
        );

        var actual = await standardCheckoutClient.CreateSdkOrder(createOrderRequest);

        Assert.That(actual.OrderId, Is.EqualTo(OrderId));
        Assert.That(actual.State, Is.EqualTo("PENDING"));
        Assert.That(actual.Token, Is.EqualTo("token"));
        Assert.That(createOrderRequest.DisablePaymentRetry, Is.False);
    }

}
