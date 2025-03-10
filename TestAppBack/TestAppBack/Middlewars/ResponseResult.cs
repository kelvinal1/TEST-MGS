using System.Net;

namespace CurbeCorporativo.Api.Middlewares
{
    public class ResponseResult<T>
    {
        public int status { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public T data { get; set; }
        public ResponseResult(T data)
        {
            this.data = data;
            this.success = true;
            this.status = (int)HttpStatusCode.OK;
        }
        public ResponseResult(T data, int status)
        {
            this.data = data;
            this.success = true;
            this.status = status;
        }
        public ResponseResult(int status, string message)
        {
            this.success = false;
            this.message = message;
            this.status = status;
        }
        public ResponseResult(string message)
        {
            this.success = true;
            this.message = message;
            this.status = (int)HttpStatusCode.OK;
        }
    }
}
