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

using System.Reflection;

namespace pg_sdk_dotnet.Common.Exception;
public static class ExceptionMapper
{
    public static readonly Dictionary<int, Type> CodeToException = new()
    {
        { 400, typeof(BadRequest) },
        { 401, typeof(UnauthorizedAccess) },
        { 403, typeof(ForbiddenAccess) },
        { 404, typeof(ResourceNotFound) },
        { 409, typeof(ResourceConflict) },
        { 410, typeof(ResourceGone) },
        { 417, typeof(ExpectationFailed) },
        { 422, typeof(ResourceInvalid) },
        { 429, typeof(TooManyRequests) }
    };

    public static void PrepareCodeToException(int responseCode, string message, PhonePeResponse phonePeResponse)
    {
        if (CodeToException.TryGetValue(responseCode, out Type? exceptionType))
        {
            if (exceptionType == null)
            {
                throw new InvalidOperationException();
            }

            try
            {
                var exceptionInstance = Activator.CreateInstance(exceptionType, responseCode, message, phonePeResponse) ?? throw new InvalidOperationException("Failed to create exception instance.");

                throw (System.Exception)exceptionInstance;
            }
            catch (TargetInvocationException)
            {
                throw new InvalidOperationException();
            }
        }
        else
        {
            throw new System.Exception($"{message}");
        }
    }
}
