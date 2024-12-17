# GenAI Samples

A repository to collect GenAI related samples and links.

## Links

* [New: Google Gen AI SDKs - unified SDK to Gemini 2.0 Gemini Developer API (Google AI) and the Gemini API on Vertex AI](https://cloud.google.com/vertex-ai/generative-ai/docs/sdks/overview)
* [Official VertexAI GenAI code samples](https://cloud.google.com/vertex-ai/docs/samples?text=generative)
* [Vertex AI SDK and client libraries](https://cloud.google.com/vertex-ai/docs/start/client-libraries)
  * [Python reference](https://cloud.google.com/vertex-ai/docs/python-sdk/use-vertex-ai-python-sdk)
  * [C# reference](https://cloud.google.com/dotnet/docs/reference/Google.Cloud.AIPlatform.V1/latest)
  * [Go reference](https://cloud.google.com/go/docs/reference/cloud.google.com/go/aiplatform/latest/apiv1)
  * [Java reference](https://cloud.google.com/java/docs/reference/google-cloud-aiplatform/latest/overview)
  * [Node.js reference](https://cloud.google.com/nodejs/docs/reference/aiplatform/latest)
  * [Ruby reference](https://cloud.google.com/ruby/docs/reference/google-cloud-ai_platform/latest)

## Samples (In this repo)

| Sample | Showing | Running on | Language | Using |
| --- | --- | --- | --- | --- |
| [Sample](./vertexai/gemini2/hello-world/) | Gemini2 on Google AI and Vertex AI with Google Gen AI SDK | Console | Python | [Google Gen AI SDK](https://googleapis.github.io/python-genai/) |
| [Sample](./vertexai/gemini/console/csharp/sdk/) | VertexAI - Gemini Text from Text & Image | Console | C# | [Google.Cloud.AIPlatform.V1 library](https://cloud.google.com/dotnet/docs/reference/Google.Cloud.AIPlatform.V1/latest/Google.Cloud.AIPlatform.V1.PredictionServiceClient) |
| [Sample](./vertexai/gemini/console/csharp/rest/) | VertexAI - Gemini Text from Text & Image | Console | C# | [VertexAI REST API](https://cloud.google.com/vertex-ai/docs/reference/rest/v1/projects.locations.publishers.models) |
| [Sample](./vertexai/gemini/console/curl/) | VertexAI - Gemini Text from Text | Console | Curl | [VertexAI REST API](https://cloud.google.com/vertex-ai/docs/reference/rest/v1/projects.locations.publishers.models) |
| [Sample](./vertexai/palm2-text/console/curl/) | VertexAI - PaLM 2 for Text | Console | Curl | [VertexAI REST API](https://cloud.google.com/vertex-ai/docs/reference/rest/v1/projects.locations.publishers.models) |
| [Sample](./vertexai/palm2-text/console/csharp/rest) | VertexAI - PaLM 2 for Text | Console | C# | [VertexAI REST API](https://cloud.google.com/vertex-ai/docs/reference/rest/v1/projects.locations.publishers.models) |
| [Sample](./vertexai/palm2-text/console/csharp/sdk) | VertexAI - PaLM 2 for Text | Console | C# | [Google.Cloud.AIPlatform.V1 library](https://cloud.google.com/dotnet/docs/reference/Google.Cloud.AIPlatform.V1/latest/Google.Cloud.AIPlatform.V1.PredictionServiceClient) |
| [Sample](./vertexai/palm2-text/console/python/sdk)  | VertexAI - PaLM 2 for Text | Console | Python | [Vertex AI SDK for Python](https://cloud.google.com/python/docs/reference/aiplatform/latest/vertexai.language_models.TextGenerationModel) |
| [Sample](./vertexai/palm2-text/functions/csharp/rest) | VertexAI - PaLM 2 for Text | Cloud Functions | C# | [VertexAI REST API](https://cloud.google.com/vertex-ai/docs/reference/rest/v1/projects.locations.publishers.models) |
| [Sample](./vertexai/palm2-text/functions/python/sdk) | VertexAI - PaLM 2 for Text | Cloud Functions | Python | [Vertex AI SDK for Python](https://cloud.google.com/python/docs/reference/aiplatform/latest/vertexai.language_models.TextGenerationModel) |
| [Sample](./vertexai/chat/console/csharp/sdk/) | VertexAI - Chat | Console | C# | [Google.Cloud.AIPlatform.V1 library](https://cloud.google.com/dotnet/docs/reference/Google.Cloud.AIPlatform.V1/latest/Google.Cloud.AIPlatform.V1.PredictionServiceClient) |
| [Sample](./vertexai/imagegen/console/csharp/rest) | VertexAI - Imagegen | Console | C# | [VertexAI REST API](https://cloud.google.com/vertex-ai/docs/reference/rest/v1/projects.locations.publishers.models) |
| [Sample](./vertexai/imagegen/console/csharp/sdk) | VertexAI - Imagegen | Console | C# | [Google.Cloud.AIPlatform.V1 library](https://cloud.google.com/dotnet/docs/reference/Google.Cloud.AIPlatform.V1/latest/Google.Cloud.AIPlatform.V1.PredictionServiceClient) |
| [Sample](./vertexai/imagegen/console/python/rest) | VertexAI - Imagegen | Console | Python | [VertexAI REST API](https://cloud.google.com/vertex-ai/docs/reference/rest/v1/projects.locations.publishers.models) |
| [Sample](./vertexai/imagegen/console/python/sdk) | VertexAI - Imagegen | Console | Python | [Vertex AI SDK for Python](https://cloud.google.com/python/docs/reference/aiplatform/latest/vertexai.preview.vision_models.ImageGenerationModel) |
| [Sample](./vertexai/imagegen/run/csharp/rest) | VertexAI - Imagegen | Cloud Run | C# | [VertexAI REST API](https://cloud.google.com/vertex-ai/docs/reference/rest/v1/projects.locations.publishers.models) |
| [Sample](./vertexai/imagegen/run/python/sdk) | VertexAI - Imagegen | Cloud Run | Python | [Vertex AI SDK for Python](https://cloud.google.com/python/docs/reference/aiplatform/latest/vertexai.preview.vision_models.ImageGenerationModel) |
