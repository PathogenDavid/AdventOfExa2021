////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                   ***** _______________  ___  _____ ____________________________  ____  __.__________  *****                   //
//               ********* \_   _____/\   \/  / /  _  \\______   \______   \_____  \|    |/ _|\______   \ *********               //
//           *************  |    __)_  \     / /  /_\  \|    |  _/|       _/ _(__  <|      <   |       _/ *************           //
// ***********************  |        \ /     \/    |    \    |   \|    |   \/       \    |  \  |    |   \ *********************** //
//           ************* /_______  //___/\  \____|__  /______  /|____|_  /______  /____|__ \ |____|_  / *************           //
//               *********         \/       \_/       \/       \/        \/       \/        \/        \/  *********               //
//                   *****  TRASH WORLD NEWS PRESENTS: EXABR3KR. UNLOCK THE FULL POTENTIAL OF YOUR EXAS.  *****                   //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using HarmonyLib;
using System;
using System.IO;
using System.Reflection;

Console.WriteLine(@"////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////");
Console.WriteLine(@"//                   ***** _______________  ___  _____ ____________________________  ____  __.__________  *****                   //");
Console.WriteLine(@"//               ********* \_   _____/\   \/  / /  _  \\______   \______   \_____  \|    |/ _|\______   \ *********               //");
Console.WriteLine(@"//           *************  |    __)_  \     / /  /_\  \|    |  _/|       _/ _(__  <|      <   |       _/ *************           //");
Console.WriteLine(@"// ***********************  |        \ /     \/    |    \    |   \|    |   \/       \    |  \  |    |   \ *********************** //");
Console.WriteLine(@"//           ************* /_______  //___/\  \____|__  /______  /|____|_  /______  /____|__ \ |____|_  / *************           //");
Console.WriteLine(@"//               *********         \/       \_/       \/       \/        \/       \/        \/        \/  *********               //");
Console.WriteLine(@"//                   *****  TRASH WORLD NEWS PRESENTS: EXABR3KR. UNLOCK THE FULL POTENTIAL OF YOUR EXAS.  *****                   //");
Console.WriteLine(@"////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////");

Harmony harmony = new("EXABR3KR");
Console.WriteLine("Loading EXAPUNKS...");

AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
{
    AssemblyName name = new(e.Name);
    string path = Path.Combine(Environment.CurrentDirectory, name.Name + ".dll");
    return Assembly.LoadFile(path);
};

Assembly steamworks = Assembly.Load("Steamworks.NET");
Assembly exapunks = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, @"EXAPUNKS.exe"));

//==================================================================================================================================
// Install Harmony patches...
//==================================================================================================================================
Console.WriteLine("Disabling Axiom EXA limiter...");

// Patch away things which enforce the -9999..9999 range
// For all of the following, the clamping is enforced by Utility.#=qaagvzkW3lU6LSv_RBmGMYg== so we just intercept that to keep things simple
// * #=qmOEKJGxOTE0Vubm6pwBCwn9FLTrqDWjhx$1PTEWwdhw=.#=qbavqza7m2Xj_eI8vmowA1g==
//   Looks to be the program interpreter
// * CustomPuzzleCompiler.<>c.#=qnjgqT0ej88lwP7AQX9V_Ul4ehesIz2pbP2c33$kBPiU=
//   Converts a JsValue to an ExaValue
// * CustomPuzzleCompiler.#=qSTbsieCZ4ySivib_F4bQRR2qk4IwapJ2IMnZYddqli4=.#=qax8ZRo8A4HZlcEWDL$GN3tv7WJLmlmzzJdmOkjTSHEs=
//   Seems to be the method for creating links between hosts
// * CustomPuzzleCompiler.#=qSTbsieCZ4ySivib_F4bQRR2qk4IwapJ2IMnZYddqli4=.#=q3CJGGe6AIQotqikGtzHPgf3NynY5itqvVsF8yK8Tiag=
//   Not sure, maybe related to checking arrays of numbers for use in files?
{
    Type exapunksUtility = exapunks.GetType("Utility", throwOnError: true);
    MethodInfo clampMethod = exapunksUtility.GetMethod("#=qaagvzkW3lU6LSv_RBmGMYg==", BindingFlags.Static | BindingFlags.Public);
    harmony.Patch(clampMethod, new HarmonyMethod(typeof(Patches).GetMethod(nameof(Patches.Clamp))));
}

// One thing that's slightly disconcerning is that I see this magic number `-10000` being compared with a the values before they're clamped in the last two CustomPuzzleCompiler methods
// Since it's just the custom puzzle compiler this is maybe fine, but we could actually break something weird if we end up unintentionally triggering that case
// These patches simply help us notice if this happens
{
    Type exapunksCustomPuzzleCompiler = exapunks.GetType("CustomPuzzleCompiler", throwOnError: true);
    Type displayClassProbably = exapunksCustomPuzzleCompiler.GetNestedType("#=qSTbsieCZ4ySivib_F4bQRR2qk4IwapJ2IMnZYddqli4=", BindingFlags.NonPublic);
    MethodInfo createHostLinkMethod = displayClassProbably.GetMethod("#=qax8ZRo8A4HZlcEWDL$GN3tv7WJLmlmzzJdmOkjTSHEs=", BindingFlags.NonPublic | BindingFlags.Instance);
    MethodInfo unknownMethod = displayClassProbably.GetMethod("#=q3CJGGe6AIQotqikGtzHPgf3NynY5itqvVsF8yK8Tiag=", BindingFlags.NonPublic | BindingFlags.Instance);
    harmony.Patch(createHostLinkMethod, new HarmonyMethod(typeof(Patches).GetMethod(nameof(Patches.CreateHostLinkProbably))));
    harmony.Patch(unknownMethod, new HarmonyMethod(typeof(Patches).GetMethod(nameof(Patches.UnknownMethod))));
}

