/*
[{
  "candidates": [
    {
      "content": {
        "role": "model",
        "parts": [
          {
            "text": "The sky is blue"
          }
        ]
      },
      "safetyRatings": [
        {
          "category": "HARM_CATEGORY_HARASSMENT",
          "probability": "NEGLIGIBLE"
        },
        {
          "category": "HARM_CATEGORY_HATE_SPEECH",
          "probability": "NEGLIGIBLE"
        },
        {
          "category": "HARM_CATEGORY_SEXUALLY_EXPLICIT",
          "probability": "NEGLIGIBLE"
        },
        {
          "category": "HARM_CATEGORY_DANGEROUS_CONTENT",
          "probability": "NEGLIGIBLE"
        }
      ]
    }
  ]
}]
*/
public class GeminiResponse
{
    public List<Candidate> Candidates { get; set; }
}

public class Candidate
{
    public Content Content { get; set; }
    public List<SafetyRating> SafetyRatings { get; set; }
}

public class Content
{
    public string Role { get; set; }
    public List<Part> Parts { get; set; }
}

public class Part
{
    public string Text { get; set; }
}

public class SafetyRating
{
    public string Category { get; set; }
    public string Probability { get; set; }
}