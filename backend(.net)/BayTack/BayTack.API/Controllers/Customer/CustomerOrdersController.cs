using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Orders.Commands.CancelOrder;
using BayTack.Application.Features.Orders.Commands.CreateOrder;
using BayTack.Application.Features.Orders.Queries.GetMyOrders;
using BayTack.Application.Features.Orders.Queries.GetOrderById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Customer
{
    // Backs: Front_end/customer/app/orders, dashboard (was mocked -> bt_c_orders)
    [Authorize]
    [Route("customer/orders")]
    public class CustomerOrdersController : ApiController
    {
        private readonly ICurrentUserService _currentUser;

        public CustomerOrdersController(ICurrentUserService currentUser) => _currentUser = currentUser;

        /// <summary>
        /// GET /customer/orders?status=
        /// -> Order[] { id, serviceId, title, provider, avatar, price, status, progress }
        /// status: active | completed | cancelled
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new GetMyOrdersQuery(userId, status), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>GET /customer/orders/{id} -> Order (full detail)</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new GetOrderByIdQuery(userId, id), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>POST /customer/orders  Body: { serviceId, tier } -> Order (booking a catalog service directly)</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest payload, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new CreateOrderCommand(userId, payload.ServiceId, payload.Tier), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>PATCH /customer/orders/{id}/cancel -> Order (status -> cancelled)</summary>
        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new CancelOrderCommand(userId, id), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }
    }

    /// <summary>Body of POST /customer/orders.</summary>
    public sealed record CreateOrderRequest(string ServiceId, string? Tier);
}