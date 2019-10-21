using Bhbk.Lib.Common.Primitives.Enums;

namespace Bhbk.Lib.Common.Services
{
    public class ContextService : IContextService
    {
        public InstanceContext InstanceType { get; private set; }

        public ContextService(InstanceContext instanceType)
        {
            InstanceType = instanceType;
        }
    }
}
