/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

using System.IO;
using System.IO.Compression;

namespace HiSystems.Interpreter
{
    public class Gzip : Function
    {
        private static int bufferSize = 4096;

        public override string Name
        {
            get
            {
                return "Gzip";
            }
        }

        public override string Description
        {
            get { return "Returns the compressed (true) or decompressed (false) data."; }
        }

        public override string Usage
        {
            get { return "Gzip(Array(Byte),true|false)"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIs(arguments, 2))
            {
                var data = (ByteArray)base.GetArgument(arguments, argumentIndex: 0).Transform();
                var doCompress = (Boolean)base.GetArgument(arguments, argumentIndex: 1).Transform();

                if (doCompress)
                {
                    return new ByteArray(Compress(data));
                }
                else
                {
                    return new ByteArray(Decompress(data));
                }
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }

        public static byte[] Compress(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static byte[] Decompress(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }

                return mso.ToArray();
            }
        }

        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[bufferSize];
            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

    }
}