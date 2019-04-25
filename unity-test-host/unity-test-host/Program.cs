using System;
using CommandLine;
using System.Diagnostics;

namespace unity_test_host
{
    class Program
    {
        const int TIMEOUT = 15 * 60 * 1000;

        public class Options
        {
            [Option('e', "editor", Required = true, HelpText = "Path to unity editor.")]
            public string Editor { get; set; }
            [Option('p', "project", Required = true, HelpText = "Path to project.")]
            public string ProjectPath { get; set; }
            [Option('o', "player", HelpText = "Path where to build player.")]
            public string PlayerPath { get; set; }
            [Option('c', "platform", HelpText = "Path to platform.")]
            public string Platform { get; set; }
            [Option('f', "filter", HelpText = "Test filter.")]
            public string Filter { get; set; }
            [Option('r', "results", HelpText = "Path to save results.")]
            public string ResultsPath { get; set; }
        }

        static void Main(string[] args)
        {
            var parsed = Parser.Default.ParseArguments<Options>(args);
            parsed.WithParsed(o =>
            {
                Console.WriteLine("Path to Unity: " + o.Editor);
                Console.WriteLine("Path to Project: " + o.ProjectPath);
                Console.WriteLine("Path to Results: " + o.ResultsPath);
                Console.WriteLine("Platform: " + o.Platform);
                Console.WriteLine("Filter: " + o.Filter);
                Console.WriteLine();

                StartEditor(o);
                StartPlayer(o.PlayerPath);
            });
            Environment.Exit(0);
        }

        static void WriteError(string value)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            Console.ForegroundColor = originalColor;
        }

        static void StartEditor(Options o)
        {
            var process = new Process();
            process.StartInfo.FileName = o.Editor;
            process.StartInfo.Arguments = $"-projectPath {o.ProjectPath} -executeMethod RunnerCLI.ExecuteTests -ptPlayerPath {o.PlayerPath} -ptFilter {o.Filter} -ptResultsPath {o.ResultsPath} -ptPlatform {o.Platform}";
            Console.WriteLine("Starting editor with:\n" + process.StartInfo.Arguments);
            process.Start();
            process.WaitForExit(TIMEOUT);
            if (process.ExitCode > 0)
            {
                WriteError($"[Error] Player exited with non zero code: {process.ExitCode}");
                Environment.Exit(process.ExitCode);
            }
        }

        static void StartPlayer(string playerPath)
        {
            Process.Start(playerPath).WaitForExit(TIMEOUT);
        }
    }
}
