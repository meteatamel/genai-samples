# VertexAI image generation, Cloud Run, C#

A VertexAI image generation sample to run on Cloud Run with C#.

Run [setup.sh](../scripts/setup.sh) to enable required services:

```bash
./scripts/setup.sh
```

## Test locally

Run locally:

```sh
dotnet run

...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://0.0.0.0:8080
```

Inside the [scripts](scripts) folder, run [test_local.sh](scripts/test.sh) to
test locally:

```sh
./test_local.sh

curl http://localhost:8080/\?images\=2\&prompt\=happy%20dogs
Created 2 image(s) with prompt: happy dogs%
```

## Deploy to Google Cloud

Run [deploy.sh](scripts/deploy.sh) to deploy to Google Cloud:

```sh
./scripts/deploy.sh
```

## Test in Google Cloud

Run [test_cloud.sh](scripts/test_cloud.sh) to test the service in Google Cloud:

```sh
./scripts/test_cloud.sh
```

## Cleanup

When you're done, you can cleanup created resources:

```sh
./scripts/cleanup.sh
```
