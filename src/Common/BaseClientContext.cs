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
using Microsoft.Extensions.Logging.Abstractions;

namespace pg_sdk_dotnet.Common;

/// <summary>
/// BaseClientContext is a concrete singleton wrapper around BaseClient.
/// It inherits all BaseClient functionality (token refresh, auth headers, HTTP handling)
/// while adding centralized singleton management for SDK initialization.
/// 
/// All API clients (StandardCheckout, Webhook, etc.) share this context to ensure:
/// - Single token refresh cycle (no conflicts)
/// - Centralized credential management
/// - Shared HttpClient (connection pooling)
/// - Consistent auth across all API calls
/// - Automatic benefit from any future BaseClient enhancements
/// </summary>
public sealed class BaseClientContext : BaseClient
{
    private static BaseClientContext? _instance;
    private static readonly object _lock = new();

    private BaseClientContext(
        string clientId,
        string clientSecret,
        int clientVersion,
        Env env,
        ILoggerFactory? loggerFactory)
        : base(clientId, clientSecret, clientVersion, env, loggerFactory ?? NullLoggerFactory.Instance)
    {
    }

    /// <summary>
    /// Gets or initializes the singleton BaseClientContext instance.
    /// Thread-safe initialization ensures only one context is created.
    /// All protected BaseClient functionality (token refresh, auth) is inherited and shared.
    /// </summary>
    public static BaseClientContext GetInstance(
        string clientId,
        string clientSecret,
        int clientVersion,
        Env env,
        ILoggerFactory? loggerFactory = null)
    {
        if (_instance != null)
        {
            return _instance;
        }

        lock (_lock)
        {
            _instance ??= new BaseClientContext(clientId, clientSecret, clientVersion, env, loggerFactory);
        }

        return _instance;
    }

    /// <summary>
    /// Public wrapper around the protected BaseClient.RequestViaAuthRefreshAsync method.
    /// Delegates to the inherited BaseClient implementation which handles:
    /// - OAuth token retrieval and refresh
    /// - Automatic retry on token expiration
    /// - HTTP request execution with authentication
    /// </summary>
    public new async Task<T> RequestViaAuthRefreshAsync<T, R>(
        HttpMethodType method,
        string endpoint,
        Dictionary<string, string> headers,
        string encodingType,
        R? requestData = default,
        Dictionary<string, string>? queryParams = null)
    {
        return await base.RequestViaAuthRefreshAsync<T, R>(
            method,
            endpoint,
            headers,
            encodingType,
            requestData,
            queryParams
        );
    }
}