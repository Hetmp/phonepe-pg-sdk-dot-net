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

namespace pg_sdk_dotnet.Payments.v2.CustomCheckout;
public static class CustomCheckoutConstants
{
    public static readonly string PAY_API = "/payments/v2/pay";
    public static readonly string ORDER_STATUS_API = "/payments/v2/order/{ORDER_ID}/status";
    public static readonly string REFUND_API = "/payments/v2/refund";
    public static readonly string REFUND_STATUS_API = "/payments/v2/refund/{REFUND_ID}/status";
    public static readonly string ORDER_DETAILS = "details";
    public static readonly string TRANSACTION_STATUS_API = "/payments/v2/transaction/{TRANSACTION_ID}/status";
    public static readonly string CREATE_ORDER_API = "/payments/v2/sdk/order";
}
