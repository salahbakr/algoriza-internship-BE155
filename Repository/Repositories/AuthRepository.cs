using Core;
using Core.Dtos.AdminDtos;
using Core.Dtos.AuthenticationDtos;
using Core.Models;
using Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseModel<AuthDto>> RegisterAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return ErrorMessage(result);
            }

            return new ResponseModel<AuthDto> { Success = true, Message = "Created account successfully" };
        }

        public async Task<ResponseModel<AuthDto>> UpdateAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return ErrorMessage(result);
            }

            return new ResponseModel<AuthDto> { Success = true, Message = "Updated account successfully" };
        }

        public async Task<ResponseModel<AuthDto>> DeleteUserAsync(ApplicationUser user)
        {
            try
            {
                await _userManager.DeleteAsync(user);
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<AuthDto> { Message = "Something went wrong while deleteing user." };
            }

            return new ResponseModel<AuthDto> { Message = "Deleted user successfully", Success = true };
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersInRole(string role, string? search, int page = 1, int pageSize = 5)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            if (!string.IsNullOrEmpty(search))
            {
                return users.Where(u => $"{u.FirstName} {u.LastName}".Contains(search)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<ResponseModel<AuthDto>> AddUserToRole(ApplicationUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
            {
                return ErrorMessage(result);
            }

            return new ResponseModel<AuthDto> { Success = true, Message = $"Added {user.FirstName} to role {role}" };
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            if (await _userManager.FindByEmailAsync(email) is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> IsUserNameExistAsync(string userName)
        {
            if (await _userManager.FindByNameAsync(userName) is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> GetUsersInRoleCount(string roleName)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.Count;
        }

        internal ResponseModel<AuthDto> ErrorMessage(IdentityResult result)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
            {
                errors += $"{error.Description},";
            }

            return new ResponseModel<AuthDto> { Success = false, Message = errors };
        }
    }
}