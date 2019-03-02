using System.Net;

namespace ItemsManager.Common.HTTP.Responses
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