import os
import sys
from google import genai

MODEL = 'gemini-2.0-flash-exp'

def get_api_key():
    api_key = os.environ.get("GOOGLE_AI_API_KEY")
    if not api_key:
        raise ValueError("GOOGLE_AI_API_KEY environment variable is not set")
    return api_key


def google_ai():
    client = genai.Client(api_key=get_api_key())

    response = client.models.generate_content(
        model=MODEL,
        contents='Why is sky blue?'
    )
    print(response.text)


def get_project_id():
    project_id = os.environ.get("GOOGLE_CLOUD_PROJECT_ID")
    if not project_id:
        raise ValueError(
            "GOOGLE_CLOUD_PROJECT_ID environment variable is not set")
    return project_id


def vertex_ai():
    client = genai.Client(
        vertexai=True,
        project=get_project_id(),
        location='us-central1'
    )

    response = client.models.generate_content(
        model=MODEL,
        contents='Why is sky blue?'
    )
    print(response.text)


if __name__ == '__main__':
    globals()[sys.argv[1]]()
