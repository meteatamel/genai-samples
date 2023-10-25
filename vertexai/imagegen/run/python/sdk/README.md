# | VertexAI - Imagegen | Cloud Run | Python | Vertex AI SDK for Python |

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

Create a bucket for images:

```sh
export BUCKET_NAME=${PROJECT_ID}-images
gsutil mb -l us-central1 gs://${BUCKET_NAME}
```

Install dependencies:

```bash
pip3 install -r requirements.txt
```

Run locally:

```bash
python3 main.py
```

Test locally:

```bash
curl http://localhost:8080/?images=2&prompt=happy%20cats
```

Deploy to cloud:

```sh
SERVICE_NAME=vertexai-imagegen-run-python311

gcloud run deploy $SERVICE_NAME \
  --allow-unauthenticated \
  --region $REGION \
  --source . \
  --set-env-vars BUCKET_NAME=$BUCKET_NAME
```

Test in the cloud:

```sh
URL=$(gcloud run services describe $SERVICE_NAME --region=$REGION --format 'value(status.url)')
curl $URL
```
