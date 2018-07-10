using System.IO;
using System.Reflection;

namespace Bhbk.Lib.Helpers.FileSystem
{
    public class Search
    {
        public static FileInfo DefaultPaths(string file)
        {
            string result;

            result = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName
                + Path.DirectorySeparatorChar + file;

            if (File.Exists(result))
                return new FileInfo(result);

            result = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName
                + Path.DirectorySeparatorChar + ".."
                + Path.DirectorySeparatorChar + ".."
                + Path.DirectorySeparatorChar + ".."
                + Path.DirectorySeparatorChar + file;

            if (File.Exists(result))
                return new FileInfo(result);

            result = Directory.GetCurrentDirectory()
                + Path.DirectorySeparatorChar + file;

            if (File.Exists(result))
                return new FileInfo(result);

            result = Directory.GetCurrentDirectory()
                + Path.DirectorySeparatorChar + ".."
                + Path.DirectorySeparatorChar + ".."
                + Path.DirectorySeparatorChar + ".."
                + Path.DirectorySeparatorChar + file;

            if (File.Exists(result))
                return new FileInfo(result);

            throw new FileNotFoundException();
        }
    }
}
