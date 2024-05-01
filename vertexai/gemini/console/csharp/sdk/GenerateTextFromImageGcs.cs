using Google.Api.Gax.Grpc;
using Google.Cloud.AIPlatform.V1;

public class GenerateTextFromImageGcs
{
    const string ProjectId = "genai-atamel";
    const string Location = "us-central1";
    const string Publisher = "google";

    const string Model_Gemini_1_0 = "gemini-1.0-pro-vision-001";
    const string Model_Gemini_1_5 = "gemini-1.5-pro-preview-0409";

    const string Model = Model_Gemini_1_5;

    public async static Task Generate()
    {
        // Create client
        var predictionServiceClient = new PredictionServiceClientBuilder
        {
            Endpoint = $"{Location}-aiplatform.googleapis.com"
        }.Build();

        // Prompt
        string prompt = "Describe this image in detail";
        string imageUrl = "gs://cloud-samples-data/generative-ai/image/320px-Felis_catus-cat_on_snow.jpg";
        Console.WriteLine($"Prompt: {prompt}");
        Console.WriteLine($"ImageUrl: {imageUrl}");

        // Initialize request argument(s)
        var content = new Content
        {
            Role = "USER"
        };
        content.Parts.AddRange(
        [
            new() {
                Text = prompt
            },
            new() {
                FileData = new() {
                    MimeType = "image/png",
                    FileUri = imageUrl
                }
            }
        ]);

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