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
public class CustomPayTest : BaseSetupWithOAuth
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
    public async Task TestPayReturnSuccess()
    {
        string url = "/payments/v2/pay";

        OAuthSetup();

        var metaInfo = MetaInfo.Builder()
            .SetUdf1("udf1")
            .Build();

        var customCheckoutPayRequest = PgPaymentRequest.UpiIntentPayRequestBuilder()
            .SetMerchantOrderId("merchantOrderId")
            .SetAmount(100)
            .SetDeviceOS("IOS")
            .SetTargetApp("PHONEPE")
            .SetMetaInfo(metaInfo)
            .SetExpireAfter(300)
            .Build();


        var customCheckoutResponse = new CustomCheckoutPayResponse
        {
            OrderId = "orderId",
            State = "PENDING",
            ExpireAt = 300,
            RedirectUrl = "https://google.com"
        };

        var headers = GetHeadersForPostReq();

        AddStubForPostRequest(url, headers, customCheckoutPayRequest, 200, new Dictionary<string, string>(), customCheckoutResponse);

        var result = await customCheckoutClient.Pay(customCheckoutPayRequest);
        Assert.That(result.OrderId, Is.EqualTo("orderId"));
        Assert.That(result.State, Is.EqualTo("PENDING"));
        Assert.That(result.RedirectUrl, Is.EqualTo("https://google.com"));

    }

}
