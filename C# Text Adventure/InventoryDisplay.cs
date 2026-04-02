namespace TextAdventure;
using TextAdventure.Items;
using TextAdventure.NPCs;
using static Color;
public static class InventoryDisplay
{
    private static Player p = Program.Player!;
    private static string InfoText = string.Empty;
    private static int SelectedItem = 0;
    private static int SelectedInfo = 0;
    private static bool InfoMode { get; set; } = false;
    private static List<string> ConsoleBuffer = new();

    private const string HIGHLIGHT_COLOR = BACK_WHITE;
    private const string INFO_BOX_OUTLINE_COLOR = FORE_LIGHT_YELLOW;
    private const string TEXT_BOX_OUTLINE_COLOR = FORE_WHITE;

    public static void InventoryLoop()
    {
        DrawInventory();
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                case ConsoleKey.Escape:
                    if (InfoMode)
                    {
                        InfoMode = false;
                        SelectedInfo = 0;
                        break;
                    }

                    InfoText = string.Empty; // Clear InfoText so the Info does not show up when re-entering the inventory.
                    Console.Clear();
                    return;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    if (InfoMode)
                    {
                        SelectedInfo++;
                        break;
                    }
                    SelectedItem = Math.Clamp(SelectedItem + 1, 0, p.Inventory.Count - 1);
                    break;
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    if (InfoMode)
                    {
                        SelectedInfo--;
                        break;
                    }
                    SelectedItem = Math.Clamp(SelectedItem - 1, 0, p.Inventory.Count - 1);
                    break;
                case ConsoleKey.Enter:
                    if (InfoMode)
                    {
                        switch(SelectedInfo)
                        {
                            case 0:
                                Use();
                                break;
                            case 1:
                                Drop();
                                break;
                            case 2:
                                Sell();
                                break;
                        }
                        SelectedItem--;
                        InfoMode = false;
                    }
                    else
                    {
                        InfoMode = true;
                    }
                    break;
                }

                if(p.Inventory.Count == 0)
                {
                    Console.Clear();
                    return;
                } 

