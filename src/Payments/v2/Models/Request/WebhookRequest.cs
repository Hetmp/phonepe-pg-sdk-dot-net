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

namespace pg_sdk_dotnet.Payments.v2.Models.Request;

/// <summary>
/// Request payload for Create Webhook API.
/// </summary>
public class WebhookRequest
{
    /// <summary>
    /// The HTTPS webhook URL to which PhonePe will post webhook responses.
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// Description of the webhook URL.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Username for webhook authentication.
    /// Length must be between 5 and 20 characters. Must be alphanumeric only.
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Password for webhook authentication.
    /// Must be 8-20 characters long. Must be alphanumeric and contain both letters and numbers.
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Array of webhook events for which callback is required.
    /// </summary>
    public required List<string> Events { get; set; }
}

/// <summary>
/// Fetch Webhook Request (List/Get specific webhook).
/// </summary>
public class FetchWebhookRequest
{
    /// <summary>
    /// Unique identifier of the webhook to fetch.
    /// </summary>
    public required string WebhookId { get; set; }
}

/// <summary>
/// Update Webhook Request.
/// </summary>
public class UpdateWebhookRequest
{
    /// <summary>
    /// The HTTPS webhook URL.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Description of the webhook URL.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Username for webhook authentication.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Password for webhook authentication.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Array of webhook events.
    /// </summary>
    public List<string>? Events { get; set; }

    public bool enabled { get; set; }
}

/// <summary>
/// Delete Webhook Request.
/// </summary>
public class DeleteWebhookRequest
{
    /// <summary>
    /// Unique identifier of the webhook to delete.
    /// </summary>
    public required string WebhookId { get; set; }
}