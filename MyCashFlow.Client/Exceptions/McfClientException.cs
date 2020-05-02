using System;
using System.Collections.Generic;
using System.Text;

namespace MyCashFlow.Client.Exceptions
{
    public class McfClientException : Exception
    {
        public string StatusCode { get; set; }
        public string Content { get; set; }
        public Dictionary<string, IEnumerable<string>> Headers { get; set; }

        public McfClientException(string message, string statusCode, string content, Dictionary<string, IEnumerable<string>> headers) :
            base(message)
        {
            StatusCode = statusCode;
            Content = content;
            Headers = headers;
        }
    }
}
