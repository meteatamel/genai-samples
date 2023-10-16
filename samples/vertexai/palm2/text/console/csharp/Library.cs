using Google.Cloud.AIPlatform.V1;
using wkt = Google.Protobuf.WellKnownTypes;

public class Library
{
    // Adjust for your project
    const string ProjectId = "genai-atamel";
    const string Location = "us-central1";
    const string Publisher = "google";
    const string ModelName = "text-bison";

    public async static Task Test()
    {
        var instance = wkt::Value.Parser.ParseJson("{ \"prompt\": " + "\"rocky road cookies\"}");
        var parameters = wkt::Value.Parser.ParseJson("{\n"
                + " \"temperature\": 0.2,\n"
                + " \"maxOutputTokens\": 1024,\n"
                + " \"topP\": 0.95,\n"
                + " \"topK\": 40\n"
                + "}");

        var predictionServiceClient = await PredictionServiceClient.CreateAsync();
        var endpointName = EndpointName.FromProjectLocationPublisherModel(ProjectId, Location, Publisher, ModelName);
        var request = new PredictRequest
        {
            EndpointAsEndpointName = endpointName,
            Instances = { instance },
            Parameters = parameters,
        };
        var predictResponse = await predictionServiceClient.PredictAsync(request);
    }
}