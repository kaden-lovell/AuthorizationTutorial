using System.Threading.Tasks;
using BloggerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BloggerApi.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : Controller {
    private readonly UserService _userService;

    public UserController(UserService userService) {
      _userService = userService;
    }

    [HttpGet("currentuser")]
    public async Task<IActionResult> GetCurrentUserAsync() {
      var result = await _userService.GetCurrentUserAsync();

      return Json(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddUserAsync([FromBody] dynamic model) {
      var result = await _userService.AddUserAsync(model);

      return Json(result);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateUserAsync([FromBody] dynamic model) {
      var result = await _userService.UpdateUserAsync(model);

      return Json(result);
    }
  }
}