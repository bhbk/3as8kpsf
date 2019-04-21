using System;

namespace Bhbk.Lib.Core.Primitives.Enums
{
    public enum InstanceContext
    {
        DeployedOrLocal,
        IntegrationTest,
        UnitTest,
    }

    public enum LoggingLevel
    {
        Critical,
        Error,
        Warning,
        Information,
        Debug,
        Trace,
    }
}
