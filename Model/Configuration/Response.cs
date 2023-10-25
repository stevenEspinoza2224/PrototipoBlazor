using System.Net;

namespace Model.Configuration
{
    public class Response
    {
        public ResponseStatus Status { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode < 300;

        public string? Message { get; set; }

        public string? CurrentException { get; set; }
        public Exception? Exception { get; set; }

        public Dictionary<string, object>? Metadata { get; set; }

        public Response(Exception currentException)
        {
            CurrentException = currentException.ToString();
            this.Exception = currentException;
            Status = ResponseStatus.Failed;
        }

        public Response(string currentExceptionMessage)
        {
            CurrentException = currentExceptionMessage;
            Status = ResponseStatus.Failed;
        }

        public Response(string format, params object[] args)
        {
            CurrentException = string.Format(format, args);
            Status = ResponseStatus.Failed;
        }

        public Response()
        {
            Status = ResponseStatus.Success;
        }
    }
    public class ResponseGeneric<T> : Response
    {
        public T? Response { get; set; }

        public ResponseGeneric(T returnObject, HttpStatusCode httpStatusCode)
        {
            Response = returnObject;
            base.StatusCode = httpStatusCode;
            base.Status = ResponseStatus.Success;
            base.CurrentException = null;
        }

        public ResponseGeneric(Exception currentException, HttpStatusCode httpStatusCode)
            : base(currentException)
        {
            Response = default;
            base.StatusCode = httpStatusCode;
        }

        public ResponseGeneric(string currentExceptionMessage, HttpStatusCode httpStatusCode)
            : base(currentExceptionMessage)
        {
            Response = default;
            base.StatusCode = httpStatusCode;
        }

        public ResponseGeneric(string format, params object[] args)
            : base(format, args)
        {
            Response = default;
        }

        public ResponseGeneric()
        {
        }
    }
    public enum ResponseStatus
    {
        Success,
        Failed
    }
}
