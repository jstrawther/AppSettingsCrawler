using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ConsoleApp2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            foreach (string path in args)
            {
                if (File.Exists(path))
                {
                    // This path is a file
                    ProcessFile(path);
                }
                else if (Directory.Exists(path))
                {
                    // This path is a directory
                    ProcessDirectory(path);
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }
        }

        // Process all files in the directory passed in, recurse on any directories
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {            
            if (!(Path.GetFileName(path).StartsWith("appsettings") && Path.GetFileName(path).EndsWith("json"))) return;
            if (path.Contains(@"\bin\")) return;

            string json = File.ReadAllText(path);
            var document = System.Text.Json.JsonDocument.Parse(json, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });
            var root = document.RootElement;
            foreach(var obj in root.EnumerateObject())
            {
                Flatten(path, obj);
            }
        }

        private static void Flatten(string path, JsonProperty obj, string accumulator = "")
        {
            accumulator = string.IsNullOrEmpty(accumulator) ? obj.Name : accumulator + ":" + obj.Name;
            if(obj.Value.ValueKind == JsonValueKind.Object)
            {
                foreach (var sub in obj.Value.EnumerateObject())
                {
                    Flatten(path, sub, accumulator);
                }
            }
            else
            {
                Console.WriteLine(string.Join("|", path, accumulator));
            }
        }
    }
}
