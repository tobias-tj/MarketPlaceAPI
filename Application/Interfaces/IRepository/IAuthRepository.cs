using Domain.DTOs;

namespace Application.Interfaces.IRepository
{
    public interface IAuthRepository
    {
        public Task<AuthResponseDTO> Register(RegisterDTO dto);
        public Task<AuthResponseDTO> Login(LoginDTO dto);
    }
}
