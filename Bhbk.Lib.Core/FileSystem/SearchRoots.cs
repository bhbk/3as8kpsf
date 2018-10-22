using System.IO;
using System.Reflection;

namespace Bhbk.Lib.Core.FileSystem
{
    public class SearchRoots
    {
        public static FileInfo ByAssemblyContext(string file)
        {
            string result;

            //try directory that the entry assembly lives in...
            result = new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName
                + Path.DirectorySeparatorChar + file;

            if (File.Exists(result))
                return new FileInfo(result);

            //try directory that the calling assembly lives in...
            result = new FileInfo(Assembly.GetCallingAssembly().Location).DirectoryName
                + Path.DirectorySeparatorChar + file;

            if (File.Exists(result))
                return new FileInfo(result);

            //try directory that the executing assembly lives in...
            result = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName
                + Path.DirectorySeparatorChar + file;

            if (File.Exists(result))
                return new FileInfo(result);

            throw new FileNotFoundException();
        }
    }
}
