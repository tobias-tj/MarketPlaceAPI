using Domain.DTOs;
using Domain.Request;

namespace Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> Register(RegisterDTO registerDTO);
        Task<AuthResponseDTO> Login(LoginDTO loginDTO);
        Task GenerateAndSendResetPin(SendResetPassword sendResetPassword);
        Task ResetPassword(ResetPasswordDTO resetPasswordDTO);
        Task<bool> ValidPin(ValidPin request);
    }
}
