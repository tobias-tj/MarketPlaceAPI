using Application.Interfaces.IRepository;
using Application.Interfaces.IServices;
using Domain.DTOs;

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
    }
}
