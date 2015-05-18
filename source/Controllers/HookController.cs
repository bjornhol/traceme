using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TraceMe.Hubs;

namespace TraceMe.Controllers
{
    public class HookController : ApiController
    {
        [HttpGet]
        [HttpPost]
        public async Task<IHttpActionResult> Tell()
        {
            string payload = "not GET or POST...";
            string challenge = string.Empty;

            if (Request.Method == HttpMethod.Post)
            {
                payload = "POST " + await Request.Content.ReadAsStringAsync();
            }
            else if(Request.Method == HttpMethod.Get)
            {
                payload = "GET " + Request.RequestUri.Query;
                challenge = Request.RequestUri.ParseQueryString()["hub.challenge"];
            }

            TraceHub.Trace(Request.RequestUri.AbsolutePath, User, payload);

            if (!string.IsNullOrEmpty(challenge))
            {
                return Content(HttpStatusCode.Accepted, challenge, new TextMediaTypeFormatter());
            }

            return Ok();
        }
    }

    class TextMediaTypeFormatter : MediaTypeFormatter
    {
        public TextMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            TextWriter writer = new StreamWriter(writeStream);
            writer.Write(value.ToString());

            return writer.FlushAsync();
        }

        public override bool CanReadType(Type type)
        {
            return typeof(string) == type;
        }

        public override bool CanWriteType(Type type)
        {
            return typeof(string) == type;
        }
    }
}
