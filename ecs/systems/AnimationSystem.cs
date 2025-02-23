using Godot;
using System;
using System.Collections.Generic;

namespace systems
{


    public partial class AnimationSystem : core.BaseSystem
    {

        public AnimationSystem()
        {
            requiredComponents.Add(typeof(components.RenderableComponent));
        }

        public override void _Ready()
        {
            base._Ready();
            GD.Print("AnimationSystem ready");
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            List<Node> entities = GetEntities();


            foreach (Node entity in entities)
            {


                components.RenderableComponent renderable = _entityManager.GetComponent<components.RenderableComponent>(entity);

                if (renderable.RenderNode == null)
                {
                    continue;
                }

                if (renderable.RenderNode.Animation == renderable._animation)
                {
                    continue;
                }

                renderable.RenderNode.Play(renderable._animation);
            }
        }
    }
}
