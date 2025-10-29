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
public class CustomTransactionTests : BaseSetupWithOAuth
{
    private CustomCheckoutClient customCheckoutClient;
    private const string MerchantRefundId = "merchantRefundId";
    
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
    public async Task CustomCheckout_TransactionStatus_Successful()
    {
        OAuthSetup(); 
        var transactionId = "transactionId-success";

        var mockResponse = new
        {
            orderId = "orderId",
            state = "PENDING",
            expireAt = 238462442,
            amount = 100,
            paymentDetails = new[]
            {
                new
                {
                    paymentMode = "UPI_COLLECT",
                    transactionId = transactionId,
                    instrument = new { type = "ACCOUNT", ifsc = "ifsc" }
                }
            }
        };

        var url = $"/payments/v2/transaction/{transactionId}/status";

        var headers = GetHeadersForGetReq();

        AddStubForGetRequest(
            url,
            new Dictionary<string, string>(),
            200,
            headers,
            new Dictionary<string, string>(),
            mockResponse
        );

        var result = await customCheckoutClient.GetTransactionStatus(transactionId);

        Assert.That(result.OrderId, Is.EqualTo("orderId"));
        Assert.That(result.State, Is.EqualTo(mockResponse.state));
        Assert.That(result.Amount, Is.EqualTo(mockResponse.amount));
        Assert.That(result.PaymentDetails.Count, Is.EqualTo(mockResponse.paymentDetails.Length));
    }

    [Test, Order(2)]
    public void CustomCheckout_TransactionStatus_Failed_WrongId()
    {
        OAuthSetup(); 
        var transactionId = "transactionId-wrongId";

        var mockError = new
        {
            code = "MAPPING_NOT_FOUND",
            message = "No entry found for Merchant",
            data = new { }
        };

        var url = $"/payments/v2/transaction/{transactionId}/status";

        var headers = GetHeadersForGetReq();

        AddStubForGetRequest(
            url,
            new Dictionary<string, string>(),
            400,
            headers,
            new Dictionary<string, string>(),
            mockError
        );

        var ex = Assert.ThrowsAsync<BadRequest>(async () => await customCheckoutClient.GetTransactionStatus(transactionId));
        Assert.That(ex.Message, Is.EqualTo("No entry found for Merchant"));
        Assert.That(ex.Code, Is.EqualTo("MAPPING_NOT_FOUND"));
        Assert.That(ex.HttpStatusCode, Is.EqualTo(400));
    }

    [Test, Order(3)]
    public void CustomCheckout_TransactionStatus_Failed_ServerError()
    {
        OAuthSetup(); 
        var transactionId = "transactionId-failed";

        var mockError = new
        {
            code = "INTERNAL_SERVER_ERROR",
            message = "Something went wrong on the server",
            data = new { }
        };

        var url = $"/payments/v2/transaction/{transactionId}/status";

        var headers = GetHeadersForGetReq();

        AddStubForGetRequest(
            url,
            new Dictionary<string, string>(),
            500,
            headers,
            new Dictionary<string, string>(),
            mockError
        );

        var ex = Assert.ThrowsAsync<ServerError>(async () => await customCheckoutClient.GetTransactionStatus(transactionId));
        Assert.That(ex.Message, Is.EqualTo("Something went wrong on the server"));
        Assert.That(ex.Code, Is.EqualTo("INTERNAL_SERVER_ERROR"));
        Assert.That(ex.HttpStatusCode, Is.EqualTo(500));
    }

}
