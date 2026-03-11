using System.Runtime.CompilerServices;

namespace MediaLens.Tests.Integration;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifierSettings.DontScrubDateTimes();
        DerivePathInfo(
            (sourceFile, projectDirectory, type, method) => new(
                directory: Path.Combine(projectDirectory, "Snapshots", type.Name),
                typeName: type.Name,
                methodName: method.Name));
    }
}
