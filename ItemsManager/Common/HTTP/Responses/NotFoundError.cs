using System.Net;

namespace ItemsManager.Common.HTTP.Responses
{
    public class NotFoundError : ApiStatus
    {
        public NotFoundError()
            : base(404, HttpStatusCode.NotFound.ToString())
        {
        }
        
        public NotFoundError(string message)
            : base(404, HttpStatusCode.NotFound.ToString(), message)
        {
        }
    }
}
