# GENERATED FILE - DO NOT EDIT (source lives in common folder)
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

source $(dirname $0)/config.sh

echo "Enable required services for Cloud Run"
gcloud services enable \
  artifactregistry.googleapis.com \
  cloudbuild.googleapis.com \
  run.googleapis.com

if [ "$SERVICE_TYPE" = "functions" ]
then
echo "Enable required services for Cloud Functions"
gcloud services enable \
  cloudfunctions.googleapis.com
fi

echo "Enable required services for VertexAI"
gcloud services enable  \
  aiplatform.googleapis.com

echo "Application default login (needed for local runs)"
gcloud auth application-default login
