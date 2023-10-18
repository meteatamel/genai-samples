public class Program
{
    static async Task Main(string[] args)
    {
        //var images = await Rest.GenerateImages("happy dogs", 2);
        var images = await Library.GenerateImages("happy dogs", 2);
        await SaveImages(images, "images");
    }

    public static async Task SaveImages(List<Image> images, string folder)
    {
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        for (int idx = 0; idx < images.Count; idx++)
        {
            string imgPath = Path.Combine(folder, $"image_{idx}.png");

            using FileStream fs = File.OpenWrite(imgPath);
            await images[idx].SaveAsPngAsync(fs);
            Console.WriteLine($"Saved {imgPath}");
        }
    }
}