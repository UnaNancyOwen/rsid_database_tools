using System;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using rsid;

namespace rsid_rename
{
    class Program
    {
        private static string input { get; set; } // Input File Path
        private static string old_user_id { get; set; } = ""; // Old User ID
        private static string new_user_id { get; set; } = ""; // New User ID

        static void Main(string[] args)
        {
            // Parse Command-Line Args
            ParseOptions(args);

            // Load DataBase
            var database = new rsid_utility.DataBase();
            var attributes = File.GetAttributes(input);
            if ((attributes & FileAttributes.Archive) != FileAttributes.Archive)
            {
                throw new Exception($"failed {input} is not archive! database file is archive!");
            }
            database.LoadFile(input);

            // ReName User ID
            var db = database.GetDataBase();
            if (db.Count == 0)
            {
                throw new Exception("failed database is empty!");
            }

            bool is_renamed = false;
            foreach ((Faceprints faceprints, string user_id) in db)
            {
                if (user_id.Replace("\0", "") != old_user_id)
                {
                    continue;
                }

                database.Remove(user_id);
                database.Add(faceprints, new_user_id);
                is_renamed = true;
                break;
            }

            // Save DataBase
            if (!is_renamed)
            {
                return;
            }

            var directory = Path.GetDirectoryName(input);
            var file_name = Path.GetFileName(input);
            database.SaveFile(directory, file_name);
        }

        private static void ParseOptions(string[] args)
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg: false);
            app.Name = nameof(rsid_rename);
            app.Description = "Re-Name User ID Tool for DataBase File of RealSense ID Viewer Format.";
            app.HelpOption("-h|--help");

            var input_argument = app.Argument(
                name: "input",
                description: "Input DataBase File",
                multipleValues: false);

            var old_user_id_argument = app.Argument(
                name: "old_user_id",
                description: "Old User ID",
                multipleValues: false);

            var new_user_id_argument = app.Argument(
                name: "new_user_id",
                description: "New User ID",
                multipleValues: false);

            app.OnExecute(() =>
            {
                if (input_argument.Value != null)
                {
                    input = input_argument.Value.ToString();
                    Console.WriteLine($"input is {input}");
                }

                if (old_user_id_argument.Value != null)
                {
                    old_user_id = old_user_id_argument.Value.ToString();
                    Console.WriteLine($"old_user_id is {old_user_id}");
                }

                if (new_user_id_argument.Value != null)
                {
                    new_user_id = new_user_id_argument.Value.ToString();
                    Console.WriteLine($"new_user_id is {new_user_id}");
                }

                bool has_arguments = (input_argument.Value != null) || 
                                     (old_user_id_argument.Value != null) || 
                                     (new_user_id_argument.Value != null);
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
