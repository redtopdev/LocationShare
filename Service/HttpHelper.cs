using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShareLocation.Service
{
    public class HttpHelper
    {
        public static HttpResponseException CreateHttpResponseException(HttpStatusCode statusCode, string message)
        {
            return new HttpResponseException(new HttpResponseMessage(statusCode) { Content = new StringContent(message) });
        }
    }
}
