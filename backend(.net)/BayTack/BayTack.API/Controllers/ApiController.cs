using Asp.Versioning;
using BayTack.Application.Abstractions.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers
{
	[ApiController]
	//[Route("api/[controller]")]
	[ApiVersion(1.0)]
	[Route("api/v{version:apiVersion}/[controller]")]

	public abstract class ApiController : ControllerBase
	{
		protected ApiController(ISender sender, ICurrentUserService currentUser)
		{
			Sender = sender;
			CurrentUser = currentUser;
		}

		protected ISender Sender { get; }
		protected ICurrentUserService CurrentUser { get; }

		protected string? CurrentUserId => CurrentUser.UserId;
		protected string? CurrentUserName => CurrentUser.Email;
		protected bool IsInRole(string role) => CurrentUser.IsInRole(role);
		protected bool IsUserAuthenticated => CurrentUser.IsAuthenticated;

	}
}
