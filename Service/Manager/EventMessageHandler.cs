using Engaze.Core.DataContract;
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
            OccuredEventType eventType = msgJObject.Value<OccuredEventType>("EventType");
            JObject eventoObject = msgJObject.Value<JObject>("Data");
            var eventId = eventoObject.Value<Guid>("EventId");

            switch (eventType)
            {
                case OccuredEventType.EventoDeleted:
                    locationManager.ClearEventAndUserLocations(eventId);
                    break;

                case OccuredEventType.EventoEnded:
                    locationManager.ClearEventAndUserLocations(eventId);
                    break;

                case OccuredEventType.ParticipantLeft:                   
                    locationManager.ClearUserLocations(eventoObject.Value<Guid>("UserId"), eventId);
                    break;

                case OccuredEventType.ParticipantStateUpdated:                   
                    locationManager.ClearUserLocations(eventoObject.Value<Guid>("UserId"), eventId);
                    break;

                default:
                    break;
            }
            return Task.CompletedTask;
        }

    }
}

