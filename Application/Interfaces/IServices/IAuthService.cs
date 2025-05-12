using Domain.DTOs;

namespace Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> Register(RegisterDTO registerDTO);
        Task<AuthResponseDTO> Login(LoginDTO loginDTO);
    }
}
