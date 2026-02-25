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
using pg_sdk_dotnet.Common;
using pg_sdk_dotnet.Payments.v2.Webhook;

namespace pg_sdk_dotnet.Payments.v2;

/// <summary>
/// WebhookClient handles webhook management operations for TSP integration.
/// Provides methods to create, update, delete, and fetch webhooks.
/// Uses the shared BaseClientContext for authentication.
/// 
/// Features:
/// - Create webhooks with event configuration
/// - Update existing webhooks
/// - Delete webhooks
/// - Fetch specific webhooks
/// - List all webhooks
/// 
/// Reference: https://developer.phonepe.com/tsp-integration/tsp-webhook/create-webhook-api
/// </summary>
public sealed class WebhookClient
{
    private readonly BaseClientContext _context;
    private readonly ILogger<WebhookClient> _logger;
    private readonly Dictionary<string, string> _headers;

    private WebhookClient(
        BaseClientContext context,
        ILoggerFactory? loggerFactory = null)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        var factory = loggerFactory ?? NullLoggerFactory.Instance;
        _logger = factory.CreateLogger<WebhookClient>();
        _headers = PrepareHeaders();
    }

    /// <summary>
    /// Gets or creates a WebhookClient instance.
    /// Must be called after BaseClientContext.GetInstance() to ensure shared context is initialized.
    /// </summary>
    public static WebhookClient GetInstance(
        string clientId,
        string clientSecret,
        int clientVersion,
        Env env,
        ILoggerFactory? loggerFactory = null)
    {
        // Initialize the shared context (singleton)
        var context = BaseClientContext.GetInstance(
            clientId,
            clientSecret,
            clientVersion,
            env,
            loggerFactory ?? NullLoggerFactory.Instance);

        // Return a new instance using the shared context
        return new WebhookClient(context, loggerFactory);
    }

    /*
     * Creates a new webhook for receiving event notifications.
     * Webhook URL must be HTTPS and credentials will be used for basic authentication.
     */
    public async Task<CreateWebhookResponse> CreateWebhook(WebhookRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        ValidateCreateWebhookRequest(request);

        var url = WebhookConstants.CREATE_WEBHOOK_API;

        try
        {
            var response = await _context.RequestViaAuthRefreshAsync<CreateWebhookResponse, WebhookRequest>(
                HttpMethodType.POST,
                url,
                _headers,
                Headers.APPLICATION_JSON,
                request
            );

            _logger.LogInformation("Webhook created successfully. WebhookId: {WebhookId}", response?.Id);
            return response!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create Webhook API Failed");
            throw;
        }
    }

    /*
     * Updates an existing webhook configuration.
     */
    public async Task<CreateWebhookResponse> UpdateWebhook(
        string webhookId,
        UpdateWebhookRequest request)
    {
        if (string.IsNullOrEmpty(webhookId))
            throw new ArgumentException("WebhookId is required", nameof(webhookId));

        ArgumentNullException.ThrowIfNull(request);

        var url = WebhookConstants.UPDATE_WEBHOOK_API.Replace("{WEBHOOK_ID}", webhookId);

        try
        {
            var response = await _context.RequestViaAuthRefreshAsync<CreateWebhookResponse, UpdateWebhookRequest>(
                HttpMethodType.PUT,
                url,
                _headers,
                Headers.APPLICATION_JSON,
                request
            );

            _logger.LogInformation("Webhook {WebhookId} updated successfully", webhookId);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update Webhook API Failed for webhook {WebhookId}", webhookId);
            throw;
        }
    }

    /*
     * Deletes a webhook.
     */
    public async Task<DeleteWebhookRequest> DeleteWebhook(string webhookId)
    {
        if (string.IsNullOrEmpty(webhookId))
            throw new ArgumentException("WebhookId is required", nameof(webhookId));

        var url = WebhookConstants.DELETE_WEBHOOK_API.Replace("{WEBHOOK_ID}", webhookId);

        try
        {
            var response = await _context.RequestViaAuthRefreshAsync<DeleteWebhookRequest, object>(
                HttpMethodType.DELETE,
                url,
                _headers,
                Headers.APPLICATION_JSON
            );

            _logger.LogInformation("Webhook {WebhookId} deleted successfully", webhookId);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete Webhook API Failed for webhook {WebhookId}", webhookId);
            throw;
        }
    }

    /*
     * Fetches a list of events that is configured for the webhook. This can be used to verify the current configuration of the webhook.
     */
    public async Task<CreateWebhookResponse> FetchWebhook()
    {
        var url = WebhookConstants.FETCH_WEBHOOK_API;

        try
        {
            var response = await _context.RequestViaAuthRefreshAsync<CreateWebhookResponse, object>(
                HttpMethodType.GET,
                url,
                _headers,
                Headers.APPLICATION_JSON
            );

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fetch Webhook API Failed for webhook");
            throw;
        }
    }

    /*
     * Lists all webhooks.
     */
    public async Task<ListWebhooksResponse> ListWebhooks(Dictionary<string, string> queryParams)
    {
        var url = WebhookConstants.LIST_WEBHOOKS_API;

        try
        {
            var response = await _context.RequestViaAuthRefreshAsync<ListWebhooksResponse, object>(
                HttpMethodType.GET,
                url,
                _headers,
                Headers.APPLICATION_JSON,
                queryParams: queryParams
            );

            _logger.LogInformation("Fetched {Count} webhooks", response?.Total ?? 0);
            return response!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "List Webhooks API Failed");
            throw;
        }
    }

    /*
     * Prepares default headers specific to webhook operations.
     */
    private static Dictionary<string, string> PrepareHeaders()
    {
        return new Dictionary<string, string>
        {
            { Headers.CONTENT_TYPE, Headers.APPLICATION_JSON }
        };
    }

    /*
     * Validates Create Webhook Request.
     * Ensures all required fields are provided with correct formats.
     */
    private static void ValidateCreateWebhookRequest(WebhookRequest request)
    {
        if (string.IsNullOrEmpty(request.Url))
            throw new ArgumentException("Webhook URL is required", nameof(request.Url));

        if (!request.Url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Webhook URL must use HTTPS protocol", nameof(request.Url));

        if (string.IsNullOrEmpty(request.Username) || request.Username.Length < 5 || request.Username.Length > 20)
            throw new ArgumentException("Username must be between 5 and 20 characters", nameof(request.Username));

        if (!IsAlphanumeric(request.Username))
            throw new ArgumentException("Username must be alphanumeric only", nameof(request.Username));

        if (string.IsNullOrEmpty(request.Password) || request.Password.Length < 8 || request.Password.Length > 20)
            throw new ArgumentException("Password must be between 8 and 20 characters", nameof(request.Password));

        if (!IsAlphanumericWithNumbers(request.Password))
            throw new ArgumentException("Password must be alphanumeric and contain both letters and numbers", nameof(request.Password));

        if (request.Events == null || request.Events.Count == 0)
            throw new ArgumentException("At least one webhook event must be specified", nameof(request.Events));
    }

    /*
     * Checks if string contains only alphanumeric characters.
     */
    private static bool IsAlphanumeric(string value)
    {
        return value.All(char.IsLetterOrDigit);
    }

    /*
     * Checks if string is alphanumeric and contains both letters and numbers.
     */
    private static bool IsAlphanumericWithNumbers(string value)
    {
        return value.All(char.IsLetterOrDigit) && 
               value.Any(char.IsLetter) && 
               value.Any(char.IsDigit);
    }
}