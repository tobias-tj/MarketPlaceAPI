using Application.Interfaces.IRepository;
using Application.Interfaces.IServices;
using Domain.DTOs;
using Domain.Request;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<AuthResponseDTO> Login(LoginDTO loginDTO)
        {
            return await _authRepository.Login(loginDTO);
        }

        public async Task<AuthResponseDTO> Register(RegisterDTO registerDTO)
        {
            return await _authRepository.Register(registerDTO);
        }

        public async Task GenerateAndSendResetPin(SendResetPassword sendResetPassword)
        {
            await _authRepository.GenerateAndSendResetPin(sendResetPassword);
        }

        public async Task ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            await _authRepository.ResetPassword(resetPasswordDTO);
        }
        public async Task<bool> ValidPin(ValidPin request)
        {
            return await _authRepository.ValidPin(request);
        }
    }
}
