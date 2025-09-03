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

using System.Text;
using NUnit.Framework;

namespace pg_sdk_dotnet.tests;

[TestFixture]
[NonParallelizable]
public class CommonUtilsTests
{
    [Test, Order(1)]
    public void Sha256_For_5_Parameters_For_Clients()
    {
        var args = new Dictionary<string, string>
        {
            { "clientId", "" },
            { "clientSecret", "" },
            { "clientVersion", "1" },
            { "env", "SANDBOX" },
            { "shouldPublishEvents", "false" }
        };
        string expected = "e39cd1485eaa4da69dce15f1b0e11d0c52075500fc062efca139b1f00348f974";

        string result = CommonUtils.CalculateSha256(args);
        Console.WriteLine(result);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test, Order(2)]
    public void Sha256_For_Callback_Validity()
    {
        var args = new Dictionary<string, string>
        {
            { "username", "" },
            { "password", "" }
        };
        string expected = "e7ac0786668e0ff0f02b62bd04f45ff636fd82db63b1104601c975dc005f3a67";

        string result = CommonUtils.CalculateSha256(args);

        Assert.That(result, Is.EqualTo(expected));
    }
    [Test, Order(3)]
    public void Sha256_For_Callback_With_Headers_And_BasicAuth()
    {
        string username = "testUser";
        string password = "testPass";
        string authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

        var headers = new Dictionary<string, string>
        {
            { "Authorization", $"Basic {authorization}" },
            { "X-Signature", "dummy-signature" },
            { "X-Timestamp", "1713625050" }
        };

        var args = new Dictionary<string, string>
        {
            { "Authorization", headers["Authorization"] },
            { "X-Signature", headers["X-Signature"] },
            { "X-Timestamp", headers["X-Timestamp"] }
        };

        string expected = CommonUtils.CalculateSha256(args);
        string result = CommonUtils.CalculateSha256(args);

        Assert.That(result, Is.EqualTo(expected));
    }

}