using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

// Set them with 'source ../../config.sh'
string? PROJECT_ID = Environment.GetEnvironmentVariable("PROJECT_ID");
string? LOCATION = Environment.GetEnvironmentVariable("LOCATION");

if (string.IsNullOrEmpty(LOCATION) || string.IsNullOrEmpty(PROJECT_ID))
{
    throw new Exception("Environment variable 'LOCATION' or 'PROJECT_ID' not set.");
}

string apiUrl = $"https://{LOCATION}-aiplatform.googleapis.com/v1/projects/{PROJECT_ID}/locations/{LOCATION}/publishers/google/models/text-bison:predict";

// TODO: This works but is it the right auth method?
GoogleCredential credential = GoogleCredential.GetApplicationDefault();
var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

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

using (HttpClient httpClient = new HttpClient())
{
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    HttpResponseMessage response = await httpClient.PostAsync(apiUrl,
        new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

    string responseBody = await response.Content.ReadAsStringAsync();
    Console.WriteLine($"Response: {response.StatusCode}");
    Console.WriteLine(responseBody);
}