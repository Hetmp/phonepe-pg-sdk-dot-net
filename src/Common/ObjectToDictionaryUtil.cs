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

using System.Text.Json;
using System.Text.Json.Nodes;

namespace pg_sdk_dotnet.Common;
public class ObjectToDictionaryUtil
{
    public static Dictionary<string, string> ObjectToDictionary(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        var dict = new Dictionary<string, string>();

        JsonNode? node = JsonNode.Parse(json);
        if (node != null)
        {
            FlattenJson(node, dict, "");
        }

        return dict;
    }

    private static void FlattenJson(JsonNode node, Dictionary<string, string> dict, string prefix)
    {
        if (node is JsonObject obj)
        {
            foreach (var prop in obj)
            {
                if (prop.Value != null)
                {
                    FlattenJson(prop.Value, dict, $"{prefix}{prop.Key}.");
                }
            }
        }
        else if (node is JsonArray array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] != null)
                {
                    FlattenJson(array[i]!, dict, $"{prefix}{i}.");
                }
            }
        }
        else
        {
            dict[prefix.TrimEnd('.')] = node.ToString();
        }
    }
}