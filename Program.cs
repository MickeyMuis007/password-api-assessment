using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using Tries;

namespace password_api_assessment
{
    class Program
    {
        static void Main(string[] args)
        {
            var layers = new List<string>() { "pP" , "aA@", "sS5", "sS5", "oO0", "rR", "dD" };
            var trieLayer = new Trie();

            trieLayer.InsertLayers(layers);
            // trieLayer.Display();
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fileName = "dict.txt";
            trieLayer.OutputToFile(fileName, path);
            Console.WriteLine($"File stored at: {path}/{fileName}");
        }
    }
}
