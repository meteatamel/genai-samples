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

export PROJECT_ID=genai-atamel # Change to your project
export REGION=us-central1

export FUNCTION_NAME=vertexai-imagegen
export SERVICE_TYPE=run
export RUNTIME=python311
export SERVICE_NAME=$FUNCTION_NAME-$SERVICE_TYPE-$RUNTIME