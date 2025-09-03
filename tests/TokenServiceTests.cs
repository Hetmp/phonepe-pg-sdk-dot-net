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

using Microsoft.Extensions.Logging;
using NUnit.Framework;
namespace pg_sdk_dotnet.tests;

[NonParallelizable]
[TestFixture]
public class TokenServiceTests : BaseSetup
{
    private Dictionary<string, string> FormData;
    private TokenService tokenService;


    [SetUp]
    public void SetUp()
    {
        var envConfigInstance = EnvConfig.GetBaseUrls(Env.TESTING);
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var credentialConfig = new CredentialConfig("auth_client", "auth_secret", 1);

        tokenService = new TokenService(credentialConfig, loggerFactory, envConfigInstance);

        FormData = new Dictionary<string, string>
        {
            { "client_id", "auth_client" },
            { "client_secret", "auth_secret" },
            { "client_version", ClientVersion.ToString() },
            { "grant_type", "client_credentials" }
        };

    }

    [Test, Order(1)]
    public async Task OAuth_Should_Return_Valid_Token()
    {
        OAuthResponse oAuthResponse = new OAuthResponse
        {
            AccessToken = "access_token",
            EncryptedAccessToken = "encryptedAccessToken",
            RefreshToken = "refreshToken",
            ExpiresIn = 2432,
            IssuedAt = 1744024547,
            ExpiresAt = 1744025047,
            SessionExpiresAt = 234543534,
            TokenType = "O-Bearer"
        };

        Dictionary<string, string> authHeaders = GetAuthHeaders();

        string authUrl = AuthUrl;

        AddStubForFormDataPostRequest(authUrl, authHeaders, FormData, 200, new Dictionary<string, string>(), oAuthResponse);

        var result = await tokenService.GetOAuthTokenAsync(HttpClient);

        Assert.That(result, Is.EqualTo("O-Bearer access_token"));
    }

    [Test, Order(2)]
    public async Task OAuthTokenRefresh()
    {
        OAuthResponse oAuthResponse = new OAuthResponse
        {
            AccessToken = "access_token",
            EncryptedAccessToken = "encrypted_access_token",
            IssuedAt = 0,
            ExpiresAt = 213221,
            TokenType = "O-Bearer"
        };

        AddStubForFormDataPostRequest(AuthUrl, GetAuthHeaders(), FormData, 200, new(), oAuthResponse);

        await tokenService.GetOAuthTokenAsync(HttpClient);
        await Task.Delay(1);
        var actual = await tokenService.GetOAuthTokenAsync(HttpClient);

        Assert.That(actual, Is.EqualTo("O-Bearer access_token"));
    }

    [Test, Order(3)]
    public async Task OAuthUseCachedToken()
    {
        var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        OAuthResponse oAuthResponse = new OAuthResponse
        {
            AccessToken = "access_token",
            EncryptedAccessToken = "encrypted_access_token",
            IssuedAt = time,
            ExpiresAt = time + 6,
            TokenType = "O-Bearer"
        };

        AddStubForFormDataPostRequest(AuthUrl, GetAuthHeaders(), FormData, 200, new(), oAuthResponse);

        var token = await tokenService.GetOAuthTokenAsync(HttpClient);
        Assert.That(token, Is.EqualTo("O-Bearer access_token"));

        _ = await tokenService.GetOAuthTokenAsync(HttpClient);
        _ = await tokenService.GetOAuthTokenAsync(HttpClient);
        token = await tokenService.GetOAuthTokenAsync(HttpClient);

        Assert.That(token, Is.EqualTo("O-Bearer access_token"));
    }

    [Test, Order(4)]
    public async Task OAuthFirstFetchWorks_SecondCallFailsSoReturnsOldToken()
    {
        var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        OAuthResponse oAuthResponse = new OAuthResponse
        {
            AccessToken = "access_token",
            EncryptedAccessToken = "encrypted_access_token",
            IssuedAt = time,
            ExpiresAt = time + 6,
            TokenType = "O-Bearer"
        };

        AddStubForFormDataPostRequest(AuthUrl, GetAuthHeaders(), FormData, 200, new(), oAuthResponse);

        var actual = await tokenService.GetOAuthTokenAsync(HttpClient);
        Assert.That(actual, Is.EqualTo("O-Bearer access_token"));

        var errorResponse = new PhonePeResponse
        {
            Code = "INVALID_CLIENT",
            Message = "Bad_Request",
            ErrorCode = "errorCode",
            Data = new Dictionary<string, object> { { "errorDescription", "Client Authentication Failure" } }
        };

        AddStubForFormDataPostRequest(AuthUrl, GetAuthHeaders(), FormData, 400, new(), errorResponse);

        var actual2 = await tokenService.GetOAuthTokenAsync(HttpClient);
        Assert.That(actual2, Is.EqualTo("O-Bearer access_token"));
    }

}

