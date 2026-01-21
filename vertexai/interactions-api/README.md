# Interactions API - Unified API for models and agents

An example that shows how to use [Interactions API](https://ai.google.dev/gemini-api/docs/interactions), the new unified API for models and agents

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

If you want to use Google AI API:

```sh
export GOOGLE_API_KEY=your-api-key
```

> [!WARNING]  
> Interactions API is currently not supported on Google Cloud API but when it is supported, you can try to run the samples
> against Vertex AI by pointing to your Google Cloud project.

If you want to use Google Cloud API:

```sh
export GOOGLE_GENAI_USE_VERTEXAI=true
export GOOGLE_CLOUD_PROJECT=your-project-id
export GOOGLE_CLOUD_LOCATION=global # Required for preview models
```

## Run samples

TODO

## References

* [Interactions API docs](https://ai.google.dev/gemini-api/docs/interactions)
* [Blog: Interactions API: A unified foundation for models and agents](https://blog.google/innovation-and-ai/technology/developers-tools/interactions-api/)