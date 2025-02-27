using System;
using System.Collections.Generic;
using behaviours;
using BehaviourTree;
using BehaviourTree.Composites;
using BehaviourTree.Decorators;
using components;
using core;
using Godot;

namespace entities
{

    public partial class BehaviourTreeUI : Tree
    {
        [Export]
        public NodePath EntityPath { get; set; }
        private Node _entity { get; set; }

        [Export]
        public NodePath EntityManagerPath { get; set; }

        private EntityManager _entityManager { get; set; }

        private BehaviourTreeComponent _btComponent { get; set; }

        public override void _Ready()
        {
            _entityManager = GetNode<EntityManager>(EntityManagerPath);
            _entity = GetNode<Node>(EntityPath);
            _btComponent = _entityManager.GetComponent<BehaviourTreeComponent>(_entity);

            HideRoot = true;
            GD.Print("Dislaying BehaviourTree", _btComponent.Behaviour);

            RenderBehaviourTree(this.GetRoot(), _btComponent.Behaviour);
        }

        public override void _PhysicsProcess(double delta)
        {
            UpdateTree(this.GetRoot());
        }


        private void RenderBehaviourTree(TreeItem parent, IBehaviour<BehaviourTreeBlackboardContext> behaviour)
        {
            RenderBehaviourTree(parent, (dynamic)behaviour);
        }
        private void RenderBehaviourTree(TreeItem parent, CompositeBehaviour<BehaviourTreeBlackboardContext> obj)
        {
            var item = RenderInternal(parent, obj);

            foreach (var child in obj.Children)
            {
                RenderBehaviourTree(item, child);
            }
        }

        private void RenderBehaviourTree(TreeItem parent, DecoratorBehaviour<BehaviourTreeBlackboardContext> obj)
        {
            RenderInternal(parent, obj);
            RenderBehaviourTree(parent, obj.Child);
        }

        private void RenderBehaviourTree(TreeItem parent, BaseBehaviour<BehaviourTreeBlackboardContext> obj)
        {
            RenderInternal(parent, obj);
        }


        private TreeItem RenderInternal(TreeItem parent, IBehaviour<BehaviourTreeBlackboardContext> obj)
        {
            var item = CreateItem(parent);
            var meta = new TreeItemMeta(obj);

            GD.Print($"Created Tree Item for ${obj}:${obj.GetHashCode()}");

            item.SetMetadata(0, meta);
            item.SetEditable(0, false);

            item.SetText(0, meta.Name);
            item.SetCustomColor(0, meta.Color);

            return item;
        }
        private void UpdateTree(TreeItem item)
        {

            UpdateInternal(item);

            TreeItem child = item.GetFirstChild();
            while (child != null)
            {
                UpdateInternal(child);
                child = child.GetNextInTree();
            }
        }

        private void UpdateInternal(TreeItem item)
        {
            TreeItemMeta meta = item.GetMetadata(0).As<TreeItemMeta>();
            item.SetCustomColor(0, meta.Color);
        }

    }

    public static class TreeHelpers
    {
        public static string GetName(IBehaviour<BehaviourTreeBlackboardContext> obj)
        {
            if (!string.IsNullOrWhiteSpace(obj.Name))
            {
                return obj.Name;
            }

            var type = obj.GetType();

            return type.Name;
        }


        private static readonly Color ReadyBrush = Colors.DarkGray;
        private static readonly Color RunningBrush = Colors.SkyBlue;
        private static readonly Color SuccessBrush = Colors.Green;
        private static readonly Color FailureBrush = Colors.Red;

        public static Color GetColor(BehaviourStatus status)
        {
            switch (status)
            {
                case BehaviourStatus.Ready: return ReadyBrush;
                case BehaviourStatus.Running: return RunningBrush;
                case BehaviourStatus.Succeeded: return SuccessBrush;
                case BehaviourStatus.Failed: return FailureBrush;
                default: throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }

    public partial class TreeItemMeta : GodotObject
    {
        public IBehaviour<BehaviourTreeBlackboardContext> Behaviour { get; }
        public string Name;

        public Color Color
        {
            get => TreeHelpers.GetColor(Behaviour.Status);
        }

        public TreeItemMeta(IBehaviour<BehaviourTreeBlackboardContext> obj)
        {
            this.Name = TreeHelpers.GetName(obj);
            this.Behaviour = obj;
        }

    }

}
