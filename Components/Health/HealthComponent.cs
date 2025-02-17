using Godot;


namespace Components
{

  public partial class HealthComponent : Node
  {

    [Export]
    public int health = 100;

    [Export]
    public int maxHealth = 100;

    [Signal]
    public delegate void OnHealthDepletedEventHandler();

    public override void _Ready() { }
    public override void _Process(double delta) { }

    public void takeDamage(int damage)
    {
      this.health -= damage;
      if (this.health <= 0)
      {
        this.health = 0;
        // HealthComponent.OnHealthDepleted.emit(this);
        // this.OnHealthDepleted.emit();
      }
    }

    public void heal(int amount)
    {
      this.health += amount;
      if (this.health > this.maxHealth) { this.health = this.maxHealth; }
    }
    public int getHealth() { return this.health; }
    public int getMaxHealth() { return this.maxHealth; }
  }
}

