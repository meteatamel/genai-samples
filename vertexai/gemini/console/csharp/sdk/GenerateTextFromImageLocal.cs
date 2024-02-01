using Google.Api.Gax.Grpc;
using Google.Cloud.AIPlatform.V1;

public class GenerateTextFromImageLocal
{
    const string ProjectId = "genai-atamel";
    const string Location = "us-central1";
    const string Publisher = "google";
    const string Model = "gemini-pro-vision";

    // This sample assumes that you have a local image.jpg file
    // Download an image from Google Cloud Storage
    // gsutil cp "gs://cloud-samples-data/generative-ai/image/320px-Felis_catus-cat_on_snow.jpg" ./image.jpg

    public async static Task Generate()
    {
        // Create client
        var predictionServiceClient = new PredictionServiceClientBuilder
        {
            Endpoint = $"{Location}-aiplatform.googleapis.com"
        }.Build();

        // Prompt
        string prompt = "Describe this image in detail";
        string imagePath = "image.jpg";
        Console.WriteLine($"Prompt: {prompt}");
        Console.WriteLine($"Image: {imagePath}");

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
                InlineData = new() {
                    MimeType = "image/png",
                    Data = Google.Protobuf.ByteString.CopyFrom(File.ReadAllBytes(imagePath))

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