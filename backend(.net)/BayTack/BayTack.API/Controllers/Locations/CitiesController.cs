using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Locations.Queries.GetCities;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Locations
{
    // Backs: Front_end/customer/app/post-request (city/area picker) and the provider
    // workshop-address flow. Public reference data - no [Authorize], matches ServicesController.
    [Route("cities")]
    public class CitiesController : ApiController
    {
        /// <summary>GET /cities -> City[] { id, name, areas: [{ id, name }] }</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await Sender.Send(new GetCitiesQuery(), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }
    }
}
