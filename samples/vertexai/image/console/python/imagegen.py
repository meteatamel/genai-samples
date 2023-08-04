import base64
import google.auth
import google.auth.transport.requests
import json
import os
import requests
from io import BytesIO
from PIL import Image

PROJECT_ID = "quizrd-prod-382117"
LOCATION = "us-central1"

AI_PLATFORM_URL = f"https://{LOCATION}-aiplatform.googleapis.com"
IMAGE_MODEL_NAME = "imagegeneration"
PREDICT_URL = f"{AI_PLATFORM_URL}/v1/projects/{PROJECT_ID}/locations/{LOCATION}/publishers/google/models/{IMAGE_MODEL_NAME}:predict"

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

def save_images(images, folder):
    if not os.path.exists(folder):
        os.makedirs(folder)

    for idx, img in enumerate(images):
        img_path = os.path.join(folder, f"image_{idx}.png")
        img.save(img_path)
        print(f"Saved {img_path}")

if __name__ == "__main__":
    images = generate_images("happy dogs", sample_count=2)
    save_images(images, "images")
