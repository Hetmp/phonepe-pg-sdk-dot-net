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
public class DeviceContext(string? deviceOS = null, string? merchantCallBackScheme = null)
{
  public string? DeviceOS { get; } = deviceOS;
  public string? MerchantCallBackScheme { get; } = merchantCallBackScheme;

  public static DeviceContextBuilder Builder() => new();

  public class DeviceContextBuilder
  {
    private string? _deviceOS;
    private string? _merchantCallBackScheme;

    /**
    * SETTERS
    */
    public DeviceContextBuilder SetDeviceOS(string? deviceOS)
    {
        this._deviceOS = deviceOS;
        return this;
    }

    public DeviceContextBuilder SetMerchantCallBackScheme(string? merchantCallBackScheme)
    {
        this._merchantCallBackScheme = merchantCallBackScheme;
        return this;
    }

    public DeviceContext Build()
    {
        return new DeviceContext(this._deviceOS, this._merchantCallBackScheme);
    }
  }
}