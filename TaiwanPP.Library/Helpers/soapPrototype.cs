using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaiwanPP.Library.Helpers
{
    public abstract class soapPrototype
    {
        protected IEnumerable<XElement> datacontainer;
        protected string envelope;
        protected HttpClient httpClient;
        protected Uri queryuri;
        protected string SOAPAction;
        public bool loaded;
        protected virtual async Task<XDocument> fetchSOAP()
        {            
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, queryuri);
            HttpContent hc = new StringContent(envelope);
            hc.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            hc.Headers.ContentType.CharSet = "utf-8";
            req.Headers.Add("SOAPAction", SOAPAction);
            req.Method = HttpMethod.Post;
            req.Content = hc;
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            HttpResponseMessage response = await httpClient.SendAsync(req);
            var returntext = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return XDocument.Parse(returntext);
            }
            return new XDocument();
        }
        public virtual async Task doWork() { }
    }
}
