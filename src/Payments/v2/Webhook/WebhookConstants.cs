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

namespace pg_sdk_dotnet.Payments.v2.Webhook;

/// <summary>
/// Constants for Webhook API endpoints.
/// Reference: https://developer.phonepe.com/tsp-integration/tsp-webhook
/// </summary>
public static class WebhookConstants
{
    /// <summary>
    /// Create Webhook API endpoint.
    /// POST /apis/pg-sandbox/configs/v1/webhooks (Sandbox)
    /// POST /apis/pg/configs/v1/webhooks (Production)
    /// </summary>
    public const string CREATE_WEBHOOK_API = "/configs/v1/webhooks";

    /// <summary>
    /// Update Webhook API endpoint.
    /// PUT /apis/pg-sandbox/configs/v1/webhooks/{WEBHOOK_ID} (Sandbox)
    /// PUT /apis/pg/configs/v1/webhooks/{WEBHOOK_ID} (Production)
    /// </summary>
    public const string UPDATE_WEBHOOK_API = "/configs/v1/webhooks/{WEBHOOK_ID}";

    /// <summary>
    /// Delete Webhook API endpoint.
    /// DELETE /apis/pg-sandbox/configs/v1/webhooks/{WEBHOOK_ID} (Sandbox)
    /// DELETE /apis/pg/configs/v1/webhooks/{WEBHOOK_ID} (Production)
    /// </summary>
    public const string DELETE_WEBHOOK_API = "/configs/v1/webhooks/{WEBHOOK_ID}";

    /// <summary>
    /// Fetch Webhook API endpoint.
    /// GET /apis/pg-sandbox/configs/v1/webhooks/{WEBHOOK_ID} (Sandbox)
    /// GET /apis/pg/configs/v1/webhooks/{WEBHOOK_ID} (Production)
    /// </summary>
    public const string FETCH_WEBHOOK_API = "/configs/v1/webhooks/events";

    /// <summary>
    /// List Webhooks API endpoint.
    /// GET /apis/pg-sandbox/configs/v1/webhooks (Sandbox)
    /// GET /apis/pg/configs/v1/webhooks (Production)
    /// </summary>
    public const string LIST_WEBHOOKS_API = "/configs/v1/webhooks";
}