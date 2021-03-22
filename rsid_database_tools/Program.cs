using System;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;

namespace rsid_database_tools
{
    class Program
    {
        private static string input { get; set; } // Input File Path (or Directory Path)
        private static string output { get; set; } = ""; // Output Directory Path
        private static bool is_merge { get; set; } = false; // Split/Merge Mode

        static void Main(string[] args)
        {
            // Parse Command-Line Args
            ParseOptions(args);

            // Load DataBase
            var database = new rsid_utility.DataBase();
            var attributes = File.GetAttributes(input);
            if (attributes == FileAttributes.Directory)
            {
                database.LoadDirectory(input);
            }
            else
            {
                database.LoadFile(input);
            }

            // Split/Merge DataBase
            if (is_merge)
            {
                // Merge One File
                database.SaveFile(output);
            }
            else
            {
                // Split Each User Files
                database.SaveEachFiles(output);
            }
        }

        private static void ParseOptions(string[] args)
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg: false);
            app.Name = nameof(rsid_database_tools);
            app.Description = "Split/Merge Tools for DataBase File of RealSense ID Viewer Format.";
            app.HelpOption("-h|--help");

            var input_argument = app.Argument(
                name: "input",
                description: "Input DataBase File or Directory Path",
                multipleValues: false);

            var output_option = app.Option(
                template: "-o|--output",
                description: "Output DataBase Files Directory Path (Default is Current Directory)",
                optionType: CommandOptionType.SingleValue);

            var mode_option = app.Option(
                template: "-m|--merge",
                description: "Merge Mode (Default is Split Mode)",
                optionType: CommandOptionType.NoValue);

            app.OnExecute(() =>
            {
                if (input_argument.Value != null)
                {
                    var value = input_argument.Value.ToString();
                    input= value;
                    Console.WriteLine($"input is {input}");
                }

                if (output_option.HasValue())
                {
                    var value = output_option.Value().ToString();
                    output= value;
                    Console.WriteLine($"output is {output}");
                }

                if (mode_option.HasValue())
                {
                    is_merge = true;
                    Console.WriteLine("mode is " + (is_merge ? "merge mode" : "split mode"));
                }

                bool has_arguments = input_argument.Value != null;
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
