using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Communicator
{
    public interface IApiCaller
    {
        Task<HttpResponseMessage> Call(Uri uri, HttpMethod method, object body = default);
    }

    public class ApiCaller : IApiCaller
    {
        public Task<HttpResponseMessage> Call(Uri uri, HttpMethod method, object body = default)
        {
            return Task.Factory.StartNew(() =>
            {

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders
                                          .Accept
                                          .Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    HttpRequestMessage Message = new HttpRequestMessage(method, uri);

                    if (body != null)
                    {
                        if (body.GetType() == typeof(string))
                        {
                            Message.Content = new StringContent(body.ToString(),
                                                        Encoding.UTF8,
                                                        "application/json");
                        }
                        else
                        {
                            Message.Content = new StringContent(JsonConvert.SerializeObject(body, Formatting.Indented),
                                                            Encoding.UTF8,
                                                            "application/json");
                        }
                    }

                    var result = client.SendAsync(Message).Result;

                    return result;

                }
            });
        }

    }
}
