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

using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using Google.Apis.Logging;

public class Program
{
    const string ProjectId = "genai-atamel";
    const string Location = "us-central1";

    const string AiPlatformUrl = $"https://{Location}-aiplatform.googleapis.com";
    const string ModelName = "imagegeneration";
    const string PredictUrl = $"{AiPlatformUrl}/v1/projects/{ProjectId}/locations/{Location}/publishers/google/models/{ModelName}:predict";

    const string BUCKET_NAME = "genai-atamel-images";

    private static async Task UploadImagesToGcs(List<Image> images)
    {
        foreach (var (image, idx) in images.Select((value, index) => (value, index)))
        {
            string fileName = $"image_{idx}.png";
            await UploadImageToGcs(image, fileName);
        }
    }

    private static async Task UploadImageToGcs(Image image, string fileName, string bucketName = BUCKET_NAME)
    {
        var client = await StorageClient.CreateAsync();
        using var outputStream = new MemoryStream();
        await image.SaveAsPngAsync(outputStream);
        var publicRead = new UploadObjectOptions { PredefinedAcl = PredefinedObjectAcl.PublicRead };
        var blob = await client.UploadObjectAsync(bucketName, fileName, "image/png", outputStream, publicRead);

        Console.WriteLine($"Uploaded file: {fileName} to bucket: {bucketName} with file URL: {blob.MediaLink}");
    }

    private async static Task<List<Image>> GenerateImages(string prompt, int sampleCount)
    {
        string payload = GeneratePayload(prompt, sampleCount);
        JObject? jsonObject = await SendRequest(payload);
        List<Image> images = new();

        JArray? predictions = jsonObject?["predictions"] as JArray;
        if (predictions != null)
        {
            foreach (JObject prediction in predictions)
            {
                byte[] bytesBase64Encoded = Convert.FromBase64String(prediction["bytesBase64Encoded"].ToString());
                using MemoryStream memoryStream = new(bytesBase64Encoded);
                Image image = Image.Load(memoryStream);
                images.Add(image);
            }
        }
        else
        {
            Console.WriteLine($"No predictions for prompt: {prompt}");
        }

        return images;
    }

    private static string GeneratePayload(string prompt, int sampleCount)
    {
        var payload = new
        {
            instances = new[]
              {
                new
                {
                    prompt = prompt
                }
            },
            parameters = new
            {
                sampleCount = sampleCount,
            }
        };

        return JsonConvert.SerializeObject(payload);
    }

    private async static Task<JObject?> SendRequest(string payload)
    {
        GoogleCredential credential = GoogleCredential.GetApplicationDefault();
        var handler = credential.ToDelegatingHandler(new HttpClientHandler());
        using HttpClient httpClient = new(handler);

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await httpClient.PostAsync(PredictUrl,
            new StringContent(payload, Encoding.UTF8, "application/json"));

        string contentString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<JObject>(contentString);
    }

    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
        var url = $"http://0.0.0.0:{port}";
        builder.WebHost.UseUrls(url);

        var app = builder.Build();

        app.MapGet("/", async context =>
        {
            string imagesParam = context.Request.Query["images"];
            int numberOfImages = string.IsNullOrEmpty(imagesParam) ? 1 : int.Parse(imagesParam);

            string prompt = context.Request.Query["prompt"];
            if (string.IsNullOrEmpty(prompt))
                prompt = "happy dogs";

            var images = await GenerateImages(prompt, numberOfImages);
            await UploadImagesToGcs(images);

            var response = $"Created {numberOfImages} image(s) with prompt: {prompt}";
            await context.Response.WriteAsync(response);
        });

        await app.RunAsync();
    }
}