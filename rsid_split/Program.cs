using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;

namespace rsid_split
{
    class Program
    {
        private static List<string> inputs { get; set; } // Input File Paths or Directory Paths
        private static string output { get; set; } = ""; // Output Directory Path

        static void Main(string[] args)
        {
            // Parse Command-Line Args
            ParseOptions(args);

            // Load DataBase
            var database = new rsid_utility.DataBase();
            foreach (var input in inputs)
            {
                var attributes = File.GetAttributes(input);
                if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    database.LoadDirectory(input);
                }
                if ((attributes & FileAttributes.Archive) == FileAttributes.Archive)
                {
                    database.LoadFile(input);
                }
            }

            // Split Each DataBases To Each Files
            database.SaveEachFiles(output);
        }

        private static void ParseOptions(string[] args)
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg: false);
            app.Name = nameof(rsid_split);
            app.Description = "Merge Multi DataBase Files Tool for DataBase File of RealSense ID Viewer Format.";
            app.HelpOption("-h|--help");

            var inputs_argument = app.Argument(
                name: "inputs",
                description: "Input DataBase Files or Directory Paths",
                multipleValues: true);

            var output_option = app.Option(
                template: "-o|--output",
                description: "Output DataBase Files Directory Path (Default is Current Directory)",
                optionType: CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                if (inputs_argument.Values != null)
                {
                    inputs = inputs_argument.Values;
                    foreach (var input in inputs)
                    {
                        Console.WriteLine($"input is {input}");
                    }
                }

                if (output_option.HasValue())
                {
                    output = output_option.Value().ToString();
                    Console.WriteLine($"output is {output}");
                }

                bool has_arguments = inputs_argument.Values != null;
                if (!has_arguments)
                {
                    app.ShowHelp();
                    throw new ArgumentException("failed required arguments not specified!");
                }

                return 0;
            });

            app.Execute(args);
        }
    }
}
