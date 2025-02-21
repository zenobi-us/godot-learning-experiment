using Godot;

namespace Components
{
  public class Attack
  {

    public string Name { get; set; }
    public int Damage { get; set; }
    public Vector2 Position { get; set; }
    public float KnockbackForce { get; set; }
    public float StunTime { get; set; }

    public Attack(string name, int damage, Vector2 position, float knockbackForce, float stunTime)
    {
      Name = name;
      Damage = damage;
      Position = position;
      KnockbackForce = knockbackForce;
      StunTime = stunTime;
    }
  }

}
