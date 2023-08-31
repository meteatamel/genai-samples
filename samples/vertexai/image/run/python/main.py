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

import os
from flask import Flask, request
from google.cloud import storage
from io import BytesIO
from vertexai.preview.vision_models import ImageGenerationModel

app = Flask(__name__)

BUCKET_NAME = "genai-atamel-images"


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
    image._pil_image.save(bytes_io, format='PNG')
    blob.upload_from_file(bytes_io, rewind=True, content_type="image/png")

    blob.make_public()
    file_url = blob.public_url
    print(f"Uploaded file: {file_name} to bucket: {bucket_name} with file url: {file_url}")
    return file_url


def generate_images(prompt, number_of_images):
    model = ImageGenerationModel.from_pretrained("imagegeneration")
    images = model.generate_images(
        prompt=prompt,
        number_of_images=number_of_images,
    )
    return images


@app.route("/")
def main():
    number_of_images = request.args.get("images", default=1, type=int)
    prompt = request.args.get("prompt", default="happy dogs", type=str)

    images = generate_images(prompt, number_of_images)
    upload_images_to_gcs(images)
    return f"Created {number_of_images} image(s) with prompt: {prompt}"


if __name__ == "__main__":
    app.run(debug=True, host="0.0.0.0", port=int(os.environ.get("PORT", 8080)))