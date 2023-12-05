using Core.Dtos.AdminDtos;
using Core.Dtos.AuthenticationDtos;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IAuthRepository
    {
        Task<ResponseModel<AuthDto>> RegisterAsync(ApplicationUser user, string password);
        Task<ResponseModel<AuthDto>> UpdateAsync(ApplicationUser user);
        Task<ResponseModel<AuthDto>> AddUserToRole(ApplicationUser user, string role);
        Task<ResponseModel<AuthDto>> DeleteUserAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<IEnumerable<ApplicationUser>> GetAllUsersInRole(string role, string? search, int page = 1, int pageSize = 5);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IList<Claim>> GetClaimsAsync(ApplicationUser user);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<bool> IsEmailExistAsync(string email);
        Task<bool> IsUserNameExistAsync(string userName);
    }
}
