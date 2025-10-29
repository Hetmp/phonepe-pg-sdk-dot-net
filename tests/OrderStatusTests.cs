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
public class OrderStatusTests : BaseSetupWithOAuth
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
    public async Task OrderStatus_Successful_StandardCheckout()
    {
        OAuthSetup();

        var orderId = "test-order-id";
        var url = $"/checkout/v2/order/{orderId}/status";

        var metaInfo = new MetaInfo("udf1", "udf2", "udf3", "udf4", "udf5", "udf6", "udf7", "udf8", "udf9", "udf10", "udf11", "udf12", "udf13", "udf14", "udf15");

        var mockOrderResponse = new
        {
            merchantOrderId = "merchantOrderId",
            merchantId = "merchantId",
            orderId = orderId,
            state = "PENDING",
            payableAmount = 100,
            feeAmount = 100,
            expireAt = 238462442,
            amount = 100,
            metaInfo = metaInfo,
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

        var response = await standardCheckoutClient.GetOrderStatus(orderId);

        Assert.That(response.OrderId, Is.EqualTo(orderId));
        Assert.That(response.State, Is.EqualTo("PENDING"));
        Assert.That(response.PaymentDetails.Count, Is.EqualTo(1));
    }

    [Test, Order(2)]
    public async Task OrderStatus_MultiplePaymentTries_StandardCheckout()
    {
        OAuthSetup();

        var orderId = "test-order-id-multiple";
        var url = $"/checkout/v2/order/{orderId}/status";

        var mockOrderResponse = new
        {
            orderId = orderId,
            state = "COMPLETED",
            expireAt = 238462442,
            amount = 100,
            errorCode = "errorCode",
            detailedErrorCode = "detailedErrorCode",
            paymentDetails = new object[]
            {
                new
                {
                    paymentMode = "UPI_COLLECT",
                    transactionId = "transactionId1",
                    errorCode = "errorCode",
                    detailedErrorCode = "detailedErrorCode",
                    instrument = new { type = "ACCOUNT", ifsc = "ifsc" }
                },
                new
                {
                    paymentMode = "UPI_INTENT",
                    transactionId = "transactionId2",
                    state = "COMPLETED",
                    instrument = new { type = "ACCOUNT", accountHolderName = "name" },
                    rail = new { type = "PG", authorizationCode = "code", transactionId = "txn123", serviceTransactionId = "serviceTxn456" }
                }
            }
        };

        var headers = GetHeadersForGetReq();

        var queryParams = new Dictionary<string, string>
        {
            { "details", "true" }
        };

        AddStubForGetRequest(
            url,
            queryParams,
            200,
            headers,
            new Dictionary<string, string>(),
            mockOrderResponse
        );

        var response = await standardCheckoutClient.GetOrderStatus(orderId, true);

        Assert.That(response.PaymentDetails.Count, Is.EqualTo(2));
        Assert.That(response.PaymentDetails[0].PaymentMode.ToString(), Is.EqualTo("UPI_COLLECT"));
        Assert.That(response.PaymentDetails[1].PaymentMode.ToString(), Is.EqualTo("UPI_INTENT"));
    }

    [Test, Order(3)]
    public async Task OrderStatus_SplitInstruments_StandardCheckout()
    {
        OAuthSetup();

        var orderId = "test-order-id-split";
        var url = $"/checkout/v2/order/{orderId}/status";

        var mockOrderResponse = new
        {
            orderId = orderId,
            state = "COMPLETED",
            expireAt = 238462442,
            amount = 100,
            paymentDetails = new[]
            {
                new
                {
                    paymentMode = "UPI_COLLECT",
                    transactionId = "transactionId",
                    errorCode = "errorCode",
                    detailedErrorCode = "detailedErrorCode",
                    instrument = new { type = "ACCOUNT", ifsc = "ifsc" },
                    splitInstruments = new[]
                    {
                        new
                        {
                            instrument = new { type = "ACCOUNT", accountHolderName = "name" },
                            rail = new { type = "PG", authorizationCode = "code", transactionId = "txn123", serviceTransactionId = "serviceTxn456" },
                            amount = 100
                        }
                    }
                }
            }
        };

        var headers = GetHeadersForGetReq();

        var queryParams = new Dictionary<string, string>
        {
            { "details", "true" }
        };

        AddStubForGetRequest(
            url,
            queryParams,
            200,
            headers,
            new Dictionary<string, string>(),
            mockOrderResponse
        );

        var result = await standardCheckoutClient.GetOrderStatus(orderId, true);

        Assert.That(result.OrderId, Is.EqualTo(orderId));
        Assert.That(result.PaymentDetails.Count, Is.EqualTo(1));
        Assert.That(result.PaymentDetails[0].SplitInstruments, Is.Not.Null);
        Assert.That(result.PaymentDetails[0].SplitInstruments[0].Instrument.Type.ToString(), Is.EqualTo("ACCOUNT"));
        Assert.That(result.PaymentDetails[0].SplitInstruments[0].Rail.Type.ToString(), Is.EqualTo("PG"));
        Assert.That(result.PaymentDetails[0].SplitInstruments[0].Amount, Is.EqualTo(100));
    }

    [Test, Order(4)]
    public async Task OrderStatus_Failed_WrongOrderID_StandardCheckout()
    {
        OAuthSetup();

        var orderId = "wrong-order-id";
        var url = $"/checkout/v2/order/{orderId}/status";

        var mockError = new
        {
            code = "MERCHANT_ORDER_MAPPING_NOT_FOUND",
            message = "No entry found for Merchant"
        };

        var headers = GetHeadersForGetReq();

        var queryParams = new Dictionary<string, string>
        {
            { "details", "false" }
        };

        AddStubForGetRequest(
            url,
            queryParams,
            400,
            headers,
            new Dictionary<string, string>(),
            mockError
        );

        var ex = Assert.ThrowsAsync<BadRequest>(async () =>
        await standardCheckoutClient.GetOrderStatus(orderId));

        Assert.That(ex.HttpStatusCode, Is.EqualTo(400));
        Assert.That(ex.Code, Is.EqualTo("MERCHANT_ORDER_MAPPING_NOT_FOUND"));
        Assert.That(ex.Message, Is.EqualTo("No entry found for Merchant"));

    }


}
