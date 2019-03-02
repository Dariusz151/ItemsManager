using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace ItemsManager.Common.Auth
{
    public static class GuidFromToken
    {
        public static Guid Get(HttpContext context)
        {
            Guid guid = new Guid();
            var identity = context.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                guid = Guid.Parse(identity.Name);
                if (guid != Guid.Empty)
                    return guid;
            }
            
            return Guid.Empty;
        }
    }
}
