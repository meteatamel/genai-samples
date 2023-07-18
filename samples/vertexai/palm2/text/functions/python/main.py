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

import functions_framework
from vertexai.preview.language_models import TextGenerationModel

def interview(temperature: float = .2) -> str:
    """Ideation example with a Large Language Model"""

    parameters = {
        "temperature": temperature,
        "max_output_tokens": 256,
        "top_p": .8,
        "top_k": 40,
    }

    model = TextGenerationModel.from_pretrained("text-bison@001")
    response = model.predict(
        'Give me ten interview questions for the role of program manager.',
        **parameters,
    )

    return response.text

@functions_framework.http
def hello_http(request):
    return f"Response from Model: \n{interview()}"