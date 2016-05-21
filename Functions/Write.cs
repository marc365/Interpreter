/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

using System.IO;

namespace HiSystems.Interpreter
{
    public class Write : Function
    {
        public override string Name
        {
            get
            {
                return "Write";
            }
        }

        public override string Description
        {
            get { return "Write data to a new file."; }
        }

        public override string Usage
        {
            get { return "Write({filename}, {bytearray})"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIs(arguments, 2))
            {
                var value = base.GetTransformedArgument<Literal>(arguments, argumentIndex: 0);
                
                var data = base.GetTransformedArgument<Literal>(arguments, argumentIndex: 1);

                return Execute(value.ToString(), data);
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }

        public Literal Execute(string value, Literal data)
        {
            if (!File.Exists(Path.Execute(value)))
            {
                if (data is ByteArray)
                {
                    File.WriteAllBytes(Path.Execute(value), (ByteArray)data);
                }
                else
                {
                    File.WriteAllText(Path.Execute(value), (Text)data);
                }

                return new Boolean(true);
            }
            else
            {
                return new Error("File exists");
            }
        }

        public static bool Execute(string value, byte[] data)
        {
            if (!File.Exists(Path.Execute(value)))
            {
                File.WriteAllBytes(Path.Execute(value), data);

                return true;
            }
            else
            {
                return false;
            }
        }


        //public static bool IsFileReady(string sFilename)
        //{
        //    // If the file can be opened for exclusive access it means that the file
        //    // is no longer locked by another process.
        //    try
        //    {
        //        using (FileStream inputStream = File.Open(sFilename, FileMode.Open, FileAccess.Read, FileShare.None))
        //        {
        //            if (inputStream.Length > 0)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        return false;
        //    }
        //}
    }
}