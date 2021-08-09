using AuthService.Models.Request;
using AuthService.Models.Response;
using System.Threading.Tasks;

namespace AuthService.Interface.Service
{
    public interface IUserService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest model);
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model);
    }
}
