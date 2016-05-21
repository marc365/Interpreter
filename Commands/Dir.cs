/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace HiSystems.Interpreter
{
    public class Dir : Command
    {
        public override string Name
        {
            get
            {
                return "Dir";
            }
        }

        public override string Description
        {
            get { return "Display the files in the root folder."; }
        }

        public override string Usage
        {
            get { return "{wildcard}"; }
        }

        public override Literal Execute(string[] args)
        {
            string output = string.Empty;
            string pattern = string.Empty;

            if (args.Length > 0)
            {
                pattern = args[0];
            }

            foreach (FileModel file in List(Path.Execute("~/"), pattern))
            {
                output = string.Format("{0}{1} {2}{3}", output, file.Name, file.Size, Environment.NewLine);
            }

            return new Text(output);
        }

        public static List<FileModel> List(string path, string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                pattern = "*.*";

            List<FileModel> files = new List<FileModel>();

            if (Directory.Exists(path))
            {
                foreach (string directoryName in Directory.EnumerateDirectories(path, "*"))
                {
                    foreach (string fileName in Directory.EnumerateFiles(System.IO.Path.Combine(path, directoryName), pattern))
                    {
                        FileModel file = new FileModel();
                        file.Location = fileName;
                        FileInfo fi = new FileInfo(fileName);

                        file.Size = fi.Length.ToString();

                        string[] locations = file.Location.Split('\\');
                        file.Name = locations[locations.Length - 1];

                        files.Add(file);
                    }

                    foreach (string innerdirectoryName in Directory.EnumerateDirectories(directoryName, "*"))
                    {
                        files.AddRange(List(innerdirectoryName, pattern));
                    }
                }

                foreach (string fileName in Directory.EnumerateFiles(path, pattern))
                {
                    FileModel file = new FileModel();
                    file.Location = fileName;
                    FileInfo fi = new FileInfo(fileName);

                    file.Size = fi.Length.ToString();
                    string[] locations = file.Location.Split('\\');
                    file.Name = locations[locations.Length - 1];


                    files.Add(file);
                }
            }

            return files;
        }
    }

    public class FileModel
    {
        public virtual string Name { get; set; }
        public virtual string Location { get; set; }
        public virtual string Size { get; set; }
    }
}