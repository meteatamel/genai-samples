# | VertexAI - Imagegen | Console | Python | VertexAI REST API |

Enable required services:

```sh
gcloud services enable aiplatform.googleapis.com
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

Install dependencies:

```sh
pip3 install -r requirements.txt
```

Run:

```bash
python3 imagegen.py
```
