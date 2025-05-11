using Application.Interfaces.IRepository;
using Application.Interfaces.IServices;
using Domain.DTOs;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDetailsDTO> GetDetailsUser(int userId)
        {
            return await _repository.GetDetailsUser(userId);
        }
    }
}
