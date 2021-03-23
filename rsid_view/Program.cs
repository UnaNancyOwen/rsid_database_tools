using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;
using rsid;
using System.Runtime.Serialization.Formatters.Binary;

namespace rsid_view
{
    class Program
    {
        private static List<string> inputs { get; set; } // Input File Paths or Directory Paths

        static void Main(string[] args)
        {
            // Parse Command-Line Args
            ParseOptions(args);

            // View User IDs
            foreach (var input in inputs)
            {
                var attributes = File.GetAttributes(input);
                if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    var files = Directory.GetFiles(input, "*");
                    foreach (var file in files)
                    {
                        ShowUserIDs(file);
                    }
                }
                else if ((attributes & FileAttributes.Archive) == FileAttributes.Archive)
                {
                    var file = input;
                    ShowUserIDs(file);
                }
                else{
                    continue;
                }
            }
        }

        private static void ShowUserIDs(string file)
        {
            try
            {
                var database = new rsid_utility.DataBase();
                database.LoadFile(file);
                var db = database.GetDataBase();
                Console.WriteLine($"* {file.Replace(@"\", "/")} ({db.Count} users)");
                foreach ((Faceprints faceprints, string user_id) in db)
                {
                    Console.WriteLine($"  - {user_id}");
                }
            }
            catch
            {
            }
        }

        private static void ParseOptions(string[] args)
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg: false);
            app.Name = nameof(rsid_view);
            app.Description = "View User IDs contain in DataBase Files for DataBase File of RealSense ID Viewer Format.";
            app.HelpOption("-h|--help");

            var inputs_argument = app.Argument(
                name: "inputs",
                description: "Input DataBase Files or Directory Paths",
                multipleValues: true);

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
