using System.Threading.Tasks;
using BloggerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BloggerApi.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class LoginController : Controller {
    private readonly LoginService _loginService;

    public LoginController(LoginService loginService) {
      _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] dynamic model) {
      var result = await _loginService.LoginAsync(model);

      return Json(result);
    }

    [HttpGet("logout")]
    public async Task<IActionResult> LogoutAsync() {
      await _loginService.LogoutAsync();

      return NoContent();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshCookieAsync([FromBody] dynamic model) {
      var result = await _loginService.LoginAsync(model);

      return Json(result);
    }
  }
}