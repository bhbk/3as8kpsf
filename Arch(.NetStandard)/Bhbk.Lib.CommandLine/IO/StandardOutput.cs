using Bhbk.Lib.CommandLine.Primitives;
using System;

namespace Bhbk.Lib.CommandLine.IO
{
    public class StandardOutput
    {
        public static int FondFarewell()
        {
            Console.WriteLine();
            Console.WriteLine();

            return (int)ExitCodes.Success;
        }

        public static int AngryFarewell(Exception ex)
        {
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine(ex.StackTrace);
            Console.ResetColor();

            Console.WriteLine();

            return (int)ExitCodes.Exception;
        }
    }
}
