# | VertexAI - PaLM 2 for Text | Cloud Functions | Python | | Vertex AI SDK for Python |

Enable required services:

```sh
gcloud services enable \
  aiplatform.googleapis.com \
  artifactregistry.googleapis.com \
  cloudbuild.googleapis.com \
  cloudfunctions.googleapis.com \
  run.googleapis.com
```

Login:

```sh
gcloud auth application-default login
```

Install dependencies:

```sh
pip3 install -r requirements.txt
```

Run locally:

```sh
functions-framework --target=hello_http --debug
```

Test locally:

```sh
curl http://localhost:8080
```

Deploy to cloud:

```sh
SERVICE_NAME=vertexai-palm2-text-function-python311

gcloud functions deploy $SERVICE_NAME \
  --allow-unauthenticated \
  --entry-point hello_http \
  --gen2 \
  --region $REGION \
  --runtime python311 \
  --source . \
  --trigger-http
```

Test in the cloud:

```sh
gcloud functions call $SERVICE_NAME \
  --gen2 \
  --region $REGION
```
