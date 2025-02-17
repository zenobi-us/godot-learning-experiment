using Godot;
using System;

namespace Core
{

  public partial class ComponentSearch
  {

    public static T GetComponent<T>(Node node, string name) where T : Node
    {
      T component = (T)node.GetMeta(name);
      if (component == null)
      {
        throw new Exception($"Component {name} not found on node {node}");
      }

      return component;
    }

    public static bool HasComponent<T>(Node node, string name) where T : Node
    {
      bool hasComponent = (T)node.GetMeta(name) != null;
      return hasComponent;
    }
  }
}
