using System;
// EntityManager.cs (Updated)
using Godot;
using System.Collections.Generic;
using System.Linq;

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


        /**
         * Programatically add a child node as a component
         */
        public T AddComponent<T>(Node entity, T component) where T : core.BaseComponent
        {
            if (component == null)
            {
                GD.PushError("A component is required.");
                return null; // Or throw an exception: throw new System.ArgumentNullException(nameof(node), "Node cannot be null.");
            }

            // An entity is only allowed one un-identifiable component.
            if (component.Id == null && HasComponent<T>(entity))
            {
                GD.Print($"Component {typeof(T).Name} already exists on {entity}");
                return GetComponent<T>(entity);
            }
            // All component ids on an entity must be unique.
            if (HasComponent<T>(entity, component.Id))
            {
                GD.Print($"Identified component {component.Id} already exists on entity {entity}");
                return GetComponent<T>(entity, component.Id);
            }

            entity.AddChild(component);
            GD.Print($"Component of type {typeof(T).Name} added to node {entity.Name}."); // Add log for debugging
            return component;
        }

        public void RemoveComponent<T>(Node entity) where T : core.BaseComponent
        {
            if (entity == null)
            {
                return;
            }

            var component = GetComponent<T>(entity);

            if (component == null)
            {
                return;
            }

            entity.RemoveChild(component);
        }

        public void RemoveComponent<T>(Node entity, string id) where T : core.BaseComponent
        {
            if (entity == null)
            {
                return;
            }

            var component = GetComponent<T>(entity, id);

            if (component == null)
            {
                return;
            }

            entity.RemoveChild(component);
        }

        // Get a component node of type T from an entity
        public T GetComponent<T>(Node entity) where T : core.BaseComponent
        {

            return GetComponents<T>(entity).FirstOrDefault();
        }
        public T GetComponent<T>(Node entity, string id) where T : core.BaseComponent
        {

            return GetComponents<T>(entity, id).FirstOrDefault();
        }

        // Returns multiple components on an entity that match
        public IEnumerable<T> GetComponents<T>(Node entity) where T : core.BaseComponent
        {
            return entity.GetChildren().OfType<T>();
        }
        // Returns multiple components on an entity that match
        public IEnumerable<T> GetComponents<T>(Node entity, string id) where T : core.BaseComponent
        {
            return GetComponents<T>(entity).Where(x => x.Id == id);
        }

        // Check if an entity has a component of type T
        public bool HasComponent<T>(Node entity) where T : core.BaseComponent
        {
            return GetComponent<T>(entity) != null;
        }
        // Check if an entity has a idenfiable component of type T
        public bool HasComponent<T>(Node entity, string id) where T : core.BaseComponent
        {
            return GetComponents<T>(entity, id).Count() > 0;
        }

        // Get all entities with specific component types
        public List<Node> GetEntitiesWithComponents(params System.Type[] componentTypes)
        {
            return _entities
                .Where(entity => componentTypes
                    .All(type => entity
                        .GetChildren()
                        .Any(child => type.IsInstanceOfType(child))))
                .ToList();

        }
    }

}
