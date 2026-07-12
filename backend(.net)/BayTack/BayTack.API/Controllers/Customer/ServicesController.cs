using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Services.Queries.GetAllServices;
using BayTack.Application.Features.Services.Queries.GetServiceById;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Customer
{
    // Backs: Front_end/customer/app/browse, service/, saved/  (currently mocked in customer-data.js -> bt_c_services)
    // Public catalog browsing - no [Authorize], matches the original stub.
    [Route("services")]
    public class ServicesController : ApiController
    {
        /// <summary>
        /// GET /services?category=&search=
        /// -> Service[] { id, title, category, icon, provider, providerId, avatar, rating, tiers: { basic, standard, premium } }
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? category, [FromQuery] string? search, CancellationToken ct)
        {
            var result = await Sender.Send(new GetAllServicesQuery(category, search), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>GET /services/{id} -> Service (full detail with pricing tiers)</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken ct)
        {
            var result = await Sender.Send(new GetServiceByIdQuery(id), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }
    }
}