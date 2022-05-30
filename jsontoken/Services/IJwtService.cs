using jsontoken.Models;

namespace jsontoken.Services
{
    public interface IJwtService
    {
       // Task<string> GetTokenAsync(AuthRequest authRequest);
        Task<AuthResponse> GetTokenAsync(AuthRequest authRequest, string ipAddress);
        Task<AuthResponse> GetRefreshTokenAsync(string ipAddress, int userId, string userName);
        Task<bool> IsTokenValid(string accessToken, string ipAddress);
    }
}
