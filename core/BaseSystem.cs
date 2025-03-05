using Godot;
using System;
using System.Collections.Generic;

namespace core
{

    // Base system class - all systems inherit from this
    public partial class BaseSystem : Node, ISystem
    {
        [Export]
        public NodePath EntityManagerPath { get; set; }

        public core.EntityManager _entityManager { get; set; }


        public List<Type> requiredComponents { get; set; }

        public BaseSystem()
        {
            requiredComponents = new List<Type>();
        }

        public override void _Ready()
        {
            base._Ready();
            _entityManager = GetNode<core.EntityManager>(EntityManagerPath);

        }

        public virtual List<Node> GetEntities()
        {

            List<Node> entities = _entityManager.GetEntitiesWithComponents(
                requiredComponents.ToArray()
            );

            return entities;
        }

    }

    public interface ISystem
    {
        public NodePath EntityManagerPath { get; set; }

        public core.EntityManager _entityManager { get; set; }

        public List<Type> requiredComponents { get; set; }

        public List<Node> GetEntities();

    }
}
