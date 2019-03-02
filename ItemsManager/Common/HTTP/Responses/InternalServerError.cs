using System.Net;

namespace ItemsManager.Common.HTTP.Responses
{
    public class InternalServerError : ApiStatus
    {
        public InternalServerError()
            : base(500, HttpStatusCode.InternalServerError.ToString())
        {
        }
        
        public InternalServerError(string message)
            : base(500, HttpStatusCode.InternalServerError.ToString(), message)
        {
        }
    }
}
