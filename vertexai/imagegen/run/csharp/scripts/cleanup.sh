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

if [ "$SERVICE_TYPE" = "functions" ]
then
  echo "Delete $SERVICE_NAME in $REGION"
  gcloud functions delete $SERVICE_NAME \
    --gen2 \
    --region $REGION
elif [ "$SERVICE_TYPE" = "run" ]
then
  echo "Delete $SERVICE_NAME in $REGION"
  gcloud run services delete $SERVICE_NAME \
    --region $REGION
fi

