namespace ItemsManager.Common.HTTP.Responses
{
    public class ApiStatus
    {
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string Message { get; set; }

        public ApiStatus(int statusCode, string statusDesc)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDesc;
        }

        public ApiStatus(int statusCode, string statusDescription, string message)
            : this(statusCode, statusDescription)
        {
            this.Message = message;
        }
    }
}
