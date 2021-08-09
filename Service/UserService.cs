using AuthService.Interface.Service;
using AuthService.Models.Request;
using AuthService.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Service
{
    public class UserService : IUserService
    {

        private readonly UserManager<IdentityUser> _userManager;
       

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest model)
        {
            IdentityUser user = new IdentityUser { Email = model.Email, UserName = model.Email };
            
            var result = await _userManager.CreateAsync(user, model.Password);

            return new RegisterResponse { Email = model.Email };
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model)
        {
            var user =  await _userManager.FindByNameAsync(model.Email).ConfigureAwait(false);
            var cheakPassword = await _userManager.CheckPasswordAsync(user, model.Password);
           
            if (user == null || cheakPassword == false) return null;
            var token = generateJwtToken(user);

            return new AuthenticateResponse { Token =  token };
        }

        private string generateJwtToken(IdentityUser account)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("1111-1111-1111-1111-1111");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
