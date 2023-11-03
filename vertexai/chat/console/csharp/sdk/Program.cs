using Google.Cloud.AIPlatform.V1;
using wkt = Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;

namespace PredictChatPromptSample;

public class Program
{
    static readonly string? ProjectId = Environment.GetEnvironmentVariable("PROJECT_ID");
    const string LocationId = "us-central1";

    static readonly string AiPlatformUrl = $"https://{LocationId}-aiplatform.googleapis.com";
    const string Publisher = "google";
    const string ModelName = "chat-bison@001";

    static async Task Main()
    {
        if (string.IsNullOrEmpty(ProjectId))
        {
            throw new Exception("Environment variable 'PROJECT_ID' not set.");
        }

        string prompt = "How many planets are there in the solar system?";
        string response = await PredictChatPrompt(prompt);
        Console.WriteLine($"Response: {response}");
    }

    private async static Task<string> PredictChatPrompt(string prompt)
    {
        // TODO: Show how to construct Protobuf directly
        var instanceJson = JsonConvert.SerializeObject(new
        {
            context = "My name is Miles. You are an astronomer, knowledgeable about the solar system.",
            examples = new[]
            {
                new
                {
                    input = new { content = "How many moons does Mars have?" },
                    output = new { content = "The planet Mars has two moons, Phobos and Deimos." }
                }
            },
            messages = new[]
            {
                new
                {
                    author = "user",
                    content = prompt
                }
            }
        });
        var instance = wkt::Value.Parser.ParseJson(instanceJson);

        // var parametersJson = JsonConvert.SerializeObject(new
        // {
        //     temperature = 0.3,
        //     maxDecodeSteps = 200,
        //     topP = 0.8,
        //     topK = 40
        // });
        // var parameters = wkt::Value.Parser.ParseJson(parametersJson);

        var parameters = new wkt::Value
        {
            StructValue = new wkt::Struct
            {
                Fields =
                {
                    { "temperature", new wkt::Value { NumberValue = 0.3 } },
                    { "maxDecodeSteps", new wkt::Value { NumberValue = 200 } },
                    { "topP", new wkt::Value { NumberValue = 0.8 } },
                    { "topK", new wkt::Value { NumberValue = 40 } }
                }
            }
        };

        PredictionServiceClient client = await new PredictionServiceClientBuilder
        {
            Endpoint = AiPlatformUrl
        }.BuildAsync();

        var request = new PredictRequest
        {
            EndpointAsEndpointName = EndpointName.FromProjectLocationPublisherModel(ProjectId, LocationId, Publisher, ModelName),
            Instances = { instance },
            Parameters = parameters,
        };

        var response = await client.PredictAsync(request);
        var content = response.Predictions[0].StructValue.Fields["candidates"].ListValue.Values[0].StructValue.Fields["content"].StringValue;
        return content;
    }
}