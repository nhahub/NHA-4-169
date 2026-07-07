using BayTack.Application.Abstractions.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public abstract class ApiController : ControllerBase
	{
		private ISender? _sender;
		private ICurrentUserService? _currentUser;



		protected ISender Sender => 
			_sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

		protected ICurrentUserService CurrentUser =>
			_currentUser ??= HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

		protected string? CurrentUserId => CurrentUser.UserId;
		protected string? CurrentUserName => CurrentUser.Email;
		protected bool IsInRole(string role) => CurrentUser.IsInRole(role);
		protected bool IsUserAuthenticated => CurrentUser.IsAuthenticated;

	}
}
