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

[TestFixture]
[NonParallelizable]
public class CustomValidateCallbackTests : BaseSetup
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
    public void CustomCheckoutClient_ValidateCallback_CorrectCredentials()
    {var username = "username";
        var password = "password";
        var authorization = "bc842c31a9e54efe320d30d948be61291f3ceee4766e36ab25fa65243cd76e0e";
        var responseBody = @"
        {
            ""event"": ""CHECKOUT_TRANSACTION_ATTEMPT_FAILED"",
            ""payload"": {
                ""state"": ""state"",
                ""amount"": 0,
                ""expireAt"": 300,
                ""merchantId"": ""MERxx"",
                ""orderId"": ""OMOxx"",
                ""paymentDetails"": [
                    {
                        ""paymentMode"": ""UPI_COLLECT""
                    }
                ]
            }
        }";

        var result = customCheckoutClient.ValidateCallback(username, password, authorization, responseBody);
        Assert.That(result.Event, Is.EqualTo("CHECKOUT_TRANSACTION_ATTEMPT_FAILED"));
        Assert.That(result.Payload.OrderId, Is.EqualTo("OMOxx"));
        Assert.That(result.Payload.PaymentDetails[0].PaymentMode.ToString(), Is.EqualTo("UPI_COLLECT"));
    }

    [Test, Order(2)]
    public void CustomCheckoutClient_ValidateCallback_WrongCredentials()
    {
        var username = "wrong_username";
        var password = "password";
        var authorization = "bc842c31a9e54efe320d30d948be61291f3ceee4766e36ab25fa65243cd76e0e";
        var responseBody = "{\"type\":\"CHECKOUT_TRANSACTION_ATTEMPT_FAILED\",\"payload\":{\"orderId\":\"OMOxx\",\"paymentDetails\":[{\"paymentMode\":\"UPI_COLLECT\"}]}}";
        
        var ex = Assert.Throws<PhonePeException>(() => customCheckoutClient.ValidateCallback(username, password, authorization, responseBody));
        Assert.That(ex.Message, Is.EqualTo("Invalid Callback"));
        Assert.That(ex.HttpStatusCode, Is.EqualTo(417));
    }

}

