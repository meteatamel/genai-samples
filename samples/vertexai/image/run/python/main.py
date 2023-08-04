# Copyright 2023 Google LLC
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#      http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and

import base64
import google.auth
import google.auth.transport.requests
import json
import os
import requests

from flask import Flask
from google.cloud import storage
from io import BytesIO
from PIL import Image

app = Flask(__name__)

PROJECT_ID = "genai-atamel"
LOCATION = "us-central1"

AI_PLATFORM_URL = f"https://{LOCATION}-aiplatform.googleapis.com"
IMAGE_MODEL_NAME = "imagegeneration"
PREDICT_URL = f"{AI_PLATFORM_URL}/v1/projects/{PROJECT_ID}/locations/{LOCATION}/publishers/google/models/{IMAGE_MODEL_NAME}:predict"

BUCKET_NAME = "genai-atamel-images"

def get_access_token():
    credentials, project_id = google.auth.default()
    auth_req = google.auth.transport.requests.Request()
    credentials.refresh(auth_req)
    return credentials.token

def generate_payload_json(access_token, prompt, **kwargs):
    negative_prompt = kwargs.get("negative_prompt")
    sample_count = kwargs.get("sample_count")
    sample_image_size = kwargs.get("sample_image_size")
    guidance_scale = kwargs.get("guidance_scale")
    seed = kwargs.get("seed")
    # base_image = kwargs.get("base_image")
    # mask = kwargs.get("mask")
    mode = kwargs.get("mode")

    headers = {
        "Authorization": f"Bearer {access_token}",
        "Content-Type": "application/json; charset=utf-8",
    }

    data = {
        "instances": [
            {
                "prompt": prompt,
            }
        ],
        "parameters": {
            "negativePrompt": negative_prompt,
            "sampleCount": sample_count,
            "sampleImageSize": sample_image_size,
            "seed": seed,
            "guidanceScale": guidance_scale,
            "mode": mode,
        }
    }

    return headers, json.dumps(data)

def send_request(headers, data):
    response = requests.post(PREDICT_URL, headers=headers, data=data)

    if response.status_code != 200:
        raise requests.exceptions.HTTPError(f"Error: {response.status_code} ({response.reason})")

    return response.json()

def generate_images(prompt, **kwargs):
    access_token = get_access_token()
    headers, data = generate_payload_json(access_token, prompt, **kwargs)
    response = send_request(headers, data)

    images = []
    if response:
        predictions = response.get("predictions")
        if predictions:
            for prediction in predictions:
                b64_decoded_string = base64.b64decode(prediction["bytesBase64Encoded"])
                img = Image.open(BytesIO(b64_decoded_string))
                images.append(img)
        else:
            print(f"No predictions for prompt: {prompt}")
    return images

def upload_images_to_gcs(images):
    # TODO: Create the bucket if it doesn't exist
    for idx, image in enumerate(images):
        file_name = f"image_{idx}.png"
        upload_image_to_gcs(image, file_name)

def upload_image_to_gcs(image, file_name, bucket_name=BUCKET_NAME):
    client = storage.Client()
    bucket = client.bucket(bucket_name)
    blob = bucket.blob(file_name)

    bytes_io = BytesIO()
    image.save(bytes_io, format='PNG')
    blob.upload_from_file(bytes_io, rewind=True, content_type="image/png")

    blob.make_public()
    file_url = blob.public_url
    print(f"Uploaded file: {file_name} to bucket: {bucket_name} with file url: {file_url}")
    return file_url

@app.route("/")
def hello_world():
    images = generate_images("happy dogs", sample_count=4)
    upload_images_to_gcs(images)
    return f"Created images {len(images)}!"


if __name__ == "__main__":
    app.run(debug=True, host="0.0.0.0", port=int(os.environ.get("PORT", 8080)))