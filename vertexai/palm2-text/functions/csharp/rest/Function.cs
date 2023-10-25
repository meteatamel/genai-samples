// Copyright 2023 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using Google.Cloud.Functions.Framework;
using Google.Cloud.Functions.Hosting;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace GenAI;

// Dependency injection configuration, executed during server startup.
public class Startup : FunctionsStartup
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        // Make an HttpClient available to our function via dependency injection.
        // There are many options here; see
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests
        // for more details.
        services.AddHttpClient<IHttpFunction, Function>();
    }
}

[FunctionsStartup(typeof(Startup))]
public class Function : IHttpFunction
{
    private readonly HttpClient _httpClient;

    public Function(HttpClient httpClient) =>
        _httpClient = httpClient;

    public async Task HandleAsync(HttpContext context)
    {
        string PROJECT_ID = Environment.GetEnvironmentVariable("PROJECT_ID");
        string REGION = Environment.GetEnvironmentVariable("REGION");
        if (string.IsNullOrEmpty(REGION) || string.IsNullOrEmpty(PROJECT_ID))
        {
            throw new Exception("Environment variable 'REGION' or 'PROJECT_ID' not set.");
        }

        string apiUrl = $"https://{REGION}-aiplatform.googleapis.com/v1/projects/{PROJECT_ID}/locations/{REGION}/publishers/google/models/text-bison:predict";

        GoogleCredential credential = GoogleCredential.GetApplicationDefault();
        var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

        var payload = new
        {
            instances = new[]
            {
                new
                {
                    prompt = "Give me ten interview questions for the role of program manager."
                }
            },
            parameters = new
            {
                temperature = 0.2,
                maxOutputTokens = 256,
                topK = 40,
                topP = 0.95
            }
        };

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await _httpClient.PostAsync(apiUrl,
            new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

        string responseBody = await response.Content.ReadAsStringAsync();
        await context.Response.WriteAsync($"Response: {response.StatusCode}");
        await context.Response.WriteAsync(responseBody);
    }
}