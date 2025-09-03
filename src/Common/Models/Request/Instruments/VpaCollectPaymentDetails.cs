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

namespace pg_sdk_dotnet.Common.Models.Request.Instruments;

public class VpaCollectPaymentDetails(string vpa) : CollectPaymentDetails(CollectPaymentDetailsType.VPA)
{
    public string Vpa { get; } = vpa;

    public static VpaCollectPaymentDetailsBuilder Builder()
    {
        return new VpaCollectPaymentDetailsBuilder();
    }
}

public class VpaCollectPaymentDetailsBuilder
{
    private string _vpa = null!;

    public VpaCollectPaymentDetailsBuilder SetVpa(string vpa)
    {
        this._vpa = vpa;
        return this;
    }

    public VpaCollectPaymentDetails Build()
    {
        return new VpaCollectPaymentDetails(this._vpa);
    }
}
