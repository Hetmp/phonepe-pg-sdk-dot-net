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

namespace pg_sdk_dotnet.Payments.v2.Models.Response;

/// <summary>
/// Response payload for Create Webhook API (success).
/// </summary>
public class CreateWebhookResponse
{
    /// <summary>
    /// Unique identifier of the webhook.
    /// Can be used to update or delete the webhook.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// The HTTPS webhook URL.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Description of the webhook.
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
    /// List of webhook events.
    /// </summary>
    public List<string>? Events { get; set; }

    /// <summary>
    /// Epoch timestamp indicating when the webhook was created.
    /// </summary>
    public long CreatedAt { get; set; }

    /// <summary>
    /// Epoch timestamp indicating the last time the webhook was updated.
    /// </summary>
    public long UpdatedAt { get; set; }

    /// <summary>
    /// Flag to indicate whether the webhook is currently active or not.
    /// </summary>
    public bool Enabled { get; set; }
}

/// <summary>
/// List all webhooks response.
/// </summary>
public class ListWebhooksResponse
{
    /// <summary>
    /// List of webhooks.
    /// </summary>
    public List<CreateWebhookResponse>? Webhooks { get; set; }

    /// <summary>
    /// Total number of webhooks.
    /// </summary>
    public int Total { get; set; }
}

/// <summary>
/// Error response for webhook operations.
/// </summary>
public class WebhookErrorResponse
{
    /// <summary>
    /// Error code.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Error message.
    /// </summary>
    public string? Message { get; set; }
}