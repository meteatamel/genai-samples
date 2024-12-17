# Gemini2 on Google AI and Vertex AI with Google Gen AI SDK

An example that shows how to use [Google Gen AI SDK](https://cloud.google.com/vertex-ai/generative-ai/docs/sdks/overview),
the new unified SDK to Gemini 2.0 for Gemini on Google AI and Vertex AI.

## Before you start

Create and activate a Python virtual env:

```sh
python -m venv .venv
source .venv/bin/activate
```

Install dependencies:

```sh
pip install -r requirements.txt
```

## Google AI

Set your API key:

```sh
export GOOGLE_AI_API_KEY=your-api-key
```

Run:

```sh
python main.py google_ai
```

## Vertex AI

This example shows how to talk to Gemini on Vertex AI.

Enable required services:

```sh
gcloud services enable aiplatform.googleapis.com
```

Login:

```sh
gcloud auth application-default login
```

Set your project id:

```sh
export GOOGLE_CLOUD_PROJECT_ID=genai-atamel
```

Run:

```sh
python main.py vertex_ai
```

## References

* [Google Gen AI SDKs](https://cloud.google.com/vertex-ai/generative-ai/docs/sdks/overview)
* [Google Gen AI SDK Python](https://googleapis.github.io/python-genai/)
* [Developer’s guide to getting started with Gemini 2.0 Flash on Vertex AI](https://medium.com/google-cloud/developers-guide-to-getting-started-with-gemini-2-0-flash-on-vertex-ai-6b4fe3c6899f)