using System.Collections.Generic;

namespace Tries {
  public class Node {
    public char Value { get; private set; }
    public List<Node> Children { get; private set; }
    public Node Parent { get; private set; }
    public int Depth { get; private set; }

    public Node(NodeBuilder nodeBuilder) {
      Value = nodeBuilder.Value;
      Parent = nodeBuilder.Parent;
      Children = nodeBuilder.Children;
      Depth = nodeBuilder.Depth;      
    }

    public bool IsLeaf()
    {
      return Children.Count == 0;
    }

    public Node FindChildNode(char searchChar) {
      foreach (var child in Children) {
        if (child.Value == searchChar)
          return child;
      }
      return null;
    }

    public void DeleteChildNode (char searchChar) {
      for (var i = 0; i < Children.Count; i++) {
        if (Children[i].Value == searchChar)
          Children.RemoveAt(i);
      }
    }
  }
}