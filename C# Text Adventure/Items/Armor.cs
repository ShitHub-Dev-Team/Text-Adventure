namespace TextAdventure.Items;
public class ArmorItem : Item
{
    public int Defense { get; private init; }
    internal ArmorItem(string name, string description, double weight, int value, int defense)
        : base(name, description, weight, value)
    {
        Name = Color.FORE_ORANGE + name + Color.RESET;
        Defense = defense;
    }
}
public static class Armor
{
    public static Item LeatherArmor = new ArmorItem(
        "Leather Armor",
        "A sturdy set of leather armor.\nProvides nearly no protection against attacks.",
        3,
        10,
        4
    );
    public static Item ChainmailArmor = new ArmorItem(
        "Chainmail Armor",
        "Armor made of lots of tiny metal rings.\nProvides great protection against slashes while not impacting mobility.",
        3.5,
        40,
        7
    );
    public static Item SteelPlateArmor = new ArmorItem(
        "Steel Plate Armor",
        "Armor made of big metal plates.\nProvides excellent protection against most kinds of meelee attacks and some projectiles.",
        8,
        110,
        14
    );
}