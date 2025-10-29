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
public class CustomRefundTests : BaseSetupWithOAuth
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
    public async Task CustomCheckout_Refund_Successful()
    {
        OAuthSetup();

        var url = "/payments/v2/refund";

        var mockRefundResponse = new
        {
            refundId = "refundId",
            amount = 100,
            state = "PENDING"
        };

        var refundRequest = RefundRequest.Builder()
            .SetMerchantRefundId(MerchantRefundId)
            .SetAmount(100)
            .Build();

        var headers = GetHeadersForPostReq();

        AddStubForPostRequest(
            url,
            headers,
            refundRequest,
            200,
            new Dictionary<string, string>(),
            mockRefundResponse
        );

        var result = await customCheckoutClient.Refund(refundRequest);

        Assert.That(result.RefundId, Is.EqualTo("refundId"));
        Assert.That(result.Amount, Is.EqualTo(100));
        Assert.That(result.State, Is.EqualTo("PENDING"));
    }
}