// Compiler.#=qOaM5AAn9HNQKdE5$ZZURDyW7irEmqV70SHsgiaufQq4=
// Emits compiler errors for out of range constants in your programs
{
    Type exapunksCompiler = exapunks.GetType("Compiler", throwOnError: true);
    MethodInfo method = exapunksCompiler.GetMethod("#=qsBlymyrni8DBYRQr1vZVlA==", BindingFlags.Static | BindingFlags.NonPublic);
    harmony.Patch(method, new HarmonyMethod(typeof(Patches).GetMethod(nameof(Patches.ReportCompilerError))));
}

// Disable uploading workshop items because they're gonna be incompatible with vanilla games and it's kinda easy to do by accident
{
    Type customPuzzleScreen = exapunks.GetType("CustomPuzzleScreen", throwOnError: true);
    MethodInfo uploadButtonHandler = customPuzzleScreen.GetMethod("#=qf3$URvv$IZ0b22cAbMZDQ5MKkIwtGyBG50A8_kCHKSw=", BindingFlags.Instance | BindingFlags.NonPublic);
    harmony.Patch(uploadButtonHandler, new HarmonyMethod(typeof(Patches).GetMethod(nameof(Patches.DisableMethod))));

    Type steamUgc = steamworks.GetType("Steamworks.SteamUGC", throwOnError: true);
    void DisableSteamUgcMethod(string methodName)
    {
        MethodInfo method = steamUgc.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
        harmony.Patch(method, new HarmonyMethod(typeof(Patches).GetMethod(nameof(Patches.DisableWorkshopUpload))));
    }

    DisableSteamUgcMethod("CreateItem");
    DisableSteamUgcMethod("StartItemUpdate");
    DisableSteamUgcMethod("SetItemTitle");
    DisableSteamUgcMethod("SetItemContent");
    DisableSteamUgcMethod("SetItemPreview");
    DisableSteamUgcMethod("SubmitItemUpdate");
}

// Disable marking puzzles as solved so the user doesn't accidentally upload a puzzle after disabling mods
{
    Type exapunksSaveData = exapunks.GetType("SaveData", throwOnError: true);
    object o = exapunksSaveData.GetField("#=qaWYTjGIF_$4wy7TFfptgetD8vsdT9NbOcXYtgefL3nw=", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
    MethodInfo markCustomPuzzleVersionAsSolved = exapunksSaveData.GetMethod("#=qJyTIH8fW1JxxbT1ArKtZrz10Cm4GWYIlKsuGDl8j1XI=", BindingFlags.Instance | BindingFlags.Public);
    harmony.Patch(markCustomPuzzleVersionAsSolved, new HarmonyMethod(typeof(Patches).GetMethod(nameof(Patches.DisableMethod))));
}

// Get the show simple message box method
{
    Type sdl = exapunks.GetType("SDL2.SDL");
    Patches.ShowSimpleMessageBox = sdl.GetMethod("SDL_ShowSimpleMessageBox", BindingFlags.Static | BindingFlags.Public) ?? throw new Exception("Could not get SDL_ShowSimpleMessageBox");
}

//==================================================================================================================================
// Start EXAPUNKS
//==================================================================================================================================
Console.WriteLine("Starting EXAPUNKS...");
exapunks.EntryPoint.Invoke(null, new object[] { new string[0] });

//==================================================================================================================================
// Patches
//==================================================================================================================================
static class Patches
{
    public static MethodInfo ShowSimpleMessageBox;

    // static bool Compiler.RemoteCompilerError(Compiler compiler, TextSpan errorSpan, string errorMessage)
    public static bool ReportCompilerError(object __0, (int, int) __1, string __2)
    {
        // Ignore errors about constants being out of range
        // (This is a bit brittle since I don't think it works with localized text, but it's kinda annoying to intercept the actual check.)
        if (__2 is "Number too large." or "Number too small.")
        { return false; }

        return true;
    }

    // static int Utility.Clamp(int x, int min, int max)
    public static bool Clamp(int __0, int __1, int __2, ref int __result)
    {
        if (__1 == -9999 && __2 == 9999)
        {
            __result = __0;
            return false;
        }

        return true;
    }

    // void CustomPuzzleCompiler.ProbablySomeLambdaDisplayClass.CreateHostLinkProbably(int, int, int)
    public static void CreateHostLinkProbably(int __0, int __1, int __2)
    {
        if (__1 == -10000 || __2 == -10000)
        { Console.Error.WriteLine($"Warning: CreateHostLinkProbably({__0}, {__1}, {__2}) uses magic number!"); }
    }

    // int CustomPuzzleCompiler.ProbablySomeLambdaDisplayClass.UnknownMethod(int, int, int, int)
    public static void UnknownMethod(int __0, int __1, int __2, int __3)
    {
        if (__1 == -10000 || __3 == -10000)
        { Console.Error.WriteLine($"Warning: UnknownMethod({__0}, {__1}, {__2}, {__3}) uses magic number!"); }
    }

    // void CustomPuzzleScreen.UploadPuzzle()
    // void SaveData.MarkCustomPuzzleVersionAsSolved(Puzzle puzzle, uint version)
    public static bool DisableMethod()
        => false;

    // Replaces various SteamAPI functions
    public static bool DisableWorkshopUpload()
    {
        if (ShowSimpleMessageBox is not null)
        {
            ShowSimpleMessageBox.Invoke
            (
                null,
                new object[]
                {
                    Activator.CreateInstance(ShowSimpleMessageBox.GetParameters()[0].ParameterType),
                    "Error",
                    "Steam Workshop support is disabled because the game is modded!",
                    IntPtr.Zero
                }
            );
        }
        throw new Exception();
    }
}
