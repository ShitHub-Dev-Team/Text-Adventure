using TextAdventure.Items;

namespace TextAdventure;

public class Player
{
    private string? _name = string.Empty;
    public string Name
    {
        get
        {
            return Color.FORE_LIGHT_CYAN + _name + Color.RESET;
        }
    }
    public double Hp { get; private set; }
    public double MaxHp { get; private set; }
    public int Money { get; set; } = 10;
    public WeaponItem EquippedWeapon { get; set; } = Weapon.Glock19 as WeaponItem;
    public ArmorItem? EquippedArmor { get; set; } = null;

    public string MoneyText => Color.FORE_WHITE + '$' + Color.FORE_GREEN + Money.ToString() + Color.RESET;

    public Room CurrentRoom { get; set; }
    public InventoryList Inventory { get; set; }
    public bool InCombat = false;
    public Player(string name, double maxHP, double maxWeight, Room startRoom)
    {
        _name = name;
        MaxHp = maxHP;
        Hp = maxHP;
        Inventory = new(maxWeight);
        CurrentRoom = startRoom;
    }
    public void Status()
    {
        Console.Write(Name + " is ");
        double percentage = Hp / MaxHp;
        if(Hp < 15)
        {
            Console.WriteLine(Color.BACK_RED + "nearly dead");
        }
        else if(percentage < 0.3)
        {
            Console.WriteLine(Color.FORE_LIGHT_RED + "badly injured");
        }
        else if(percentage < 0.5)
        {
            Console.WriteLine(Color.FORE_ORANGE + "injured");
        }
        else if(percentage < 0.75)
        {
            Console.WriteLine(Color.FORE_LIGHT_YELLOW + "lightly injured");
        }
        else
        {
            Console.WriteLine(Color.FORE_LIGHT_GREEN + "feeling fine");
        }
        Console.Write(Color.RESET);

        Console.WriteLine($"Carrying {Color.FORE_WHITE}{Inventory.Count}{Color.FORE_CYAN} items " + Color.RESET + "and " + MoneyText + ' ' +
            $"with a total weight of {Color.BACK_WHITE}{Color.FORE_BLACK}{Math.Round(Inventory.InventoryWeight, 2)}/{Math.Round(Inventory.MaxInventoryWeight, 2)}{Color.RESET} kg");
        
        Console.WriteLine($"Wielding {EquippedWeapon.Name}({Color.FORE_WHITE}{EquippedWeapon.Damage} {Color.FORE_RED}ATK{Color.RESET}) and wearing " +
                          (EquippedArmor != null 
                              ? $"{EquippedArmor.Name}({Color.FORE_WHITE}{EquippedArmor.Defense} {Color.FORE_BLUE}DEF{Color.RESET})" 
                              : $"{Color.FORE_ORANGE}Nothing{Color.RESET}({Color.FORE_WHITE}0 {Color.FORE_BLUE}DEF{Color.RESET})") 
                          + "!");
    }

    public void Heal(double ammount)
    {
        Hp = Math.Min(MaxHp, Hp + ammount);
    }

    public double Damage(double ammount)
    {
        double actualDamage = Math.Max(1, ammount - (EquippedArmor != null ? EquippedArmor.Defense : 0));
        Hp = Math.Max(0, Hp - actualDamage);
        return actualDamage;
    }
    public void HealthUp(int ammount)
    {
        MaxHp += ammount;
    }
}