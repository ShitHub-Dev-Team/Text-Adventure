namespace TextAdventure.Items;
public class WeaponItem : Item
{
    public int Damage { get; private init; }
    internal WeaponItem(string name, string description, double weight, int value, int damage)
        : base(name, description, weight, value)
    {
        Name = Color.FORE_LIGHT_PURPLE + name + Color.RESET;
        Damage = damage;
    }
}
public static class Weapon
{
    public static Item Glock19 = new WeaponItem(
        "Glock 19",
        "Its a Glock 19. Debug weapon. So you can one-shot enemies. Infinite ammo included.",
        0.8,
        0,
        100
    );
    public static Item WoodenClub = new WeaponItem(
        "Wooden Club",
        "A thick piece of wood. Perfect for bonking someine on the head with it.",
        2,
        4,
        4
    );
    public static Item IronSword = new WeaponItem(
        "Iron Sword",
        "The sharpened blade looks like it could pierce through a stone wall.",
        2.5,
        50,
        12
    );
}