export class Attack {
  constructor(
    public name: string,
    public damage: number,
    public position: godot.Vector2,
    public knockbackForce: number,
    public stunTime: number
  ) {}
}
