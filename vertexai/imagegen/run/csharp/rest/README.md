# | VertexAI - Imagegen | Cloud Run | C# | [VertexAI REST API]

Enable required services:

```sh
gcloud services enable \
  aiplatform.googleapis.com \
  artifactregistry.googleapis.com \
  cloudbuild.googleapis.com \
  run.googleapis.com
```

Login:

```sh
gcloud auth application-default login
```

Set your `PROJECT_ID` and `REGION`:

```sh
export PROJECT_ID=genai-atamel
export REGION=us-central1
```

Create a bucket for images:

```sh
export BUCKET_NAME=${PROJECT_ID}-images
gsutil mb -l $REGION gs://${BUCKET_NAME}
```

Run locally:

```sh
dotnet run
```

Test locally:

```bash
curl http://localhost:8080/\?images\=2\&prompt\=happy%20dogs
Created 2 image(s) with prompt: happy dogs%
```

Deploy to cloud:

```sh
SERVICE_NAME=vertexai-imagegen-run-dotnet6

gcloud run deploy $SERVICE_NAME \
  --allow-unauthenticated \
  --region $REGION \
  --source .. \
  --set-env-vars PROJECT_ID=$PROJECT_ID,REGION=$REGION,BUCKET_NAME=$BUCKET_NAME
```

Test in the cloud:

```sh
URL=$(gcloud run services describe $SERVICE_NAME --region=$REGION --format 'value(status.url)')
curl $URL
```
