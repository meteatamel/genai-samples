using System.Diagnostics;
using Google.Cloud.AIPlatform.V1;
using Google.Cloud.VertexAI.Extensions;
using Microsoft.Extensions.AI;

// Snippets that show how to use Google.Cloud.VertexAI.Extensions library
// Vertex AI implementation of Microsoft.Extensions.AI

string projectId = Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT") ?? throw new InvalidOperationException("GOOGLE_CLOUD_PROJECT environment variable is not set");
string location = Environment.GetEnvironmentVariable("GOOGLE_CLOUD_LOCATION") ?? "us-central1";

await IChatClient_BasicRequestResponse();
// await IChatClient_BasicRequestResponse_Streaming();
// await IChatClient_ChatExample();
// await IChatClient_ChatExample_Streaming();
// await IEmbeddingGenerator_EmbedSeveralInputs();
// await ImageGenerator_GenerateImage();

async Task IChatClient_BasicRequestResponse()
{
    IChatClient client = await new PredictionServiceClientBuilder()
        .BuildIChatClientAsync(EndpointName.FormatProjectLocationPublisherModel(projectId, location, "google",
            "gemini-3-flash-preview"));

    var response = await client.GetResponseAsync("Why is sky blue?");
    Console.WriteLine(response.Text);
}

async Task IChatClient_BasicRequestResponse_Streaming()
{
    IChatClient client = await new PredictionServiceClientBuilder()
        .BuildIChatClientAsync(EndpointName.FormatProjectLocationPublisherModel(projectId, location, "google",
            "gemini-3-flash-preview"));

    await foreach (ChatResponseUpdate update in client.GetStreamingResponseAsync("Why is sky blue?"))
    {
        Console.Write(update.Text);
    }
}

async Task IChatClient_ChatExample()
{
    IChatClient client = await new PredictionServiceClientBuilder()
        .BuildIChatClientAsync(EndpointName.FormatProjectLocationPublisherModel(projectId, location, "google",
            "gemini-3-flash-preview"));

    List<ChatMessage> history = [];
    while (true)
    {
        Console.Write("User: ");
        history.Add(new(ChatRole.User, Console.ReadLine()));

        var response = await client.GetResponseAsync(history);
        Console.WriteLine($"AI: {response}");

        history.AddMessages(response);
    }
}

async Task IChatClient_ChatExample_Streaming()
{
    IChatClient client = await new PredictionServiceClientBuilder()
        .BuildIChatClientAsync(EndpointName.FormatProjectLocationPublisherModel(projectId, location, "google",
            "gemini-3-flash-preview"));

    List<ChatMessage> history = [];
    while (true)
    {
        Console.Write("User: ");
        history.Add(new(ChatRole.User, Console.ReadLine()));

        List<ChatResponseUpdate> updates = [];
        await foreach (ChatResponseUpdate update in client.GetStreamingResponseAsync(history))
        {
            Console.Write(update);
            updates.Add(update);
        }
        Console.WriteLine();

        history.AddMessages(updates);
    }
}


async Task IEmbeddingGenerator_EmbedSeveralInputs()
{
    IEmbeddingGenerator<string, Embedding<float>> generator = await new PredictionServiceClientBuilder()
        .BuildIEmbeddingGeneratorAsync(EndpointName.FormatProjectLocationPublisherModel(projectId, location,
            "google", "gemini-embedding-001"));

    GeneratedEmbeddings<Embedding<float>> embeddings = await generator.GenerateAsync(["Hello", "World"]);
    foreach (Embedding<float> embedding in embeddings)
    {
        Console.WriteLine(string.Join(",", embedding.Vector.ToArray()));
    }
}

#pragma warning disable MEAI001
async Task ImageGenerator_GenerateImage()
{
    IImageGenerator generator = await new PredictionServiceClientBuilder()
        .BuildIImageGeneratorAsync(EndpointName.FormatProjectLocationPublisherModel(projectId, location, "google",
            "imagen-4.0-fast-generate-001"));

    ImageGenerationResponse response = await generator.GenerateImagesAsync("A cute baby sea otter");

    foreach (var image in response.Contents.OfType<DataContent>())
    {
        string path = $"{Path.GetRandomFileName()}.png";
        File.WriteAllBytes(path, image.Data.Span);
        Console.WriteLine($"Image saved to {path}");
        Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
    }
}
#pragma warning restore MEAI001

