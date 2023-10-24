using Google.Cloud.AIPlatform.V1;
using wkt = Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;

public class Program
{
    static readonly string? ProjectId = Environment.GetEnvironmentVariable("PROJECT_ID");
    static readonly string? Region = Environment.GetEnvironmentVariable("REGION");

    static readonly string AiPlatformUrl = $"https://{Region}-aiplatform.googleapis.com";
    const string Publisher = "google";
    const string ModelName = "text-bison";

    public async static Task<string> GenerateText(string prompt)
    {
        if (string.IsNullOrEmpty(ProjectId) || string.IsNullOrEmpty(Region))
        {
            throw new Exception("Environment variable 'PROJECT_ID' or 'REGION' not set.");
        }

        var instance = new
        {
            prompt = prompt
        };

        var parameters = new
        {
            temperature = 0.2,
            maxOutputTokens = 256,
            topK = 40,
            topP = 0.95
        };

        PredictionServiceClient client = await new PredictionServiceClientBuilder
        {
            Endpoint = AiPlatformUrl
        }.BuildAsync();

        var request = new PredictRequest
        {
            EndpointAsEndpointName = EndpointName.FromProjectLocationPublisherModel(ProjectId, Region, Publisher, ModelName),
            Instances = { wkt::Value.Parser.ParseJson(JsonConvert.SerializeObject(instance)) },
            Parameters = wkt::Value.Parser.ParseJson(JsonConvert.SerializeObject(parameters)),
        };

        var response = await client.PredictAsync(request);
        var content = response.Predictions[0].StructValue.Fields["content"].StringValue;
        return content;
    }

    static async Task Main()
    {
        string prompt = "Give me ten interview questions for the role of program manager.";
        var content = await GenerateText(prompt);
        Console.WriteLine($"Content: {content}");
    }
}