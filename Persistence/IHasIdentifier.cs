using System;

namespace Diner.Persistence
{
    public interface IHasIdentifier
    {
        Guid Id { get; }
    }
}
