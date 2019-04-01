using System;

namespace Bhbk.Lib.Core.UnitOfWork
{
    public enum ExecutionType
    {
        Test,
        Normal,
    }

    public enum LoggingType
    {
        Critical = 0,
        Error = 1,
        Warning = 2,
        Information = 3,
        Debug = 4,
        Trace = 5,
    }
}
