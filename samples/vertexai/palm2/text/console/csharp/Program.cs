public class Program
{
    static async Task Main(string[] args)
    {
        var text = await Rest.GenerateText("Give me ten interview questions for the role of program manager.");
        Console.WriteLine($"Response: {text}");
    }
}