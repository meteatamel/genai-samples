using Google.Api.Gax.Grpc;
using Google.Cloud.AIPlatform.V1;

public class GenerateTextFromText
{
    const string ProjectId = "genai-atamel";
    const string Location = "us-central1";
    const string Publisher = "google";
    const string Model = "gemini-pro";

    public async static Task Generate()
    {
        // Create client
        var predictionServiceClient = new PredictionServiceClientBuilder
        {
            Endpoint = $"{Location}-aiplatform.googleapis.com"
        }.Build();

        // Prompt
        string prompt = "Why is the sky blue?";
        Console.WriteLine($"Prompt: {prompt}");

        // Initialize request argument(s)
        var content = new Content
        {
            Role = "USER"
        };
        content.Parts.Add(new Part()
        {
            Text = prompt
        });

        var generateContentRequest = new GenerateContentRequest
        {
            Model = $"projects/{ProjectId}/locations/{Location}/publishers/{Publisher}/models/{Model}",
            GenerationConfig = new GenerationConfig
            {
                Temperature = 0.4f,
                TopP = 1,
                TopK = 32,
                MaxOutputTokens = 2048
            }
        };
        generateContentRequest.Contents.Add(content);

        // Make the request, returning a streaming response
        using PredictionServiceClient.StreamGenerateContentStream response = predictionServiceClient.StreamGenerateContent(generateContentRequest);

        Console.Write($"Response: ");

        // Read streaming responses from server until complete
        AsyncResponseStream<GenerateContentResponse> responseStream = response.GetResponseStream();
        while (await responseStream.MoveNextAsync())
        {
            GenerateContentResponse responseItem = responseStream.Current;
            Console.WriteLine(responseItem.Candidates[0].Content.Parts[0].Text);
        }
    }
}