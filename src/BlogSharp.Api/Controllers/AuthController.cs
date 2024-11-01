using BlogSharp.Data.Data;
using BlogSharp.Data.Entities;
using BlogSharp.Data.Extensions;
using BlogSharp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogSharp.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(
            ApiDbContext context,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("registrar")]
        [Produces("application/json")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] RegistroUsuarioModel model)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Senha);

            if (result.Succeeded) 
            {
                var usuario = new Usuario(Guid.Parse(user.Id), model.Email.ToLower());
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                await _signInManager.SignInAsync(user, false);

                return Ok(await GerarJwt(model.Email));
            }

            return Problem("Falha ao registrar o usuário!");
        }

        [HttpPost("login")]
        [Produces("application/json")]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioModel model)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Senha, false, true);

            if (result.Succeeded)
            {
                return Ok(await GerarJwt(model.Email));
            }

            return Problem("Email ou senha incorretos!");
        }

        private async Task<UsuarioLoginResponseModel> GerarJwt(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
            };

            foreach (var role in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return ObterRespostaToken(encodedToken, user);
        }

        private UsuarioLoginResponseModel ObterRespostaToken(string encodedToken, IdentityUser usuario)
        {
            return new UsuarioLoginResponseModel
            {
                Authenticated = true,
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalHours,
                UsuarioToken = new UsuarioResponseModel
                {
                    Id = Guid.Parse(usuario.Id),
                    Email = usuario.Email,
                }
            };
        }
    }
}
