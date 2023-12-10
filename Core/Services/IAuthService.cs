using Core.Dtos.AuthenticationDtos;
using Core.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IAuthService
    {
        Task<ResponseModel<AuthDto>> RegisterAsync<TRegisterDto>(TRegisterDto registerModel, string role) where TRegisterDto : BaseRegisterDto;
        Task<ResponseModel<AuthDto>> UpdateAsync(EditDto updateModel);
        Task<ResponseModel<AuthDto>> DeleteAsync(string id, string role);
        Task<ResponseModel<AuthDto>> LoginAsync(LoginDto loginModel);
        Task<ResponseModel<AuthDto>> CreateAndLoginAsAdmin();
        Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user);
    }
}
