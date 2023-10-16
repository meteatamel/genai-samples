using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

// Adjust for your project
const string PROJECT_ID = "genai-atamel";
const string REGION = "us-central1";

if (string.IsNullOrEmpty(REGION) || string.IsNullOrEmpty(PROJECT_ID))
{
    throw new Exception("Environment variable 'REGION' or 'PROJECT_ID' not set.");
}

string apiUrl = $"https://{REGION}-aiplatform.googleapis.com/v1/projects/{PROJECT_ID}/locations/{REGION}/publishers/google/models/text-bison:predict";

var payload = new
{
    instances = new[]
    {
        new
        {
            prompt = "Give me ten interview questions for the role of program manager."
        }
    },
    parameters = new
    {
        temperature = 0.2,
        maxOutputTokens = 256,
        topK = 40,
        topP = 0.95
    }
};

GoogleCredential credential = GoogleCredential.GetApplicationDefault();
//var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
var handler = credential.ToDelegatingHandler(new HttpClientHandler());
using HttpClient httpClient = new(handler);
//httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

HttpResponseMessage response = await httpClient.PostAsync(apiUrl,
    new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

string responseBody = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Response: {response.StatusCode}");
Console.WriteLine(responseBody);