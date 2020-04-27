using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.GlobalErrorHandler.Utility
{
    public class ApiError
    {
        public string Id { get; set; }
        public int StatusCode { get; set; }
        public string Title { get; set; }
        public string Details { get; set; } = "Contact Support Team";

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
