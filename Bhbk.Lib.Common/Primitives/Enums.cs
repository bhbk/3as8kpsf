using System;

namespace Bhbk.Lib.Common.Primitives.Enums
{
    public enum InstanceContext
    {
        DeployedOrLocal,
        IntegrationTest,
        UnitTest,
    }

    public enum LoggingLevel
    {
        Critical = 0,
        Error = 1,
        Warning = 2,
        Information = 3,
        Debug = 4,
        Trace = 5,
    }
}
