   _____  ____________   _______________ __________________ ________  ___________ _______________  ___  _____
  /  _  \ \______ \   \ /   /\_   _____/ \      \__    ___/ \_____  \ \_   _____/ \_   _____/\   \/  / /  _  \
 /  /_\  \ |    |  \   Y   /  |    __)_  /   |   \|    |     /   |   \ |    __)    |    __)_  \     / /  /_\  \
/    |    \|    `   \     /   |        \/    |    \    |    /    |    \|     \     |        \ /     \/    |    \
\____|__  /_______  /\___/   /_______  /\____|__  /____|    \_______  /\___  /    /_______  //___/\  \____|__  /
        \/        \/                 \/         \/                  \/     \/             \/       \_/       \/
                                 TRASH WORLD NEWS PRESENTS: ADVENT OF EXA 2021

This repository contains my solutions for Advent of Code 2021 https://adventofcode.com/2021/ implemented in EXA,
the language used to program the EXA-VM in the fictional universe of EXAPUNKS https://zachtronics.com/exapunks/.

Normally registers in EXA-VM are limited to the range of -9999 to 9999. Thanks to the recent Christmas 1998 AXIOM
leaks, Trash World News has discovered AXIOM hid the ability to expand all registers to the full 32-bit signed
integer range. AXIOM wanted to reserve this power for themselves to give them a unfair competitive edge, but
with the power of EXABR3KR that power now belongs to the people!

╔══════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║         _ \ __| _ \  _ \   __|_ _|__ __| _ \  _ \ \  /    _ \\ \   / __| _ \ \   /_ _| __|\ \      /         ║
║           / _|  __/ (   |\__ \  |    |  (   |   /\  /    (   |\ \ /  _|    /\ \ /   |  _|  \ \ \  /          ║
║        _|_\___|_|  \___/ ____/___|  _| \___/ _|_\ _|    \___/  \_/  ___|_|_\ \_/  ___|___|  \_/\_/           ║
╚══════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

     EXABR3KR │ C# source code for my tiny EXAPUNKS mod loader & mod for expanding the register size.
      PUZZLES │ The VirtualNetwork+ JavaScript definitions for the puzzles
              │ See https://www.zachtronics.com/virtualnetwork/ on AXIOMconnect for details.
    SOLUTIONS │ The solutions as EXA source code for if you just want to read them.
              │ (An EXA syntax reference can be found in Trash World News Issue #1 included with the game.)
SOLUTIONS.EXA │ The solutions as EXODUS code files. (For loading in-game.)
   README.TXT │ The readme, you're currently reading it. For your eyes only.

╔══════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║           _ _|  \ |  __|__ __| \   |    |   _ _|  \ |  __|   __|\ \  /   \   _ ) _ \__ / |  / _ \            ║
║             |  .  |\__ \   |  _ \  |    |     |  .  | (_ |   _|  >  <   _ \  _ \   / _ \ . <    /            ║
║           ___|_|\_|____/  _|_/  _\____|____|___|_|\_|\___|  ___| _/\_\_/  _\___/_|_\___/_|\_\_|_\            ║
╚══════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

If you want to execute the solutions in-game, you will need to use EXABR3KR to mod your game. (EXAPUNKS will
only be modded when launched through EXABR3KR, so you don't have to worry about it messing up your game files.)

The mod targets the current Steam version of the game. It will most likely *not* work with the GOG or Itch.io
version without tweaking some things. It might also only work with the Windows version of the game, I don't
know how Zachtronics is packaging the Linux or macOS versions. Additionally, it's very likely that the mod will
break if Zachtronics ever updates the game again. (It hasn't been updated since November 2020, but if you're
from the future just know this might be a problem for you.)

To build the mod loader+mod get the .NET 6 SDK from https://dotnet.microsoft.com/ and run `dotnet build` in the
`EXABR3KR` directory. (You can also just open and build the solution Visual Studio.)

To install the mod, copy all files from the build output from `bin/Debug/` to the EXAPUNKS game files.
(EXAB3KR.exe and EXAPUNKS.exe should be in the same folder. -- If you aren't sure where EXAPUNKS is
installed, right-click the game in Steam and select Manage > Browse Local Files.)

To give EXAPUNKS access to the puzzles and solutions, you need to copy them into your save directory.
On Windows, you can find your save game directory in `%USERPROFILE%\Documents\My Games\EXAPUNKS\<YourSteamId>\`
Copy the files in `SOLUTIONS.EXA` into this folder. Copy the files in `PUZZLES` into the `custom` subfolder.
(If you want to try solving the puzzles yourself skip the `SOLUTIONS.EXA` folder.)

Launch `EXAB3RKR.exe` to start the game with the mod loader. To view and execute the solutions, you need to
be far enough into the game to launch `AXIOM VIRTUALNETWORK+`. Upon launching, go to the `PERSONAL NETWORKS` tab
and double-click the solution you want to view.
