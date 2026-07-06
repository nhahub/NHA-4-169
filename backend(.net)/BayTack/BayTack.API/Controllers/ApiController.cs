using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public abstract class ApiController : ControllerBase
	{
		private ISender? _sender;

		protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
	}
}
