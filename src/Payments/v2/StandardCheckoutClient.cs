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
using System.Text.Json;

namespace pg_sdk_dotnet.Payments.v2;

/// <summary>
/// StandardCheckoutClient handles all standard checkout payment operations.
/// No longer inherits BaseClient. Instead, delegates auth/token handling to shared BaseClientContext.
/// This is a lightweight request handler focused on payment-specific logic.
/// 
/// Features:
/// - Automatic header management based on account type and method
/// - Optional TSP headers for partner accounts
/// - SDK headers automatically added for SDK methods
/// </summary>
public sealed class StandardCheckoutClient
{
    private readonly AccountType _accountType;
    private readonly BaseClientContext _context;
    private readonly ILogger<StandardCheckoutClient> _logger;
    private readonly Dictionary<string, string> _headers;

    private StandardCheckoutClient(
        BaseClientContext context,
        ILoggerFactory? loggerFactory = null,
        AccountType accountType = AccountType.DirectMerchant
    )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        this._logger = (loggerFactory ?? NullLoggerFactory.Instance).CreateLogger<StandardCheckoutClient>();                
        this._headers = PrepareHeaders();
        _accountType = accountType;
    }

    /// <summary>
    /// Gets or creates a StandardCheckoutClient instance.
    /// Account type should be configured via BaseClientContext.GetInstance().
    /// </summary>
    public static StandardCheckoutClient GetInstance(
        string clientId,
        string clientSecret,
        int clientVersion,
        Env env,
        ILoggerFactory? loggerFactory = null,
        AccountType accountType = AccountType.DirectMerchant
    )
    {
        // Initialize the shared context (singleton) with account type
        var context = BaseClientContext.GetInstance(
            clientId,
            clientSecret,
            clientVersion,
            env,
            loggerFactory ?? NullLoggerFactory.Instance
        );

        // Return a new instance using the shared context
        return new StandardCheckoutClient(context, loggerFactory, accountType);
    }

    /*
     * Initiates a standard checkout payment.
     * Headers are automatically managed based on account type and request parameters.
     */
    public async Task<StandardCheckoutPayResponse> Pay(StandardCheckoutPayRequest payRequest, TspHeaderMetadata? tspMetadata = null)
    {
        var url = StandardCheckoutConstants.PAY_API;
        var headers = PreparePayHeaders();

        // Merge TSP headers if partner account and metadata provided
        if (_accountType == AccountType.Partner)
        {
            headers = MergeTspHeaders(headers, tspMetadata);
        }

        try
        {
            var response = await _context.RequestViaAuthRefreshAsync<StandardCheckoutPayResponse, StandardCheckoutPayRequest>(
                HttpMethodType.POST,
                url,
                headers,
                Headers.APPLICATION_JSON,
                payRequest
            );
            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Pay API Failed");
            throw;
        }
    }

    /*
     * Fetches the order status.
     */
    public async Task<OrderStatusResponse> GetOrderStatus(string merchantOrderId, bool details = false, TspHeaderMetadata? tspMetadata = null)
    {
        var url = StandardCheckoutConstants.ORDER_STATUS_API.Replace("{ORDER_ID}", merchantOrderId);
        var queryParams = new Dictionary<string, string> { { StandardCheckoutConstants.ORDER_DETAILS, details.ToString().ToLower() } };

        var headers = PreparePayHeaders();

        // Merge TSP headers if partner account and metadata provided
        if (_accountType == AccountType.Partner)
        {
            headers = MergeTspHeaders(headers, tspMetadata);
        }

        try
        {
            var response = await _context.RequestViaAuthRefreshAsync<OrderStatusResponse, object>(
                HttpMethodType.GET,
                url,
                headers,
                Headers.APPLICATION_JSON,
                null,
                queryParams
            );

            return response;

        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Order Status API Failed");
            throw;
        }
    }

    /*
    * Initiates a refund for a completed order.
    */
    public async Task<RefundResponse> Refund(RefundRequest refundRequest, TspHeaderMetadata? tspMetadata = null)
    {
        var url = StandardCheckoutConstants.REFUND_API;

        var headers = PreparePayHeaders();

        // Merge TSP headers if partner account and metadata provided
        if (_accountType == AccountType.Partner)
        {
            headers = MergeTspHeaders(headers, tspMetadata);
        }

        try
        {
            var response = await _context.RequestViaAuthRefreshAsync<RefundResponse, RefundRequest>(
                HttpMethodType.POST,
                url,
                headers,
                Headers.APPLICATION_JSON,
                refundRequest
            );

            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Refund API Failed");
            throw;
        }
    }

    /*
    * Fetches the status of a refund.
    */
    public async Task<RefundStatusResponse> GetRefundStatus(string refundId, TspHeaderMetadata? tspMetadata = null)
    {
        var url = StandardCheckoutConstants.REFUND_STATUS_API.Replace("{REFUND_ID}", refundId);

        var headers = PreparePayHeaders();

        // Merge TSP headers if partner account and metadata provided
        if (_accountType == AccountType.Partner)
        {
            headers = MergeTspHeaders(headers, tspMetadata);
        }

        try
        {
            var response = await _context.RequestViaAuthRefreshAsync<RefundStatusResponse, object>(
                HttpMethodType.GET,
                url,
                headers,
                Headers.APPLICATION_JSON
            );

            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Refund Status API Failed");
            throw;
        }
    }

    /*
    * Fetches the status of a transaction.
    */
    public async Task<OrderStatusResponse> GetTransactionStatus(string transactionId, TspHeaderMetadata? tspMetadata = null)
    {
        var url = StandardCheckoutConstants.TRANSACTION_STATUS_API.Replace("{TRANSACTION_ID}", transactionId);

        var headers = PreparePayHeaders();

        // Merge TSP headers if partner account and metadata provided
        if (_accountType == AccountType.Partner)
        {
            headers = MergeTspHeaders(headers, tspMetadata);
        }

        try
        {
            var response = await _context.RequestViaAuthRefreshAsync<OrderStatusResponse, object>(
                HttpMethodType.GET,
                url,
                headers,
                Headers.APPLICATION_JSON
            );

            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Transaction Status API Failed");
            throw;
        }
    }

    /*
    * Creates an SDK order for integration.
    * Automatically includes SDK-specific headers.
    */
    public async Task<CreateSdkOrderResponse> CreateSdkOrder(CreateSdkOrderRequest sdkRequest)
    {
        var url = StandardCheckoutConstants.CREATE_ORDER_API;
        try
        {
            // SDK order creation always includes SDK headers
            var headers = PreparePayHeaders(addSdkHeaders: true);

            var response = await _context.RequestViaAuthRefreshAsync<CreateSdkOrderResponse, CreateSdkOrderRequest>(
                HttpMethodType.POST,
                url,
                headers,
                Headers.APPLICATION_JSON,
                sdkRequest
            );

            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Create SDK Order API Failed");
            throw;
        }
    }

    /*
    * Validates the callback response.
    */
    public CallbackResponse ValidateCallback(
        string username,
        string password,
        string authorization,
        string responseBody)
    {
        if (!CommonUtils.IsCallbackValid(username, password, authorization))
        {
            throw new PhonePeException(417, "Invalid Callback");
        }

        var callbackResponse = JsonSerializer.Deserialize<CallbackResponse>(responseBody, JsonOptions.CaseInsensitiveWithEnums)
            ?? throw new PhonePeException(500, "Invalid Callback Response");

        return callbackResponse;

    }

    /*
     * Prepares default headers specific to standard checkout.
     */
    private static Dictionary<string, string> PrepareHeaders()
    {
        return new Dictionary<string, string>
        {
            { Headers.CONTENT_TYPE, Headers.APPLICATION_JSON },
            { Headers.SOURCE, Headers.INTEGRATION },
        };
    }

    /*
     * Prepares headers for pay/SDK order operations.
     * Conditionally adds SDK-specific headers based on method type.
     */
    private Dictionary<string, string> PreparePayHeaders(bool addSdkHeaders = false)
    {
        var headers = new Dictionary<string, string>(_headers);

        // Add SDK headers when explicitly requested (for SDK-based payment flows).

        if (addSdkHeaders)
        {
            headers[Headers.SOURCE_VERSION] = Headers.API_VERSION;
            headers[Headers.SOURCE_PLATFORM] = Headers.SDK_TYPE;
            headers[Headers.SOURCE_PLATFORM_VERSION] = Headers.SDK_VERSION;
        }

        return headers;
    }

    /*
     * Merges standard headers with TSP-specific dynamic headers.
     * TSP headers are optional and request-specific based on integration channel.
     */
    private static Dictionary<string, string> MergeTspHeaders(
        Dictionary<string, string> standardHeaders,
        TspHeaderMetadata? tspMetadata)
    {
        var mergedHeaders = new Dictionary<string, string>(standardHeaders);
        var tspHeaders = TspHeaderBuilder.BuildHeaders(tspMetadata);

        foreach (var header in tspHeaders)
        {
            mergedHeaders[header.Key] = header.Value;
        }

        return mergedHeaders;
    }
}