namespace ChatApp.API.Controllers.auth
{
    public class AuthController : ApiController
    {
        // POST http://localhost:5077/api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await Sender.Send(command);

            //var response = result.ToApiResponse();

            //return StatusCode(response.StatusCode, response);
        }
    }
}
