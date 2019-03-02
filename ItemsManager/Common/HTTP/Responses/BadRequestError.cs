using System.Net;

namespace ItemsManager.Common.HTTP.Responses
{
    public class BadRequestError : ApiStatus
    {
        public BadRequestError()
            : base(400, HttpStatusCode.BadRequest.ToString())
        {
        }
        
        public BadRequestError(string message)
            : base(400, HttpStatusCode.BadRequest.ToString(), message)
        {
        }
    }
}
