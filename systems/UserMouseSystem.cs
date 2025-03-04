using components;
using core;
using Godot;

namespace systems
{
    /**
     * This system exists to decouple various systems from accessing the mouse position as a 
     * specific thing. 
     *
     * Instead the goal is that other systems can now just treat the mouse position as some generic position of "some" entity.
     *
     */
    [GlobalClass]
    public partial class UserMouseSystem : BaseSystem
    {
        public static string MouseEntityName = "UserMouse";
        private Node userMouse;

        public override void _Ready()
        {
            base._Ready();

            // On initialisation
            // we need to create a UserMouse entity and add a PositionComponent
            Node entity = this._entityManager.CreateEntity(MouseEntityName);

            this._entityManager.AddComponent(entity, new NameableComponent(MouseEntityName));
            this._entityManager.AddComponent(entity, new UserMouseComponent());

            userMouse = entity;

        }

        /**
         * On every tick, update this entity's positionComponent with the mouse position
         */
        public override void _Process(double delta)
        {
            base._Process(delta);

            var positionComponent = _entityManager.GetComponent<PositionComponent>(userMouse);
            var mousePosition = GetViewport().GetMousePosition();

            positionComponent.Position = mousePosition;

        }

    }
}
