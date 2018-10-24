using System;

namespace Bhbk.Lib.Core.Primitives.Enums
{
    public enum ActivityType
    {
        Create,
        Read,
        Update,
        Delete
    }

    public enum ContextType
    {
        IntegrationTest,
        UnitTest,
        Live
    }
}
