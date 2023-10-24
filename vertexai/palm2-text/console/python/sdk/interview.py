import os
import vertexai
from vertexai.preview.language_models import TextGenerationModel

def interview(temperature: float = .2) -> None:

    # Set them with 'source ../config.sh'
    project = os.environ['PROJECT_ID']
    region = os.environ['REGION']

    # This is needed if you don't want to rely on `gcloud auth login`
    vertexai.init(project=project, location=region)

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
    print(f"Response from Model: {response.text}")

if __name__ == "__main__":
    interview()