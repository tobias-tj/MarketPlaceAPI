using Domain.DTOs;

namespace Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<UserDetailsDTO> GetDetailsUser(int userId);
    }
}
