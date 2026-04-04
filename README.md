<h1 align="center">
  <span>C# Text Adventure</span>
</h1>

[![Build](https://img.shields.io/github/actions/workflow/status/Joelbu537/C-Text-Adventure/dotnet-desktop.yml?branch=master&style=for-the-badge&label=build&logo=github)](https://github.com/Joelbu537/C-Text-Adventure/actions/workflows/dotnet-desktop.yml) <- ignore that
[![License](https://img.shields.io/badge/License-GPL_v3-blue?style=for-the-badge&logo=readthedocs&logoColor=white)](https://www.gnu.org/licenses/gpl-3.0)
[![Flavortown](https://img.shields.io/badge/Flavortown-Project-red?style=for-the-badge)](https://flavortown.hackclub.com/projects/9276)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge)](https://dotnet.microsoft.com/)

<h2 align="center">
  <span>Installation</span>
</h2>

### Downloading a precompiled version
If you just want to get the program running, head over to the [releases tab](https://github.com/Joelbu537/C-Text-Adventure/releases) and **download the newest release**.\
Make sure to choose the right operating system!\
Extract the files from the .zip and launch the executable.\
I don't update the released versions with each commit, so building from source might give you access to features you wouldn't get otherwise.
### Building from source code
If you want to make sure that you get what you see, you can simply **clone the repository and build it yourself**.\
Use `dotnet build --configuration Release` to build.
> [!NOTE]
> You need to have **.NET 10** installed to **build** this project.\
> The prebuilt releases already come with it.
<h2 align="center">
  <span>Gameplay</span>
</h2>

Creating a good story for the game is not my top priority, instead, I want to focus on the functionality. So be aware of that.\
Playing the game is pretty simple. Just write what you want to do. Like `go north` or `take note`.\
To get a better understanding of the commands, you can always go to the help page by typing `help`, where all commands, parameters and variations are listed.

<h2 align="center">
  <span>Demo-Information</span>
</h2>
This is a demo, a bunch of gameplay mechanics cramped together. You can "finish" this game in about 1 minute if you know what to do and where to go.
If you play this, you might find yourself not knowing what to do. This is because there isn't anything to do really.
As of now, all you can really do is pick up some of the items lying arround, kill the only enemy that exists... yeah that's it.

<h2 align="center">
  <span>What's next</span>
</h2>
Since I most likeley won't be able to finish this game before Flavortown ends, I instead want to turn it into a demo that presents the different game systems I have created.\
<h3 align="center">
  <span>Planned Features</span>
</h2>
- Randomized Map, difficutly scales with distance. Map is either infinite or has a fixed size, allowing a endless game mode and one where you can win the game.<br>
- Plugin support. This should allow for new weapons, rooms, interactions, NPC's, basically anything should be expandable, and it should even be possible to add new game mechanics.<br>
- Leaving the native console behind and using something else supported on all Desktop Environments like Avalonia, since the behaviour of the default console is pissing me off. No, I want to CLEAR the console, not just scroll to the part without text.<br>
