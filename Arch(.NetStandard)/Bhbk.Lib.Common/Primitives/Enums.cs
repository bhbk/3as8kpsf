using System;

namespace Bhbk.Lib.Common.Primitives.Enums
{
    public enum InstanceContext
    {
        DeployedOrLocal = 4,
        End2EndTest = 3,
        SystemTest = 2,
        IntegrationTest = 1,
        UnitTest = 0,
    }

    public enum LoggingLevel
    {
        Trace = 5,
        Debug = 4,
        Information = 3,
        Warning = 2,
        Error = 1,
        Critical = 0,
    }
}
