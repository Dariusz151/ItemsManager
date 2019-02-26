using System;

namespace ItemsManager.Common.Auth
{
    public interface IJwtHandler
    {
        JsonWebToken Create(Guid userId);
    }
}
