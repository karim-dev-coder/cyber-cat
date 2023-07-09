namespace CppLauncherService.InternalModels;

internal readonly struct RunCommand
{
    public static RunCommand CreateCompile(string cppFileName, string objectFileName)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            return new RunCommand("wsl", $"g++ -g {cppFileName} -Wall -o {objectFileName} -static-libgcc -static-libstdc++");
        }

        return new RunCommand("g++", $"-g {cppFileName} -Wall -o {objectFileName} -static-libgcc -static-libstdc++");
    }

    public static RunCommand CreateLaunch(string objectFileName, string input)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            return new RunCommand("wsl", $"./{objectFileName}", input);
        }

        return new RunCommand($"./{objectFileName}", string.Empty, input);
    }

    public string Command { get; }
    public string Arguments { get; }
    public string Input { get; }

    private RunCommand(string command, string arguments, string input = null)
    {
        Command = command;
        Arguments = arguments;
        Input = input;
    }
}