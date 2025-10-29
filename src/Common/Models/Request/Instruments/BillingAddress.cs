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

using System.Text.Json.Serialization;

namespace pg_sdk_dotnet.Common.Models.Request.Instruments;
public class BillingAddress
{
    [JsonPropertyName("line1")]
    public string? Line1 { get; private set; }

    [JsonPropertyName("line2")]
    public string? Line2 { get; private set; }

    [JsonPropertyName("city")]
    public string? City { get; private set; }

    [JsonPropertyName("state")]
    public string? State { get; private set; }

    [JsonPropertyName("zip")]
    public string? Zip { get; private set; }

    [JsonPropertyName("country")]
    public string? Country { get; private set; }

    private BillingAddress(string? line1, string? line2, string? city, string? state, string? zip, string? country)
    {
        Line1 = line1;
        Line2 = line2;
        City = city;
        State = state;
        Zip = zip;
        Country = country;
    }

    public static BillingAddressBuilder Builder() => new();

    public class BillingAddressBuilder
    {
        private string? _line1;
        private string? _line2;
        private string? _city;
        private string? _state;
        private string? _zip;
        private string? _country;

        public BillingAddressBuilder SetLine1(string line1)
        {
            this._line1 = line1;
            return this;
        }

        public BillingAddressBuilder SetLine2(string line2)
        {
            this._line2 = line2;
            return this;
        }

        public BillingAddressBuilder SetCity(string city)
        {
            this._city = city;
            return this;
        }

        public BillingAddressBuilder SetState(string state)
        {
            this._state = state;
            return this;
        }

        public BillingAddressBuilder SetZip(string zip)
        {
            this._zip = zip;
            return this;
        }

        public BillingAddressBuilder SetCountry(string country)
        {
            this._country = country;
            return this;
        }

        public BillingAddress Build()
        {
            return new BillingAddress(this._line1, this._line2, this._city, this._state, this._zip, this._country);
        }
    }
}