                SelectedInfo = Math.Clamp(SelectedInfo, 0, 2);
                DrawInventory();
            }
        }
    }
    private static void DrawInventory()
    {
        while (Console.WindowHeight < 19 || Console.WindowWidth < 35)
        {
            Console.Clear();
            Boxing.WindowTooSmall();
            Thread.Sleep(2000);
        }

        Console.Clear();
        ConsoleBuffer.Clear();

        int itemDisplayCount = Console.WindowHeight - 5; // Ammount of items that can be displayed without scrolling, -2 for roof, -2 for floor, -1 for empty line (so the roof does not get pusehd out of the visible console).

        ConsoleBuffer.Add(Boxing.WindowCeiling(Console.WindowWidth - 4));  //
        ConsoleBuffer.Add(Boxing.WindowWall("", Console.WindowWidth - 4)); // Top Border

        for (int i = 0; i < itemDisplayCount; i++) // If the Player has more items than can be displayed, a scenario known as a "Skill Issue" occurs.
        {
            if (!(i < p.Inventory.Count))    // If there are no more items to display, fill with empty lines.
            {
                ConsoleBuffer.Add(Boxing.WindowWall("", Console.WindowWidth - 4));
                continue;
            }
            ConsoleBuffer.Add(Boxing.WindowWall($"{(i == SelectedItem ? HIGHLIGHT_COLOR : "")}{p.Inventory[i].Name}{RESET}", Console.WindowWidth - 4)); // Display the item, if it's selected, highlight it. The highlighting color sucks.
        }

        ConsoleBuffer.Add(Boxing.WindowWall("", Console.WindowWidth - 4)); // Bottom Border
        ConsoleBuffer.Add(Boxing.WindowFloor(Console.WindowWidth - 4));                //



        int descriptionLines = Console.WindowHeight - 10 - 2; // Lines available for description, - 10 for interactions and roof, - 2 for floor.
        int infoBoxTotalWidth = Math.Min(Convert.ToInt32((Console.WindowWidth - 4) / 2.5), 60); // Total width of info box, should be 1/5 of the console but never more than 60 characters wide.
        int infoBoxInnerWidth = infoBoxTotalWidth - 4; // Inner width of info box, without walls and space.
        if (InfoMode)    // Draw Info Window
        {
            // Info Box Drawing
            int insertionIndex = Console.WindowWidth - infoBoxTotalWidth - 2;

            ConsoleBuffer[1] = ConsoleBuffer[1].OverwriteAt(Boxing.WindowCeiling(infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex);                                                        // Info Roof
            ConsoleBuffer[2] = ConsoleBuffer[2].OverwriteAt(Boxing.WindowWall("", infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex);                                                         // Empty line
            ConsoleBuffer[3] = ConsoleBuffer[3].OverwriteAt(Boxing.WindowWall(Boxing.Center($"{(SelectedInfo == 0 ? BACK_GREEN : BACK_BLACK)}  USE Item {RESET}",
                infoBoxInnerWidth), infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex);                                                                                                                      // Info USE Button
            ConsoleBuffer[4] = ConsoleBuffer[4].OverwriteAt(Boxing.WindowWall("", infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex);                                                        // Empty Line
            ConsoleBuffer[5] = ConsoleBuffer[5].OverwriteAt(Boxing.WindowWall(Boxing.Center($"{(SelectedInfo == 1 ? BACK_RED : BACK_BLACK)} DROP Item {RESET}",
                infoBoxInnerWidth), infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex);                                                                                                                      // Info DROP line  
            ConsoleBuffer[6] = ConsoleBuffer[6].OverwriteAt(Boxing.WindowWall("", infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex);                                                        // Empty Line
            ConsoleBuffer[7] = ConsoleBuffer[7].OverwriteAt(Boxing.WindowWall(Boxing.Center($"{(SelectedInfo == 2 ? BACK_YELLOW : BACK_BLACK)} SELL Item {RESET}",
                infoBoxInnerWidth), infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex);                                                                                                                     // Info SELL line  
            ConsoleBuffer[8] = ConsoleBuffer[8].OverwriteAt(Boxing.WindowWall((SelectedInfo == 2) ? Boxing.Center($"{FORE_LIGHT_GREEN} {p.Inventory[SelectedItem].SellValueText} {RESET}", infoBoxInnerWidth) : "", infoBoxInnerWidth, FORE_LIGHT_YELLOW), insertionIndex);                                                       // Empty Line
            ConsoleBuffer[9] = ConsoleBuffer[9].OverwriteAt(Boxing.WindowWall("", infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex); // Empty line

            string[] descriptionArray = Boxing.WrapText(p.Inventory[SelectedItem].Description, infoBoxInnerWidth - 2); // Turn the description into an array of lines that fit the infobox.

            for(int i = 10; i < Console.WindowHeight - 5; i++) // Loop through said array 
            {
                if(i - 10 < descriptionArray.Length)
                {
                    ConsoleBuffer[i] = ConsoleBuffer[i].OverwriteAt(Boxing.WindowWall(' ' + descriptionArray[i - 10], infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex); // Description Line
                    continue;
                }
                ConsoleBuffer[i] = ConsoleBuffer[i].OverwriteAt(Boxing.WindowWall("", infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex); // Empty Line
            }

            ConsoleBuffer[Console.WindowHeight - 5] = ConsoleBuffer[Console.WindowHeight - 5].OverwriteAt(Boxing.WindowWall("", infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex); // Empty line
            ConsoleBuffer[Console.WindowHeight - 4] = ConsoleBuffer[Console.WindowHeight - 4].OverwriteAt(Boxing.WindowWall("", infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex); // Empty line
            ConsoleBuffer[Console.WindowHeight - 3] = ConsoleBuffer[Console.WindowHeight - 3].OverwriteAt(Boxing.WindowFloor(infoBoxInnerWidth, INFO_BOX_OUTLINE_COLOR), insertionIndex); // Info Floor
        }

        if (InfoText != string.Empty)
        {
            int infoTextTotalWidth = Console.WindowWidth - 4 - (InfoMode ? infoBoxTotalWidth : 0);
            int infoTextInnerWidth = infoTextTotalWidth - 4;

            ConsoleBuffer[Console.WindowHeight - 6] =
                ConsoleBuffer[Console.WindowHeight - 6].OverwriteAt(
                    Boxing.WindowCeiling(infoTextInnerWidth, TEXT_BOX_OUTLINE_COLOR), 2);

            string[] infoArray = Boxing.WrapText(InfoText, Math.Max(1, infoTextInnerWidth - 2));
            for (int i = 0; i < 2; i++)
            {
                string line = (i < infoArray.Length) ? infoArray[i] : string.Empty;
                ConsoleBuffer[Console.WindowHeight - 5 + i] = ConsoleBuffer[Console.WindowHeight - 5 + i]
                    .OverwriteAt(Boxing.WindowWall(line, infoTextInnerWidth, TEXT_BOX_OUTLINE_COLOR), 2);
            }

            ConsoleBuffer[Console.WindowHeight - 3] =
                ConsoleBuffer[Console.WindowHeight - 3].OverwriteAt(
                    Boxing.WindowFloor(infoTextInnerWidth, TEXT_BOX_OUTLINE_COLOR), 2);
        }

        // Draw the final buffer
        Console.Clear();
        foreach (string line in ConsoleBuffer)
        {
            Console.WriteLine(line);
        }
    }
    private static void Use()
    {
        Item targetItem = p.Inventory[SelectedItem];


        if(targetItem is HealthingItem healthItem)
        {
            p.HealthUp(healthItem.HealthUpAmmount);
            p.Heal(healthItem.HealAmount);
            InfoText = $"{p.Name} used {p.Inventory[SelectedItem]}!\nTheir {FORE_RED}Max HP{RESET} increased by {FORE_GREEN}{healthItem.HealthUpAmmount}{RESET}.";
            p.Inventory.RemoveAt(SelectedItem);
        }
        else if (targetItem is HealingItem healItem)
        {
            p.Heal(healItem.HealAmount);
            InfoText = $"{p.Name} used {p.Inventory[SelectedItem]}!\nThey recovered {FORE_GREEN}{healItem.HealAmount} {FORE_RED}HP{RESET}.";
            p.Inventory.RemoveAt(SelectedItem);
        }
        else if (targetItem is InfoItem infoItem)
        {
            InfoText = infoItem.Message;
        }
        else if (targetItem is WeaponItem weaponItem)
        {
            p.Inventory.Add(p.EquippedWeapon);
            p.EquippedWeapon = weaponItem;
            p.Inventory.RemoveAt(SelectedItem);

            InfoText = $"{p.Name} is now wielding {weaponItem.Name}!";
        }
        else if (targetItem is ArmorItem armorItem)
        {
            if (p.EquippedArmor != null) p.Inventory.Add(p.EquippedArmor);
            p.EquippedArmor = armorItem;
            p.Inventory.RemoveAt(SelectedItem);

            InfoText = $"{p.Name} is now wearing {armorItem.Name}!";
        }
    }

    private static void Drop()
    {
        Item targetItem = p.Inventory[SelectedItem];
        p.CurrentRoom.Inventory.Add(targetItem);
        p.Inventory.RemoveAt(SelectedItem);
        InfoText = $"{p.Name} dropped {targetItem}!";
    }
    private static void Sell()
    {
        if(p.CurrentRoom.NPCs.Count == 0)
        {
            InfoText = $"There is no one in this {FORE_WHITE}room{RESET} to trade with.";
            return;
        }

        foreach(NPC npc in p.CurrentRoom.NPCs)
        {
            if(npc is not FriendlyNPC)
            {
                continue;
            }

            FriendlyNPC? trader = npc as FriendlyNPC;
            Item targetItem = p.Inventory[SelectedItem];
            trader!.Inventory.Add(targetItem);
            p.Money += targetItem.SellValue;
            p.Inventory.Remove(targetItem);

            InfoText = $"{p.Name} sold {targetItem.Name} to {trader.Name} for {targetItem.SellValueText}";
            return;
        }
        InfoText = $"There is no one present who might buy {p.Inventory[SelectedItem].Name}";
    }
}