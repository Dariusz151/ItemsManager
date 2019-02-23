using System;

namespace ItemsManager.Common.Types
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}
