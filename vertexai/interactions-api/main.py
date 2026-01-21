"""
These samples demonstrates how to use the new Interactions API.
"""
import time
import wave
import base64
import sys
from google import genai

MODEL = "gemini-3-flash-preview"
MODEL_TTS = "gemini-2.5-flash-preview-tts"
MODEL_AGENT = "deep-research-pro-preview-12-2025"

# Basic interactions

def basic_generate_content():
    """Generate text using the old generate_content method."""
    client = genai.Client()

    response = client.models.generate_content(
        model=MODEL,
        contents="Tell me a short joke about programming."
    )
    print(response.text)


def basic_interactions():
    """Generate text using the new Interactions API."""
    client = genai.Client()

    interaction = client.interactions.create(
        model=MODEL,
        input="Tell me a short joke about programming."
    )

    print(interaction.outputs[-1].text)

# Conversation

def chat_stateful():
    """Demonstrate stateful chat using the new Interactions API."""
    client = genai.Client()

    # 1. First turn
    prompt = "Hi, my name is Mete."
    print(f"User: {prompt}")
    interaction1 = client.interactions.create(
        model=MODEL,
        input=prompt
    )
    print(f"Model: {interaction1.outputs[-1].text}")

    # 2. Second turn (passing previous_interaction_id)
    prompt = "What is my name?"
    print(f"User: {prompt}")
    interaction2 = client.interactions.create(
        model=MODEL,
        input=prompt,
        previous_interaction_id=interaction1.id
    )
    print(f"Model: {interaction2.outputs[-1].text}")


def chat_stateless():
    """Demonstrate stateless chat using the new Interactions API."""
    client = genai.Client()

    prompt = "What are the three largest cities in Spain?"
    print(f"User: {prompt}")

    conversation_history = [
        {
            "role": "user",
            "content": prompt
        }
    ]

    interaction1 = client.interactions.create(
        model=MODEL,
        input=conversation_history
    )

    print(f"Model: {interaction1.outputs[-1].text}")

    conversation_history.append({"role": "model", "content": interaction1.outputs})

    prompt = "What is the most famous landmark in the second one?"
    print(f"User: {prompt}")

    conversation_history.append({
        "role": "user",
        "content": prompt
    })
    
    interaction2 = client.interactions.create(
        model=MODEL,
        input=conversation_history
    )

    print(f"Model: {interaction2.outputs[-1].text}")


# Multimodal understanding

def image_understanding():
    """Demonstrate image understanding using the new Interactions API."""
    client = genai.Client()

    interaction = client.interactions.create(
        model=MODEL,
        input=[
            {"type": "text", "text": "Describe the image."},
            {
                "type": "image",
                "uri": "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg",
                "mime_type": "image/png"
            }
        ]
    )
    print(interaction.outputs[-1].text)

# TODO: This doesn't work for some reason
def audio_understanding():
    """Demonstrate audio understanding using the new Interactions API."""
    client = genai.Client()

    interaction = client.interactions.create(
        model=MODEL,
        input=[
            {"type": "text", "text": "What is the audio about?"},
            {
                "type": "audio",
                "uri": "https://commons.wikimedia.org/wiki/File:Alcal%C3%A1_de_Henares_(RPS_13-04-2024)_canto_de_ruise%C3%B1or_(Luscinia_megarhynchos)_en_el_Soto_del_Henares.wav",
                "mime_type": "audio/wav"
            }
        ]
    )
    print(interaction.outputs[-1].text)


def video_understanding():
    """Demonstrate video understanding using the new Interactions API."""
    client = genai.Client()

    interaction = client.interactions.create(
        model=MODEL,
        input=[
            {"type": "text", "text": "Describe the video."},
            {
                "type": "video",
                "uri": "https://youtu.be/98DcoXwGX6I",
                "mime_type": "video/mp4"
            }
        ]
    )
    print(interaction.outputs[-1].text)

def pdf_understanding():
    """Demonstrate PDF understanding using the new Interactions API."""
    client = genai.Client()

    interaction = client.interactions.create(
        model=MODEL,
        input=[
            {"type": "text", "text": "Describe the PDF."},
            {
                "type": "document",
                "uri": "https://arxiv.org/pdf/2303.10130.pdf",
                "mime_type": "application/pdf"
            }
        ]
    )
    print(interaction.outputs[-1].text)


# Multimodal generation

# TODO: This might not work for your country
def image_generation():
    """Demonstrate image generation using the new Interactions API."""
    client = genai.Client()

    interaction = client.interactions.create(
        model=MODEL,
        input="Generate an image of a cat.",
        response_modalities=["image"],
        generation_config={
            "image_config": {
                "aspect_ratio": "9:16",
                "image_size": "2k"
            }
        }
    )

    for output in interaction.outputs:
        if output.type == "image":
            print(f"Generated image with mime_type: {output.mime_type}")
            with open("cat.png", "wb") as f:
                f.write(base64.b64decode(output.data))


def audio_generation():
    """Demonstrate audio generation using the new Interactions API."""
    client = genai.Client()

    interaction = client.interactions.create(
        model=MODEL_TTS,
        input="Say the following: WOOHOO This is so much fun!",
        response_modalities=["audio"],
        generation_config={
            "speech_config": {
                "language": "en-us",
                "voice": "kore"
            }
        }
    )

    for output in interaction.outputs:
        if output.type == "audio":
            print(f"Generated audio with mime_type: {output.mime_type}")
            with wave.open("generated_audio.wav", "wb") as wf:
                wf.setnchannels(1)
                wf.setsampwidth(2)
                wf.setframerate(24000)
                wf.writeframes(base64.b64decode(output.data))


def audio_multi_speaker_generation():
    """Demonstrate audio generation using the new Interactions API."""
    client = genai.Client()

    interaction = client.interactions.create(
        model=MODEL_TTS,
        input="""TS the following conversation between Alice and Bob.
        Alice: Hi Bob, how are you doing today? 
        Bob: I'm doing great, thanks for asking! How about you? 
        Alice: Fantastic! I just learned about the Gemini API.""",
        response_modalities=["audio"],
        generation_config= {
            "speech_config": [
                {"voice": "Zephyr", "speaker": "Alice", "language": "en-US"},
                {"voice": "Puck", "speaker": "Bob", "language": "en-US"}
            ]
        }
    )

    for output in interaction.outputs:
        if output.type == "audio":
            print(f"Generated audio with mime_type: {output.mime_type}")
            with wave.open("generated_multi_speaker_audio.wav", "wb") as wf:
                wf.setnchannels(1)
                wf.setsampwidth(2)
                wf.setframerate(24000)
                wf.writeframes(base64.b64decode(output.data))

# Agents


def agent():
    """Demonstrate agent using the new Interactions API."""
    client = genai.Client()

    prompt = "Research the history of the Google TPUs with a focus on 2025 and 2026."
    print(f"User: {prompt}")

    # 1. Start the Deep Research Agent
    initial_interaction = client.interactions.create(
        input=prompt,
        agent=MODEL_AGENT,
        background=True
    )

    print(f"Research started. Interaction ID: {initial_interaction.id}")

    # 2. Poll for results
    while True:
        interaction = client.interactions.get(initial_interaction.id)
        print(f"Status: {interaction.status}")

        if interaction.status == "completed":
            print("\nFinal Report:\n", interaction.outputs[-1].text)
            break
        if interaction.status in ["failed", "cancelled"]:
            print(f"Failed with status: {interaction.status}")
            break

        time.sleep(10)

if __name__ == '__main__':
    globals()[sys.argv[1]]()
