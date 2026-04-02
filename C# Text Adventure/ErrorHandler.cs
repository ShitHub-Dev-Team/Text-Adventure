using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace TextAdventure;

public static class ErrorHandler
{
    public static Version GameVersion = new(0, 2, 0);
    private static bool _isStable = true;
    public static void SyntaxError()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Syntax Error or missing parameters!");
        Console.ResetColor();
        Console.WriteLine("To see a list of all available commands, use \"help\"");
    }

    public static void InternalError(Exception ex)
    {
        Console.WriteLine("\x1b[1;38;5;001mInternal Error!" + Color.RESET);
        Console.WriteLine("The game may or may not be playable anymore, you might encounter game-breaking bugs from now on.\n");

        Boxing.WriteLineCentered("\x1b[1;38;5;015m" + "ERROR DETAILS" + Color.RESET);
        Console.WriteLine("GAME VERSION  : " + Color.FORE_WHITE + GameVersion + Color.RESET);
        Console.WriteLine("ERROR TYPE    : " + Color.FORE_WHITE + ex.GetType() + Color.RESET);
        Console.WriteLine("ERROR MESSAGE : " + Color.FORE_WHITE + ex.Message + Color.RESET + '\n');

        Boxing.WriteLineCentered("\x1b[1;38;5;015m" + "STACK TRACE" + Color.RESET);
        Console.WriteLine(ex.StackTrace);

        
        Console.WriteLine("\n\n");
        /*
        Console.WriteLine("To automatically report this error, please press 'Y', or any other key to continue without reporting.\n");

        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Y)
                {
                    Console.WriteLine($"{Color.FORE_WHITE}You have chosen to {Color.FORE_RED}not{Color.FORE_WHITE} report this error.");
                    break;
                }
                Console.WriteLine($"{Color.FORE_WHITE}You have chosen to {Color.FORE_GREEN}report{Color.FORE_WHITE} this error.\n" +
                                  $"Thank you for helping me at improving this game :)\n");

                _ = Task.Run(() => ReportError(ex));
                break;
            }
        }

        _isStable = false;
        */ // DEMO

        Console.WriteLine("Please report this Bug via a GitHub Issue!\nThank you in advance!"); // DEMO
    }

    public static async Task ReportError(Exception ex)
    {
        byte[] errorData = Encoding.UTF8.GetBytes($"{GameVersion}\n{ex.GetType()}\n{ex.Message}\n{ex.StackTrace}");

        using TcpClient client = new TcpClient();
        await client.ConnectAsync("garageman.ip64.de", 33533);

        await using NetworkStream stream = client.GetStream();

        await stream.WriteAsync(errorData, 0, errorData.Length);
        await stream.FlushAsync();

        Debug.WriteLine("Sent data!");
    }
}