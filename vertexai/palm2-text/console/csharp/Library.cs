using Google.Cloud.AIPlatform.V1;
using wkt = Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;

public class Library
{
    // Adjust for your project
    const string ProjectId = "genai-atamel";
    const string Location = "us-central1";

    const string AiPlatformUrl = $"https://{Location}-aiplatform.googleapis.com";
    const string Publisher = "google";
    const string ModelName = "text-bison";

    public async static Task<string> GenerateText(string prompt)
    {
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
            EndpointAsEndpointName = EndpointName.FromProjectLocationPublisherModel(ProjectId, Location, Publisher, ModelName),
            Instances = { wkt::Value.Parser.ParseJson(JsonConvert.SerializeObject(instance)) },
            Parameters = wkt::Value.Parser.ParseJson(JsonConvert.SerializeObject(parameters)),
        };

        var response = await client.PredictAsync(request);
        var content = response.Predictions[0].StructValue.Fields["content"].StringValue;
        return content;
    }
}