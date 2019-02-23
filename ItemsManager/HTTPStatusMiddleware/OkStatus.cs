

using ItemsManager.HTTPStatusMiddleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ItemsManager.HTTPStatusMiddleware
{
    public class OkStatus : ApiStatus
    {
        public OkStatus()
            : base(200, HttpStatusCode.OK.ToString())
        {
        }

        public OkStatus(string message)
            : base(200, HttpStatusCode.OK.ToString(), message)
        {
        }
    }
}