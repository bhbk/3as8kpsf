using System;

namespace Bhbk.Lib.Core.Interfaces
{
    public interface IGenericActivity
    {
        Guid Id { get; set; }
        Guid ActorId { get; set; }
        string ActivityType { get; set; }
        string TableName { get; set; }
        string KeyValues { get; set; }
        string OriginalValues { get; set; }
        string CurrentValues { get; set; }
        DateTime Created { get; set; }
    }
}
