using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;
using VietnamBusiness.Models;
using VietnamBusiness.Repositories;

namespace VietnamBusiness.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IRepository<User> repository, IMapper mapper, ILogger<UserService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(int? skip = null, int? take = null)
        {
            try
            {
                var users = await _repository.GetAsync(
                    orderBy: q => q.OrderBy(u => u.Email),
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }

        public async Task<UserDTO> GetUserByIdAsync(long id)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {Id} not found", id);
                    return null;
                }

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user with ID {Id}", id);
                throw;
            }
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _repository.GetFirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogWarning("User with Email {Email} not found", email);
                    return null;
                }

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user with Email {Email}", email);
                throw;
            }
        }

        public async Task<UserDTO> CreateUserAsync(UserCreateDTO userDto)
        {
            try
            {
                var existingUser = await _repository.GetFirstOrDefaultAsync(u => u.Email == userDto.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("User with Email {Email} already exists", userDto.Email);
                    throw new InvalidOperationException($"User with email {userDto.Email} already exists");
                }

                var user = _mapper.Map<User>(userDto);
                user.PasswordHash = HashPassword(userDto.Password);
                await _repository.AddAsync(user);

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with Email {Email}", userDto.Email);
                throw;
            }
        }

        public async Task<UserDTO> UpdateUserAsync(long id, UserUpdateDTO userDto)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {Id} not found for update", id);
                    return null;
                }

                _mapper.Map(userDto, user);
                await _repository.UpdateAsync(user);

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> UpdateUserPasswordAsync(long id, UserPasswordUpdateDTO passwordDto)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {Id} not found for password update", id);
                    return false;
                }

                // Verify current password
                if (!VerifyPassword(passwordDto.CurrentPassword, user.PasswordHash))
                {
                    _logger.LogWarning("Incorrect current password for user with ID {Id}", id);
                    throw new InvalidOperationException("Current password is incorrect");
                }

                // Update password
                user.PasswordHash = HashPassword(passwordDto.NewPassword);
                await _repository.UpdateAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating password for user with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {Id} not found for deletion", id);
                    return false;
                }

                // Deactivate user instead of deleting
                user.IsActive = false;
                await _repository.UpdateAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {Id}", id);
                throw;
            }
        }

        public async Task<int> CountUsersAsync()
        {
            try
            {
                return await _repository.CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting users");
                throw;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            return hash == HashPassword(password);
        }
    }
}
