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
public class ExceptionTests : BaseSetupWithOAuth
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
    public async Task R999_ShouldThrowPhonePeException_WithHttpStatusCode503()
    {
        OAuthSetup();

        var url = "/checkout/v2/pay";

        var request = StandardCheckoutPayRequest.Builder()
            .SetMerchantOrderId("merchantOrderId")
            .SetAmount(100)
            .Build();

        var headers = GetHeadersForPostReq();

        var errorResponse = "R999";

        AddStubForPostRequest(
            url,
            headers,
            request,
            503,
            new Dictionary<string, string>(),
            errorResponse
        );

        var exception = Assert.ThrowsAsync<PhonePeException>(async () =>
            await standardCheckoutClient.Pay(request)
        );

        Assert.That(exception.Message, Is.EqualTo("Service Unavailable"));
        Assert.That(exception.HttpStatusCode, Is.EqualTo(503));
    }

    [Test, Order(2)]
    public void ForbiddenAccess_ShouldStoreResponseCodeAndMessage()
    {
       var mockResponse = new PhonePeResponse
        {
            Code = "R999",
            Message = "Access Denied",
            ErrorCode = "Error code"
        };
        var exception = new ForbiddenAccess(403, "Access Denied", mockResponse);

        Assert.That(exception.Message, Is.EqualTo("Access Denied"));
        Assert.That(exception.HttpStatusCode, Is.EqualTo(403));
    }

    [Test, Order(3)]
    public void ResourceConflict_ShouldReturn409()
    {
        var mockResponse = new PhonePeResponse
        {
            Code = "R999",
            Message = "Conflict occurred",
            ErrorCode = "Error code"
        };
        var exception = new ResourceConflict(409, "Conflict occurred", mockResponse);

        Assert.That(exception.Message, Is.EqualTo("Conflict occurred"));
        Assert.That(exception.HttpStatusCode, Is.EqualTo(409));
    }

    [Test, Order(4)]
    public void ResourceGone_ShouldReturn410()
    {
        var mockResponse = new PhonePeResponse
        {
            Code = "R998",
            Message = "Resource no longer available",
            ErrorCode = "Gone"
        };
        var exception = new ResourceGone(410, "Resource no longer available", mockResponse);

        Assert.That(exception.Message, Is.EqualTo("Resource no longer available"));
        Assert.That(exception.HttpStatusCode, Is.EqualTo(410));
    }

    [Test, Order(5)]
    public void ResourceInvalid_ShouldReturn422()
    {
        var mockResponse = new PhonePeResponse
        {
            Code = "R997",
            Message = "Invalid resource",
            ErrorCode = "Invalid"
        };
        var exception = new ResourceInvalid(422, "Invalid resource", mockResponse);

        Assert.That(exception.Message, Is.EqualTo("Invalid resource"));
        Assert.That(exception.HttpStatusCode, Is.EqualTo(422));
    }

    [Test, Order(6)]
    public void ResourceNotFound_ShouldReturn404()
    {
        var mockResponse = new PhonePeResponse
        {
            Code = "R996",
            Message = "Resource not found",
            ErrorCode = "NotFound"
        };
        var exception = new ResourceNotFound(404, "Resource not found", mockResponse);

        Assert.That(exception.Message, Is.EqualTo("Resource not found"));
        Assert.That(exception.HttpStatusCode, Is.EqualTo(404));
    }

    [Test, Order(7)]
    public void TooManyRequests_ShouldReturn429()
    {
        var mockResponse = new PhonePeResponse
        {
            Code = "R995",
            Message = "Too many requests",
            ErrorCode = "RateLimit"
        };
        var exception = new TooManyRequests(429, "Too many requests", mockResponse);

        Assert.That(exception.Message, Is.EqualTo("Too many requests"));
        Assert.That(exception.HttpStatusCode, Is.EqualTo(429));
    }

    [Test, Order(8)]
    public void UnauthorizedAccess_ShouldReturn401()
    {
        var mockResponse = new PhonePeResponse
        {
            Code = "R994",
            Message = "Unauthorized",
            ErrorCode = "Unauthorized"
        };
        var exception = new UnauthorizedAccess(401, "Unauthorized", mockResponse);

        Assert.That(exception.Message, Is.EqualTo("Unauthorized"));
        Assert.That(exception.HttpStatusCode, Is.EqualTo(401));
    }

    [Test, Order(9)]
    public void ExpectationFailed_ShouldReturn417()
    {
       var mockResponse = new PhonePeResponse
        {
            Code = "R999",
            Message = "Expectation Failed",
            ErrorCode = "Error code"
        };
        var exception = new ExpectationFailed(417, "Expectation Failed", mockResponse);

        Assert.That(exception.Message, Is.EqualTo("Expectation Failed"));
        Assert.That(exception.HttpStatusCode, Is.EqualTo(417));
    }
}
