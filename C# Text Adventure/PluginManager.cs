using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TextAdventure.PluginContract;

namespace TextAdventure;
public static class PluginManager
{
    public static List<IPlugin> Plugins { get; } = new();
    private static List<string> pluginDLLs = new();
    private static string pluginDir = Path.Combine(AppContext.BaseDirectory, "Plugins");
    private static string pluginConfigPath = Path.Combine(pluginDir, "plugins.cfg");
    public static void LoadPlugins()
    {
        Directory.CreateDirectory(pluginDir);

        List<PluginConfig>? pluginConfig = new();
        if(File.Exists(pluginConfigPath)) pluginConfig = JsonSerializer.Deserialize<List<PluginConfig>?>(File.ReadAllText(pluginConfigPath));

        HostContext ctx = new();

        foreach (string dll in Directory.EnumerateFiles(pluginDir, "*.dll"))
        {
            try
            {
                var asm = Assembly.LoadFrom(dll);

                foreach (var t in asm.GetTypes())
                {
                    if (t.IsAbstract || !typeof(IPlugin).IsAssignableFrom(t)) continue;

                    if (Activator.CreateInstance(t) is IPlugin plugin)
                    {
                        // Check double loading
                        if (Plugins.Any(other =>
                            {
                                if (other.Name.ToLower() == plugin.Name.ToLower())
                                {
                                    Console.WriteLine(
                                        $"{Color.FORE_RED}Failed to load{Color.RESET} plugin {Color.FORE_WHITE}{plugin.Name}{Color.RESET} ({Path.GetFileName(dll)}) because another plugin ({Path.GetFileName(pluginDLLs[Plugins.IndexOf(other)])}) with the same name is already loaded.");
                                    return true;
                                }
                                return false;
                            })) continue;

                        string pluginHash = Md5File(dll);
                        if (pluginConfig.Any(x => x.Hash == pluginHash))
                        {
                            plugin.Enabled = pluginConfig.First(x => x.Hash == pluginHash).Enabled;
                        }
                        else
                        {
                            plugin.Enabled = false;
                            pluginConfig.Add(new PluginConfig { Hash = pluginHash, DllName = Path.GetFileName(dll), Enabled = false });
                        }
                        
                        Plugins.Add(plugin);
                        pluginDLLs.Add(dll);
                        Console.WriteLine($"Loaded plugin {Color.FORE_WHITE}{plugin.Name}{Color.RESET} ({Path.GetFileName(dll)}) {plugin.Version}");
                        plugin.Initialize(ctx);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Color.FORE_RED}Failed to load{Color.RESET} plugin {Path.GetFileName(dll)}\n{ex.Message}\n{ex.StackTrace}");
            }
        }

        File.WriteAllText(pluginConfigPath, JsonSerializer.Serialize(pluginConfig));
    }
    public static void PluginManagerDisplay()
    {
        int selectedIndex = 0;
        while (true)
        {
            Console.Clear();
            // Draw
            Console.WriteLine("Plugins are a WIP feature that DO NOT WORK in this demo."); // DEMO
            for (int i = 0; i < Plugins.Count; i++)
            {
                if(i == selectedIndex)
                {
                    Console.Write(Color.BACK_WHITE + Color.FORE_BLACK);
                }

                IPlugin p = Plugins[i];
                Console.WriteLine($"[{(p.Enabled ? 'X' : ' ')}] {Plugins[i].Name} {p.Version}");

                Console.Write(Color.RESET);
            }
            // Read
            while (true)
            {
                if(Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Console.Clear();

                        List<PluginConfig>? pluginConfig = JsonSerializer.Deserialize<List<PluginConfig>?>(File.ReadAllText(pluginConfigPath)) ?? new();

                        for (int i = 0; i < Plugins.Count; i++)
                        {
                            string hash = Md5File(pluginDLLs[i]);
                            int index = pluginConfig.FindIndex(x => x.Hash == hash);
                            var cfg = pluginConfig[index];
                            cfg.Enabled = Plugins[i].Enabled;
                            pluginConfig[index] = cfg;
                        }

                        File.WriteAllBytes(pluginConfigPath, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(pluginConfig)));
                        return;
                    }
                    if (key.Key == ConsoleKey.Enter)
                    {
                        Plugins[selectedIndex].Enabled = !Plugins[selectedIndex].Enabled;
                    }
                    else if (key.Key is ConsoleKey.S or ConsoleKey.DownArrow)
                    {
                        selectedIndex = Math.Clamp(selectedIndex + 1, 0, Plugins.Count - 1);
                    }
                    else if (key.Key is ConsoleKey.W or ConsoleKey.UpArrow)
                    {
                        selectedIndex = Math.Clamp(selectedIndex - 1, 0, Plugins.Count - 1);
                    }

                    break;
                }
            }
        }
    }

    private static string Md5File(string filePath)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filePath);

        return Convert.ToHexString(md5.ComputeHash(stream));
    }
}

public struct PluginConfig
{
    public string Hash { get; set; }
    public string DllName { get; set; }
    public bool Enabled { get; set; }
}