using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

namespace Tries
{
  public class Trie
  {
    private readonly Node _root;

    public Trie()
    {
      _root = Factory.GetNodeBuilder().SetValue('^').SetDepth(0).Build();
    }

    public Node Prefix(string word)
    {
      var currentNode = _root;
      var result = currentNode;

      foreach (var letter in word)
      {
        currentNode = currentNode.FindChildNode(letter);
        if (currentNode == null)
          break;
        result = currentNode;
      }

      return result;
    }

    public bool Search(string word)
    {
      var prefix = Prefix(word);
      return prefix.Depth == word.Length && prefix.FindChildNode('$') != null;
    }

    public void InsertRange(List<string> items)
    {
      for (int i = 0; i < items.Count; i++)
        Insert(items[i]);
    }

    public void Insert(string word)
    {
      var commonPrefix = Prefix(word);
      var current = commonPrefix;

      for (var i = current.Depth; i < word.Length; i++)
      {
        var newNode = Factory.GetNodeBuilder().SetValue(word[i]).SetDepth(current.Depth + 1).SetParent(current).Build();
        current.Children.Add(newNode);
        current = newNode;
      }

      // Add last node in branch
      current.Children.Add(Factory.GetNodeBuilder().SetValue('$').SetDepth(current.Depth + 1).SetParent(current).Build());
    }

    public void InsertLayers(List<string> layers)
    {
      InsertLayersHelper(_root, layers);
    }

    private void InsertLayersHelper(Node node, List<string> layers)
    {
      if (node.IsLeaf() && node.Depth < layers.Count)
      {
        foreach (var letter in layers[node.Depth])
        {
          node.Children.Add(Factory.GetNodeBuilder().SetValue(letter).SetDepth(node.Depth + 1).SetParent(node).Build());
        }
      }

      foreach (var current in node.Children)
      {
        InsertLayersHelper(current, layers);
      }

    }

    public void Delete(string word)
    {
      if (Search(word))
      {
        var node = Prefix(word).FindChildNode('$');

        while (node.IsLeaf())
        {
          var parent = node.Parent;
          parent.DeleteChildNode(node.Value);
          node = parent;
        }
      }
    }

    public void Display()
    {
      DisplayHelper(_root, "");
    }

    public void OutputToFile(string fileName, string filePath)
    {
      var sb = new StringBuilder();
      SetToStringBuilder(_root, sb, "");
      if (!Directory.Exists(filePath))
      {
        Directory.CreateDirectory(filePath);
      }
      using(FileStream fs = File.Create($"{filePath}/{fileName}"))
      {
        byte[] info = new UTF8Encoding(true).GetBytes(sb.ToString());
        fs.Write(info, 0, info.Length);
      }
    }

    private void SetToStringBuilder(Node node, StringBuilder sb, string str)
    {
      if (node.IsLeaf())
      {
        sb.Append(str).AppendLine();
      }

      int i = 0;
      foreach (var current in node.Children)
      {
        i++;
        if (current.Depth == 1)
        {
          str = "";
        }

        if (i > 1 && str.Length > 0)
        {
          str = str.Remove(str.Length - 1, 1);
        }

        if (current.Value != '$')
        {
          str += current.Value;
        }
        SetToStringBuilder(current, sb, str);
      }
    }

    private void DisplayHelper(Node node, string str)
    {
      if (node.IsLeaf())
      {
        Console.WriteLine(str);
      }

      int i = 0;
      foreach (var current in node.Children)
      {
        i++;
        if (current.Depth == 1)
        {
          str = "";
        }

        if (i > 1 && str.Length > 0)
        {
          str = str.Remove(str.Length - 1, 1);
        }

        if (current.Value != '$')
        {
          str += current.Value;
        }
        DisplayHelper(current, str);
      }
    }
  }
}