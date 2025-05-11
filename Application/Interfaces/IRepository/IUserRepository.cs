using Domain.DTOs;

namespace Application.Interfaces.IRepository
{
    public interface IUserRepository
    {
        Task<UserDetailsDTO> GetDetailsUser(int userId);
    }
}
