using Domain.DTOs;
using Domain.Request;

namespace Application.Interfaces.IRepository
{
    public interface IAuthRepository
    {
        public Task<AuthResponseDTO> Register(RegisterDTO dto);
        public Task<AuthResponseDTO> Login(LoginDTO dto);
        public Task GenerateAndSendResetPin(SendResetPassword sendResetPassword);
        public Task ResetPassword(ResetPasswordDTO dto);
        public Task<bool> ValidPin(ValidPin request);
    }
}
