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
public class RefundStatusTests : BaseSetupWithOAuth
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
    public async Task RefundStatus_Successful_StandardCheckout()
    {
        OAuthSetup();

        var refundId = "test-refund-id";
        var url = $"/payments/v2/refund/{refundId}/status";

        var mockRefundResponse = new
        {
            merchantRefundId = refundId,
            amount = 100,
            state = "PENDING",
            paymentDetails = new[]
            {
                new 
                {
                    transactionId = "txn_123456",
                    paymentMode = "UPI_COLLECT",
                    timestamp = 34234324L,
                    amount = 100,
                    state = "PENDING",
                    errorCode = (string?)null,
                    detailedErrorCode = (string?)null,
                    instrument = (object?)null, 
                    rail = (object?)null,      
                    splitInstruments = new object[] {} 
                }
            }
        };

        var headers = GetHeadersForGetReq();

        AddStubForGetRequest(
            url,
            new Dictionary<string, string>(),
            200,
            headers,
            new Dictionary<string, string>(),
            mockRefundResponse
        );

        var response = await standardCheckoutClient.GetRefundStatus(refundId);

        Assert.That(response.MerchantRefundId, Is.EqualTo(refundId));
        Assert.That(response.Amount, Is.EqualTo(100));
        Assert.That(response.State, Is.EqualTo("PENDING"));
        Assert.That(response.PaymentDetails.Count, Is.EqualTo(1));
        Assert.That(response.PaymentDetails[0].PaymentMode.ToString(), Is.EqualTo("UPI_COLLECT"));
    }
}
