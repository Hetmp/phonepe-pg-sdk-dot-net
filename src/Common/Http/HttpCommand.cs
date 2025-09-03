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

using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace pg_sdk_dotnet.Common.Http;
public class HttpCommand<T, R>
{
    private readonly HttpClient _client;
    private readonly string _hostUrl;
    private readonly string _url;
    private readonly Dictionary<string, string> _headers;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly R? _requestData;
    private readonly string _encodingType;
    private readonly HttpMethodType _methodName;
    private readonly Dictionary<string, string> _queryParams;
    private readonly ILogger<HttpCommand<T, R>> _logger;

    public HttpCommand(
        HttpClient client,
        string hostUrl,
        string url,
        Dictionary<string, string> headers,
        R requestData,
        string encodingType,
        HttpMethodType methodName,
        Dictionary<string, string> queryParams,
        ILogger<HttpCommand<T, R>> logger)
    {
        this._client = client ?? throw new ArgumentNullException(nameof(client));
        this._hostUrl = hostUrl ?? throw new ArgumentNullException(nameof(hostUrl));
        this._url = url;
        this._headers = headers ?? [];
        this._jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        this._requestData = requestData;
        this._encodingType = encodingType;
        this._methodName = methodName;
        this._queryParams = queryParams ?? [];
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /**
     * Prepares the HTTP request body based on the encoding type and request data.
     * Returns the request body containing the serialized data.
     * @return the prepared HttpContent or null
     */
    private HttpContent? PrepareRequestBody()
    {
        if (this._requestData is null)
        {
            return null;
        }

        switch (this._encodingType)
        {
            case "application/x-www-form-urlencoded":
                Dictionary<string, string> dict = ObjectToDictionaryUtil.ObjectToDictionary(this._requestData);
                return new FormUrlEncodedContent(dict);

            case "application/json":
                string requestForApplicationJson = JsonSerializer.Serialize(this._requestData, JsonOptions.IndentedWithPaymentConverters);
                return new StringContent(requestForApplicationJson, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));

            default:
                throw new InvalidOperationException("Unsupported encoding type.");

        }
    }

    /**
     * Constructs an HTTP URL by appending the provided URL to the base host URL.
     * Also adds any query parameters to the URL.
     *
     * @return the constructed URI
     * @throws ArgumentException if the host or endpoint URL is invalid
     */
    private async Task<Uri> PrepareHttpUrl()
    {
        if (string.IsNullOrWhiteSpace(this._hostUrl))
        {
            throw new ArgumentException("Host URL cannot be null or empty.", nameof(this._hostUrl));
        }

        if (string.IsNullOrWhiteSpace(this._url))
        {
            throw new ArgumentException("Endpoint URL cannot be null or empty.", nameof(this._url));
        }

        try
        {
            var builder = new UriBuilder($"{this._hostUrl}{this._url}");

            if (this._queryParams?.Count > 0)
            {
                var query = await new FormUrlEncodedContent(this._queryParams).ReadAsStringAsync();
                builder.Query = query;
            }

            return builder.Uri;
        }
        catch (UriFormatException ex)
        {
            throw new ArgumentException("Invalid URL format.", ex);
        }
    }

    /**
     * Executes the HTTP request using the configured URL, method, headers, and body.
     * Handles the HTTP response and deserializes it to the expected return type.
     *
     * @return deserialized response object of type T
     */
    public async Task<T> ExecuteAsync()
    {
        var httpUrl = await PrepareHttpUrl();
        var request = PrepareRequest(httpUrl);
        var response = await this._client.SendAsync(request);

        return await HandleResponse(response);
    }

    /**
     * Handles the HTTP response.
     * If the response status code indicates success, deserializes the response to T.
     * If not, attempts to parse error details and throws appropriate exceptions.
     *
     * @param response the received HttpResponseMessage
     * @return deserialized object of type T
     * @throws PhonePeException or derived exceptions on failure
     */
    private async Task<T?> HandleResponse(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadAsStringAsync();
        this._logger.LogDebug($"Raw JSON Response from : {responseBody}");

        int statusCode = (int)response.StatusCode;
        this._logger.LogDebug($"Status Code: {statusCode}");

        if (statusCode >= 200 && statusCode <= 299)
        {
            if (string.IsNullOrWhiteSpace(responseBody))
            {
                return default;
            }

            var result = JsonSerializer.Deserialize<T>(responseBody, this._jsonOptions);
            return result ?? throw new InvalidOperationException("Deserialization returned null.");
        }

        try
        {
            var phonePeResponse = JsonSerializer.Deserialize<PhonePeResponse>(responseBody, this._jsonOptions);
            var message = response.ReasonPhrase;

            if (phonePeResponse == null)
            {
                throw new PhonePeException(statusCode, message, null);
            }

            if (ExceptionMapper.CodeToException.ContainsKey(statusCode))
            {
                ExceptionMapper.PrepareCodeToException(statusCode, message, phonePeResponse);
            }
            else if (statusCode >= 400 && statusCode <= 499)
            {
                throw new ClientError(statusCode, message ?? "Client error", phonePeResponse);
            }
            else if (statusCode >= 500 && statusCode <= 599)
            {
                throw new ServerError(statusCode, message ?? "Server error", phonePeResponse);
            }
            throw new PhonePeException(statusCode, message, phonePeResponse);

        }
        catch (JsonException)
        {
            throw new PhonePeException(statusCode, response.ReasonPhrase ?? "Invalid JSON format", null);
        }
    }


    /**
     * Constructs the HttpRequestMessage using the URL, method, body, and headers.
     * Throws an exception if the HTTP method is not supported.
     *
     * @param httpUrl the final URI for the request
     * @return the constructed HttpRequestMessage
     * @throws PhonePeException if an unsupported method is provided
     */
    private HttpRequestMessage PrepareRequest(Uri httpUrl)
    {
        var content = PrepareRequestBody();

        var request = new HttpRequestMessage
        {
            RequestUri = httpUrl,
            Method = this._methodName switch
            {
                HttpMethodType.POST => HttpMethod.Post,
                HttpMethodType.GET => HttpMethod.Get,
                _ => throw new PhonePeException(405, "Method Not Supported")
            },
            Content = content
        };

        foreach (var header in this._headers)
        {
            if (header.Key.Equals(Headers.CONTENT_TYPE, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
        return request;
    }
}
