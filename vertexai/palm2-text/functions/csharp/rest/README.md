# | VertexAI - PaLM 2 for Text | Cloud Functions | C# | VertexAI REST API |

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

Set your `PROJECT_ID` and `REGION`:

```sh
PROJECT_ID=genai-atamel
REGION=us-central1
```

Build:

```sh
dotnet build
```

Run locally:

```sh
dotnet run
```

Test locally:

```bash
curl http://localhost:8080
```

Deploy to cloud:

```sh
SERVICE_NAME=vertexai-palm2-text-function-dotnet6

gcloud functions deploy vertexai-palm2-text-function-dotnet6 \
  --allow-unauthenticated \
  --entry-point GenAI.Function \
  --gen2 \
  --region $REGION \
  --runtime dotnet6 \
  --source . \
  --trigger-http \
  --set-env-vars PROJECT_ID=$PROJECT_ID,REGION=$REGION
```

Test in the cloud:

```sh
gcloud functions call $SERVICE_NAME \
  --gen2 \
  --region $REGION
```
