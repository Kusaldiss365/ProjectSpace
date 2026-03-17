using ProjectSpace.Dtos.Auth;

namespace ProjectSpace.Services.AuthService
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterDto user);

        Task<LoginResponseDto> LoginAsync(LoginDto user);

        Task<AuthResponse> GetCurrentUserAsync(string userId);
    }
}
