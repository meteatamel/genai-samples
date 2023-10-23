public class Program
{
    static async Task Main(string[] args)
    {
        string prompt = "Give me ten interview questions for the role of program manager.";
        //var content = await Rest.GenerateText(prompt);
        var content = await Library.GenerateText(prompt);
        Console.WriteLine($"Content: {content}");
    }
}