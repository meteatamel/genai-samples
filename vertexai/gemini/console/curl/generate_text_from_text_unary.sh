#!/bin/bash

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
# limitations under the License.

PROJECT_ID="genai-atamel"
LOCATION="us-central1"
API_ENDPOINT=${LOCATION}-aiplatform.googleapis.com
MODEL_ID="gemini-pro"

curl -X POST -H "Authorization: Bearer $(gcloud auth print-access-token)" \
    -H "Content-Type: application/json"  \
    https://${API_ENDPOINT}/v1/projects/${PROJECT_ID}/locations/${LOCATION}/publishers/google/models/${MODEL_ID}:generateContent -d \
    $'{
      "contents": {
        "role": "USER",
        "parts": { "text": "Why is the sky blue?" }
      },
      "generation_config":{
        "temperature": 0.4,
        "top_p": 1,
        "top_k": 32,
        "max_output_tokens": 2048
      }
  }'


