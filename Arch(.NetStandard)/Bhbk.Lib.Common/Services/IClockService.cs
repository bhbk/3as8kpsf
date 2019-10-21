using System;

namespace Bhbk.Lib.Common.Services
{
    public interface IClockService
    {
        DateTimeOffset UtcNow { get; set; }
    }
}
