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

using System.Security.Cryptography;
using System.Text;

namespace pg_sdk_dotnet.Common.Utils;

public enum ShaAlgorithm
{
    SHA256
}

public static class CommonUtils
{
    public static string CalculateSha256(Dictionary<string, string> args)
    {
        string data = string.Join(":", args.Values);
        return ShaHex(data, ShaAlgorithm.SHA256);
    }

    public static string ShaHex(string data, ShaAlgorithm algorithm)
    {
        return algorithm switch
        {
            ShaAlgorithm.SHA256 => ComputeSha256Hash(data),
            _ => throw new NotSupportedException()
        };
    }

    private static string ComputeSha256Hash(string data)
    {
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    public static bool IsCallbackValid(string username, string password, string authorization)
    {
        string sha256 = CalculateSha256(new Dictionary<string, string>
        {
            { ClientConstants.USERNAME, username },
            { ClientConstants.PASSWORD, password }
        });

        return sha256.Equals(authorization, StringComparison.OrdinalIgnoreCase);
    }
}