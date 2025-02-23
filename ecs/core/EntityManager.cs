// EntityManager.cs (Updated)
using Godot;
using System.Collections.Generic;

namespace core
{

    public partial class EntityManager : Node
    {
        private List<Node> _entities = new();

        public override void _Ready()
        {
            // Register entities from the scene
            foreach (Node node in GetTree().GetNodesInGroup("entities"))
            {
                if (node.GetParent() == GetParent()) // Only direct children of Main
                {
                    RegisterEntity(node);
                }
            }
        }

        // Register an entity
        public void RegisterEntity(Node node)
        {
            if (!_entities.Contains(node))
            {
                _entities.Add(node);
            }
        }

        // Get a component node of type T from an entity
        public T GetComponent<T>(Node entity) where T : Node
        {
            foreach (Node child in entity.GetChildren())
            {
                if (child is T component)
                {
                    return component;
                }
            }
            return null;
        }

        // Check if an entity has a component of type T
        public bool HasComponent<T>(Node entity) where T : Node
        {
            return GetComponent<T>(entity) != null;
        }

        // Get all entities with specific component types
        public List<Node> GetEntitiesWithComponents(params System.Type[] componentTypes)
        {
            List<Node> matchingEntities = new();
            foreach (Node entity in _entities)
            {
                bool hasAll = true;
                foreach (var type in componentTypes)
                {
                    bool hasComponent = false;
                    foreach (Node child in entity.GetChildren())
                    {
                        if (type.IsInstanceOfType(child))
                        {
                            hasComponent = true;
                            break;
                        }
                    }
                    if (!hasComponent)
                    {
                        hasAll = false;
                        break;
                    }
                }
                if (hasAll)
                {
                    matchingEntities.Add(entity);
                }
            }
            return matchingEntities;
        }
    }

}
