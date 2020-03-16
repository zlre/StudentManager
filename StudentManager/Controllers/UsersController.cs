namespace StudentManager.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using StudentManager.Repositories;
    using StudentManager.Services;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IAuthenticateService _authenticateService;
        private IUsersRepository _repository;
        
        public UsersController(IAuthenticateService authenticateService, IUsersRepository users)
        {
            _authenticateService = authenticateService;
            _repository = users;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticatedUser>> Authenticate([FromBody]LoginCredentials credentials)
        {
            var user = await _authenticateService.Authenticate(credentials);

            if (user == null)
            {
                return BadRequest(new { message = "Имя пользователя или пароль не корректны" });
            }
                
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticatedUser>> Register([FromBody]LoginCredentials credentials)
        {
            var isAdded = await _repository.AddUserAsync(credentials);

            if (isAdded)
            {
                var user = await _authenticateService.Authenticate(credentials);

                if (user == null)
                {
                    return BadRequest(new { message = "Возникла ошибка при попытке зарегистрировать пользователя" });
                }
                else
                {
                    return Ok(user);
                }
            }
            else
            {
                return BadRequest(new { message = "Пользователь уже зарегистрирован" });
            }
        }        
    }
}