using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Abstractions
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        private ISender? _sender;

        protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
