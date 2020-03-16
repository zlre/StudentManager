namespace StudentManager.Services
{
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using StudentManager.Helpers;
    using StudentManager.Repositories;
    using StudentManagerData;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUsersRepository _repository;
        private readonly AppSettings _appSettings;
        private readonly IHashFunction _hash;

        public AuthenticateService(IUsersRepository users, IOptions<AppSettings> appSettings, IHashFunction hash )
        {
            _repository = users;
            _appSettings = appSettings.Value;
            _hash = hash;
        }

        public async Task<AuthenticatedUser> Authenticate(LoginCredentials credentials)
        {
            var userID = await _repository.GetUsersIdAsync(credentials.Login);

            if (userID == null)
            {
                return null;
            }

            byte[] salt = userID.ToByteArray();
            //солью для хэширования пароля является id пользователя
            string hashedPassword = _hash.Hash(salt, credentials.Password);

            var user = await _repository.GetUserAsync(credentials.Login, hashedPassword);

            if (user == null)
            {
                return null;
            }

            return new AuthenticatedUser() { Id = user.Id, Login = user.Login, Token = CreateToken(user) };
        }

        private string CreateToken(User user) 
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString())
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }
}
