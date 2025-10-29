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

public class Expiry
{
    public string Month { get; private set; }

    public string Year { get; private set; }

    private Expiry(string month, string year)
    {
        Month = month;
        Year = year;
    }

    public static ExpiryBuilder Builder() => new();

    public class ExpiryBuilder
    {
        private string _expiryMonth = null!;
        private string _expiryYear = null!;

        public ExpiryBuilder SetExpiryMonth(string expiryMonth)
        {
            this._expiryMonth = expiryMonth;
            return this;
        }

        public ExpiryBuilder SetExpiryYear(string expiryYear)
        {
            this._expiryYear = expiryYear;
            return this;
        }

        public Expiry Build()
        {
            return new Expiry(this._expiryMonth, this._expiryYear);
        }
    }
}