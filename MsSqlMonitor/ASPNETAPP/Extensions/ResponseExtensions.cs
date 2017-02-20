using ASPNETAPP.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace ASPNETAPP.Extensions
{
    public static class ResponseExtensions
    {
        public static IHttpActionResult ToHttpResponse<TModel>(this IListModelResponse<TModel> response, HttpRequestMessage request)
        {
            var status = HttpStatusCode.OK;

            if (response.DidError)
            {
                status = HttpStatusCode.InternalServerError;
            }
            else if (response.Model == null)
            {
                status = HttpStatusCode.NoContent;
            }
            JsonSerializerSettings settings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            var result = new HttpResponseMessage(status)
            {
                RequestMessage = request,
                Content = new StringContent(JsonConvert.SerializeObject(response, settings))
            };
            return new JsonHttpActionResult(result);
        }

        public static IHttpActionResult ToHttpResponse<TModel>(this ISingleModelResponse<TModel> response, HttpRequestMessage request)
        {
            var status = HttpStatusCode.OK;

            if (response.DidError)
            {
                status = HttpStatusCode.InternalServerError;
            }
            else if (response.Model == null)
            {
                status = HttpStatusCode.NoContent;
            }
            JsonSerializerSettings settings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            var result = new HttpResponseMessage(status)
            {
                RequestMessage = request,
                Content = new StringContent(JsonConvert.SerializeObject(response, settings))
            };
            return new JsonHttpActionResult(result);
        }
    }
}
