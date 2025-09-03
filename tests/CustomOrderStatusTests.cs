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
public class CustomOrderStatusTests : BaseSetupWithOAuth
{
    private CustomCheckoutClient customCheckoutClient;
    
    [SetUp] 
    public void Setup()
    {
        var env = Env.TESTING;

        customCheckoutClient = CustomCheckoutClient.GetInstance(
            ClientId,
            ClientSecret,
            ClientVersion,
            env
        );
    }
        

    [Test, Order(1)]
    public async Task OrderStatus_Successful_CustomCheckout()
    {
        OAuthSetup(); 

        var orderId = "test-order-id";
        var url = $"/payments/v2/order/{orderId}/status";

        var mockOrderResponse = new
        {
            orderId = orderId,
            state = "PENDING",
            expireAt = 238462442,
            amount = 100,
            paymentDetails = new[]
            {
                new
                {
                    paymentMode = "UPI_COLLECT",
                    transactionId = "transactionId",
                    instrument = new { type = "ACCOUNT", ifsc = "ifsc" }
                }
            }
        };

        var headers = GetHeadersForGetReq();

        var queryParams = new Dictionary<string, string>
        {
            { "details", "false" }
        };

        AddStubForGetRequest(
            url,
            queryParams,
            200,
            headers,     
            [],    
            mockOrderResponse
        );

        var response = await customCheckoutClient.GetOrderStatus(orderId);

        Assert.That(response.OrderId, Is.EqualTo(orderId));
        Assert.That(response.State, Is.EqualTo("PENDING"));
        Assert.That(response.PaymentDetails.Count, Is.EqualTo(1));
    }
}
