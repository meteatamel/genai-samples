public class Program
{
    static async Task Main()
    {
        //await GenerateTextFromText.Generate();
        //await GenerateTextFromImageLocal.Generate();
        await GenerateTextFromImageGcs.Generate();
    }
}