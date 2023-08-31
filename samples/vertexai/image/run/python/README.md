# VertexAI image generation, Cloud Run, Python

A VertexAI image generation sample to run on Cloud Run with Python.

First, change `PROJECT_ID` in [config.sh](config.sh) to your own.

Run [setup.sh](setup.sh) to enable required services:

```bash
../setup.sh
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

```bash
./deploy.sh
```

Test in the cloud:

```sh
./test_cloud.sh
```
