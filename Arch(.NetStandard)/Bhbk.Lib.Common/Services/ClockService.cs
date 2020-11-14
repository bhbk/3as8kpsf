using Bhbk.Lib.Common.Primitives.Enums;
using System;

namespace Bhbk.Lib.Common.Services
{
    public class ClockService : IClockService
    {
        private readonly InstanceContext _instance;
        private DateTimeOffset _moment;
        private TimeSpan _offset;
        private bool _future;

        public ClockService()
            : this(new ContextService(InstanceContext.DeployedOrLocal)) { }

        public ClockService(IContextService context)
        {
            _instance = context.InstanceType;
            _moment = DateTime.UtcNow;
        }

        public DateTimeOffset UtcNow
        {
            get
            {
                if (_instance == InstanceContext.DeployedOrLocal
                    || _instance == InstanceContext.End2EndTest)
                    return DateTime.UtcNow;

                else
                {
                    if (_future)
                        return DateTime.UtcNow.Add(_offset);
                    else
                        return DateTime.UtcNow.Subtract(_offset);
                }
            }
            set
            {
                if (_instance == InstanceContext.DeployedOrLocal
                    || _instance == InstanceContext.End2EndTest)
                    throw new NotSupportedException();

                else
                {
                    _moment = value;

                    if (_moment > DateTime.UtcNow)
                    {
                        _future = true;
                        _offset = _moment.Subtract(DateTime.UtcNow);
                    }
                    else
                    {
                        _future = false;
                        _offset = DateTime.UtcNow.Subtract(_moment.UtcDateTime);
                    }
                }
            }
        }
    }
}
