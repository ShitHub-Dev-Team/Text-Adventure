namespace TextAdventure.NPCs;
public class HostileNPC : NPC
{
    public override string Name => Color.FORE_RED + base.Name + Color.RESET;
    public int AttackDamage { get; private init; }
    public int Health { get; set; }
    public event Program.Action OnDeath;
    public HostileNPC(string name, string description, int money, int damage, int health, string dialogue = "They simply stare at you.", Program.Action? onDeath = null) : base(name, description, money, dialogue)
    {
        AttackDamage = damage;
        Health = health;
        if (onDeath != null)
        {
            OnDeath += onDeath;
        }
    }

    public void Damage(int amount)
    {
        Health = Math.Max(0, Health - amount);

        if (Health != 0)
        {
            Console.WriteLine($"{Name} lost {amount} {Color.FORE_RED}HP{Color.RESET} and has {Health} {Color.FORE_RED}HP{Color.RESET} left!");
            return;
        }
        Console.WriteLine($"{Name} perished!    +{MoneyText}");
        Program.Player.Money += Money;
        OnDeath();
        Program.Player.CurrentRoom.NPCs.Remove(this);
    }
}