using CommandLine;

namespace iSun.Cmd;

public class CommandLineOptions
{
    [Option('c', "cities", Required = true, HelpText = "Pass in the cities for temperature readings")]
    public required IEnumerable<string> Cities { get; set; }
}