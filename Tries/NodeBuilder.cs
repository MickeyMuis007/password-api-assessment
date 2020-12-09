using System.Collections.Generic;

namespace Tries {
  public class NodeBuilder {
    public char Value { get; private set; }
    public Node Parent { get; private set; }
    public List<Node> Children { get; private set; }
    public int Depth { get; private set; }

    public NodeBuilder() {
      Children = new List<Node>();
      Parent = null;
    }

    public NodeBuilder SetValue(char value) {
      Value = value;
      return this;
    }

    public NodeBuilder SetParent(Node parentNode) {
      Parent = parentNode;
      return this;
    }

    public NodeBuilder SetChildren(List<Node> children) {
      Children = children;
      return this;
    }

    public NodeBuilder SetDepth(int depth) {
      Depth = depth;
      return this;
    }

    public Node Build() {
      return new Node(this);
    }
  }
}