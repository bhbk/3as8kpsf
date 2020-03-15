using Bhbk.Lib.CommandLine.Primitives;
using System;

namespace Bhbk.Lib.CommandLine.IO
{
    public class StandardOutput
    {
        public static int FondFarewell()
        {
            Console.WriteLine();
            Console.WriteLine("Press key to exit...");
            Console.WriteLine();

            Console.ReadKey();
            return (int)ExitCodes.Success;
        }

        public static int AngryFarewell(Exception ex)
        {
            Console.WriteLine();
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine(ex.StackTrace);
            Console.WriteLine();
            Console.WriteLine("Press key to exit...");
            Console.WriteLine();

            Console.ReadKey();
            return (int)ExitCodes.Exception;
        }
    }
}
