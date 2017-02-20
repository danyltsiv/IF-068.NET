using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ASPNETAPP.Responses
{
    public class JsonHttpActionResult : IHttpActionResult
    {
        HttpResponseMessage response;

        public JsonHttpActionResult(HttpResponseMessage response)
        {
            this.response = response;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(response);
        }
    }
}