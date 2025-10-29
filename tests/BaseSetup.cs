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
public class BaseSetup : BaseWireMockTest
{
    protected string ClientId = "client_id";
    protected string ClientSecret = "client_secret";
    protected int ClientVersion = 1;
    protected string AuthUrl = "/v1/oauth/token";

    public static Dictionary<string, string> GetHeadersForPostReq()
    {
        return new Dictionary<string, string>
        {
            { Headers.OAUTH_AUTHORIZATION, "O-Bearer access_token" },
            { Headers.SOURCE, Headers.INTEGRATION },
            { Headers.SOURCE_VERSION,  Headers.API_VERSION },
            { Headers.SOURCE_PLATFORM, Headers.SDK_TYPE },
            { Headers.SOURCE_PLATFORM_VERSION, Headers.SDK_VERSION },
            { Headers.CONTENT_TYPE, Headers.APPLICATION_JSON},
        };
    }

    public static Dictionary<string, string> GetHeadersForGetReq()
    {
        return new Dictionary<string, string>
        {
            { Headers.SOURCE, Headers.INTEGRATION },
            { Headers.SOURCE_VERSION,  Headers.API_VERSION },
            { Headers.SOURCE_PLATFORM, Headers.SDK_TYPE },
            { Headers.SOURCE_PLATFORM_VERSION, Headers.SDK_VERSION },
            { Headers.OAUTH_AUTHORIZATION, "O-Bearer access_token" }
        };
    }
    public static Dictionary<string, string> GetAuthHeaders()
    {
        return new Dictionary<string, string>
        {
            { Headers.CONTENT_TYPE, Headers.APPLICATION_FORM_URLENCODED },
            { Headers.ACCEPT, Headers.APPLICATION_JSON }
        };
    }


}