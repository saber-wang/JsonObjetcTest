// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

byte[] _newlineDelimiter = Encoding.UTF8.GetBytes("\n");
var asyncJsonObject = ReadJsonObjectsAsync(new JsonNodeOptions { PropertyNameCaseInsensitive = true });

using System.IO.MemoryStream memory = new();

try
{
    await foreach (var jsonObjetct in asyncJsonObject)
    {
        Console.WriteLine($"PrivateMemorySize64:{Process.GetCurrentProcess().PrivateMemorySize64}");
        await memory.WriteAsync(System.Text.Encoding.UTF8.GetBytes(jsonObjetct.ToJsonString(new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        })));
        await memory.WriteAsync(_newlineDelimiter);
        await memory.FlushAsync();
        Console.WriteLine($"PrivateMemorySize64:{Process.GetCurrentProcess().PrivateMemorySize64}");
    }
   
    Console.WriteLine($"PrivateMemorySize64:{Process.GetCurrentProcess().PrivateMemorySize64}");
}
catch (OutOfMemoryException ex)
{

    Console.WriteLine($"PrivateMemorySize64:{Process.GetCurrentProcess().PrivateMemorySize64}");
    Console.WriteLine(ex.ToString());
    throw;
}

static async IAsyncEnumerable<JsonObject?> ReadJsonObjectsAsync(JsonNodeOptions? nodeOptions = null)
{
    DirectoryInfo directoryInfo = new("jsons");

    foreach (var fileInfo in directoryInfo.EnumerateFiles())
    {

        using var file = fileInfo.OpenRead();
        Console.WriteLine($"PrivateMemorySize64:{Process.GetCurrentProcess().PrivateMemorySize64}");
        //var filebyte = await file.ReadToEndAsync();
        yield return JsonNode.Parse(file, nodeOptions)?.AsObject();
    }
}