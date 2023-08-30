import os
from vertexai.preview.vision_models import ImageGenerationModel


def generate_images(prompt, number_of_images):
    model = ImageGenerationModel.from_pretrained("imagegeneration")
    images = model.generate_images(
        prompt=prompt,
        number_of_images=number_of_images,
    )
    return images


def save_images(images, folder):
    if not os.path.exists(folder):
        os.makedirs(folder)

    for idx, img in enumerate(images):
        img_path = os.path.join(folder, f"image_{idx}.png")
        img.save(img_path)
        print(f"Saved {img_path}")


if __name__ == "__main__":
    images = generate_images("happy dogs", 2)
    save_images(images, "images")
