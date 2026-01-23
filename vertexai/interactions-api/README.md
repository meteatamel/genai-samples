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

Run the samples using `python main.py <sample-name>`.

### Basic interactions

```sh
# Generate text using the old generate_content method
python main.py basic_generate_content

# Generate text using the new Interactions API
python main.py basic_interaction

# Generate text using the new Interactions API in a streaming fashion
python main.py basic_interaction_stream
```

### Conversation

```sh
# Stateful chat using the new Interactions API
python main.py chat_stateful

# Stateless chat using the new Interactions API
python main.py chat_stateless
```

### Multimodal understanding

```sh
# Image understanding
python main.py image_understanding

# Audio understanding
python main.py audio_understanding

# Video understanding
python main.py video_understanding

# PDF understanding
python main.py pdf_understanding
```

### Multimodal generation

```sh
# Image generation
python main.py image_generation

# Audio generation
python main.py audio_generation

# Audio multi-speaker generation
python main.py audio_multi_speaker_generation
```

### Agents

```sh
# Deep Research Agent
python main.py agent
```

### Built-in tools

```sh
# Grounding with Google Search
python main.py tool_google_search

# MCP server
python main.py tool_mcp
```

## References

* [Interactions API docs](https://ai.google.dev/gemini-api/docs/interactions)
* [Blog: Interactions API: A unified foundation for models and agents](https://blog.google/innovation-and-ai/technology/developers-tools/interactions-api/)