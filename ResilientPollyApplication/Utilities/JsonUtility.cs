﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Utilities
{
    public static class JsonUtility
    {
        public static async Task<JsonTextReader> GetJsonFromResponseAsync(HttpResponseMessage response)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            var streamReader = new StreamReader(stream);
            var jsonReader = new JsonTextReader(streamReader);
            return jsonReader;
        }
    }
}
