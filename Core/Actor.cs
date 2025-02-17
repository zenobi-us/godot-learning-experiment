using System;
using Godot;

namespace Objects
{

  public partial class Actor : CharacterBody2D
  {
    public override void _EnterTree()
    {
      this.AddToGroup("Actors");
      GD.Print("Adding Actor to Tree");
      // this.set_notify_transform(true);
    }

    public override void _ExitTree()
    {
      this.RemoveFromGroup("Actors");
      GD.Print("Removing Actor from Tree");
    }
  }
}

// export class Actor extends godot.CharacterBody2D {
//   _enter_tree(): void {
//     this.add_to_group("Actors");
//     // this.set_notify_transform(true);
//   }

//   @onready("Actor")
//   input: godot.Vector2;

//   _exit_tree(): void {
//     this.remove_from_group("Actors");
//   }
// }

// export function get_component<T>(
//   owner: godot.Node | godot.Node2D,
//   component: string
// ): T | null {
//   return owner.get_meta(component, null) as T;
// }

// export function has_component(
//   owner: godot.Node | godot.Node2D,
//   component: string
// ): boolean {
//   return owner.has_meta(component);
// }
