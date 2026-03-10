using System.Runtime.CompilerServices;

namespace MediaLens.Tests.Integration;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifierSettings.DontScrubDateTimes();
    }
}
