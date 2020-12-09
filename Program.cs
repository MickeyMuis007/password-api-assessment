using System.Collections.Generic;
using Tries;

namespace password_api_assessment
{
    class Program
    {
        static void Main(string[] args)
        {
            var items = new List<string>() { "password" , "Password", "pas5word", "p@ssword", "P@ssword" };
            var trie = new Trie();
            trie.InsertRange(items);
            trie.Display();
        }
    }
}
