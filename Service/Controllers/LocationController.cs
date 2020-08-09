/// <summary>
/// Developer: ShyamSk
/// </summary>

namespace ShareLocation.Service
{
    using Engaze.Core.DataContract;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    public class LocationController : ControllerBase
    {
        private ILogger<LocationController> logger;
        private ILocationManager locationManager;
        public LocationController(ILogger<LocationController> logger, ILocationManager locationManager)
        {
            this.logger = logger;
            this.locationManager = locationManager;
        }


        [HttpGet("location/{eventId:guid}/{requesterId:guid}")]
        public async Task<IActionResult> Get(Guid requesterId, Guid eventId)
        {
            logger.LogInformation("Getting location");

            //validate 
            string message = await locationManager.ValidateLocationRequest(requesterId, eventId);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            //put try catch only when you want to return custom message or status code, else this will
            //be caught in ExceptionHandling middleware so no need to put try catch here

            return Ok(locationManager.GetLocations(eventId));
        }

        [HttpPost("location/{userId:guid}")]
        public IActionResult Post(Guid userId, Location location)
        {
            logger.LogInformation("Saving location");

            //put try catch only when you want to return custom message or status code, else this will
            //be caught in ExceptionHandling middleware so no need to put try catch here

            locationManager.SetLocation(userId, location);

            return new StatusCodeResult(StatusCodes.Status201Created);
        }
    }
}
