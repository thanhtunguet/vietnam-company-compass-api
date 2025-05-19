using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;

namespace VietnamBusiness.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync(int? skip = null, int? take = null);
        Task<UserDTO> GetUserByIdAsync(long id);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> CreateUserAsync(UserCreateDTO userDto);
        Task<UserDTO> UpdateUserAsync(long id, UserUpdateDTO userDto);
        Task<bool> UpdateUserPasswordAsync(long id, UserPasswordUpdateDTO passwordDto);
        Task<bool> DeleteUserAsync(long id);
        Task<int> CountUsersAsync();
    }
}
