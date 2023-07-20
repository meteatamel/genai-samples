# VertexAI Palm2 text, Cloud Functions, Python

A VertexAI Palm2 text sample to run on Cloud Functions with Python.

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
functions-framework --target=hello_http --debug
```

Test locally:

```bash
curl http://localhost:8080
```

Deploy to cloud:

```bash
./deploy.sh
```

Test in the cloud:

```sh
./test_cloud.sh
```
