# VertexAI Palm2 text, Cloud Functions, C#

A VertexAI Palm2 text sample to run on Cloud Functions with C#.

First, change `PROJECT_ID` in [config.sh](config.sh) to your own.

Run [setup.sh](setup.sh) to enable required services:

```bash
../setup.sh
```

Build:

```bash
dotnet build
```

Run locally:

```bash
source config.sh
dotnet run
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
