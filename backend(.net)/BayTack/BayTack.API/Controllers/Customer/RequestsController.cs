using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Jobs.Commands.AcceptOffer;
using BayTack.Application.Features.Jobs.Commands.CreateRequest;
using BayTack.Application.Features.Jobs.Commands.DeleteRequest;
using BayTack.Application.Features.Jobs.Queries.GetMyRequests;
using BayTack.Application.Features.Jobs.Queries.GetRequestById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Customer
{
    /// <summary>Request body for POST /customer/requests.</summary>
    public sealed record CreateRequestBody(
        string ServiceId,
        string Title,
        string Description,
        string LocationDetails,
        int CityId,
        int? AreaId,
        decimal? Budget,
        DateTime? Deadline,
        string? PreferredPayment);

    // Backs: Front_end/customer/app/post-request, requests/  (was mocked -> bt_c_requests)
    // A "request" is a customer job post (CustomerJob) that providers submit offers (ProviderBid) against.
    [Authorize]
    [Route("customer/requests")]
    public class RequestsController : ApiController
    {
        private readonly ICurrentUserService _currentUser;

        public RequestsController(ICurrentUserService currentUser) => _currentUser = currentUser;

        /// <summary>
        /// GET /customer/requests
        /// -> Request[] { id, title, category, budget, deadline, location, status, offers: Offer[] }
        /// status: open | assigned | closed
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new GetMyRequestsQuery(userId), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>GET /customer/requests/{id} -> Request (with offers from providers)</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new GetRequestByIdQuery(id, userId), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>POST /customer/requests  Body: { title, category, budget, deadline, location } -> Request</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequestBody payload, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var command = new CreateRequestCommand(
                userId,
                payload.ServiceId,
                payload.Title,
                payload.Description,
                payload.LocationDetails,
                payload.CityId,
                payload.AreaId,
                payload.Budget,
                payload.Deadline,
                payload.PreferredPayment);

            var result = await Sender.Send(command, ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>DELETE /customer/requests/{id} -> { success: true }</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new DeleteRequestCommand(id, userId), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// POST /customer/requests/{id}/offers/{offerId}/accept -> Request (status -> assigned, becomes an Order)
        /// </summary>
        [HttpPost("{id}/offers/{offerId}/accept")]
        public async Task<IActionResult> AcceptOffer(string id, string offerId, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new AcceptOfferCommand(id, offerId, userId), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }
    }
}