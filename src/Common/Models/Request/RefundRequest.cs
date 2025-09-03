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

namespace pg_sdk_dotnet.Common.Models.Request;

/**
 * Creates a request to initiate a refund -> RefundRequest.Builder()
 */

public class RefundRequest(
    string merchantRefundId,
    string originalMerchantOrderId,
    long amount)
{
    public string MerchantRefundId { get; } = merchantRefundId;
    public string OriginalMerchantOrderId { get; } = originalMerchantOrderId;
    public long Amount { get; } = amount;
    public static RefundRequestBuilder Builder()
    {
        return new RefundRequestBuilder();
    }
}

public class RefundRequestBuilder
{
    private string _merchantRefundId = string.Empty;
    private string _originalMerchantOrderId = string.Empty;
    private long _amount;

    public RefundRequestBuilder SetMerchantRefundId(string merchantRefundId)
    {
        this._merchantRefundId = merchantRefundId;
        return this;
    }
    public RefundRequestBuilder SetMerchantOrderId(string originalMerchantOrderId)
    {
        this._originalMerchantOrderId = originalMerchantOrderId;
        return this;
    }

    public RefundRequestBuilder SetAmount(long amount)
    {
        this._amount = amount;
        return this;
    }


    public RefundRequest Build()
    {
        return new RefundRequest(
            this._merchantRefundId,
            this._originalMerchantOrderId,
            this._amount
        );
    }
}
