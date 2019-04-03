using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Entidades
{
    public class Response<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public string messageError { get; set; }
        public T data { get; set; }
        public int totalRecords { get; set; }
        public Response() { }
    }
}