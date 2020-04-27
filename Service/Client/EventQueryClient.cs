using Engaze.Core.DataContract;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ShareLocation.Service.Client
{
    public class EventQueryClient : IEventQueryClient
    {
        IConfiguration configuration;
        const string EventQueryBaseUrlKey = "EventQueryBaseUrl";
        public EventQueryClient(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(Guid userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>(EventQueryBaseUrlKey));
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                //HTTP GET
                var response = await client.GetAsync("events/user/" + userId);

                if (response.IsSuccessStatusCode)
                {
                    string stringData = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(stringData))
                    {
                        return null;
                    }

                    return JsonConvert.DeserializeObject<IEnumerable<Event>>(stringData);
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw new Exception($"Call to the service {response.RequestMessage.RequestUri} " +
                    $"failed with the status code : {response.StatusCode} and reason phrase :{ response.ReasonPhrase}");

            }
        }
    }
}
