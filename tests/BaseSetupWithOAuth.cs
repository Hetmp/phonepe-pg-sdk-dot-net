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

namespace pg_sdk_dotnet.tests;
public class BaseSetupWithOAuth : BaseSetup
{
    private OAuthResponse oAuthResponse;
    private readonly Dictionary<string, string> FormData;

    public BaseSetupWithOAuth()
    {
        FormData = new Dictionary<string, string>
        {
            { "client_id", ClientId },
            { "client_secret", ClientSecret },
            { "client_version", ClientVersion.ToString() },
            { "grant_type", "client_credentials" }
        };
    }

    public void OAuthSetup()
    {
        oAuthResponse = new OAuthResponse
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

    }
}
