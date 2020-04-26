using Engaze.Core.MessageBroker.Consumer;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace ShareLocation.Service
{
    public class EventMessageHandler : IMessageHandler
    {
        private ILocationManager locationManager;


        public EventMessageHandler(ILocationManager locationManager)
        {
            this.locationManager = locationManager;
        }
        public void OnError(string error)
        {
            throw new NotImplementedException();
        }

        public async Task OnMessageReceivedAsync(string message)
        {
            try
            {
                await ProcessMessage(JObject.Parse(message));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception ocured :" + ex.ToString());
            }
        }

        private Task ProcessMessage(JObject msgJObject)
        {
            string eventType = msgJObject.Value<string>("EventType");
            JObject eventoObject = msgJObject.Value<JObject>("Data");
            var eventId = eventoObject.Value<Guid>("EventId");          

            switch (eventType)
            {                   
                case "EventoDeleted":
                    locationManager.ClearEventAndUserLocations(eventId);
                    break;

                case "EventoEnded":
                    locationManager.ClearEventAndUserLocations(eventId);
                    break;

                case "ParticipantLeft":
                    var userId = eventoObject.Value<Guid>("UserId");
                    locationManager.ClearUserLocations(userId, eventId);
                    break;             

                default:
                    break;
            }
            return Task.CompletedTask;
        }

    }
}

