using AutoMapper;
using Core;
using Core.Dtos.AuthenticationDtos;
using Core.JWTModels;
using Core.Models;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly JWT _jwt;
        private readonly IMapper _mapper;

        public AuthService(
            IUnitOfWork unitOfWork, 
            IImageService imageService, 
            IOptions<JWT> jwt, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _jwt = jwt.Value;
            _mapper = mapper;
        }

        public async Task<ResponseModel<AuthDto>> RegisterAsync(DoctorRegisterDto registerModel, string role)
        {
            if (await _unitOfWork.AuthRepository.IsEmailExistAsync(registerModel.Email))
            {
                return new ResponseModel<AuthDto> { Success = false, Message = "Email is already registered" };
            }

            if (await _unitOfWork.AuthRepository.IsUserNameExistAsync(registerModel.UserName))
            {
                return new ResponseModel<AuthDto> { Success = false, Message = "Username is already registered" };
            }

            var imgUrl = await _imageService.ValidateImage(registerModel.ImageFile);
            if (imgUrl is null)
            {
                return new ResponseModel<AuthDto> { Success = false, Message = "Image is not valid (must not exceed 2mb and allowed extensions are (.png, .jpg, .webp))" };
            }

            ApplicationUser user = _mapper.Map<ApplicationUser>(registerModel);
            user.Image = imgUrl;

            Specialization specialize = new Specialization();

            if (role == "Doctor")
            {
                specialize = await _unitOfWork.Specializations.GetByIdAsync(registerModel.SpecializeId);

                if (specialize is null)
                {
                    return new ResponseModel<AuthDto> { Success = false, Message = "No specialize match that id: " + registerModel.SpecializeId };
                }

                user.Specialize = specialize;
            }

            var result = await _unitOfWork.AuthRepository.RegisterAsync(user, registerModel.Password);

            if (result.Success) 
            {
                await _unitOfWork.AuthRepository.AddUserToRole(user, role);

                var jwtSecurityToken = await CreateJwtToken(user);

                return new ResponseModel<AuthDto>
                {
                    Message = result.Message,
                    Success = result.Success,
                    Data = new AuthDto
                    {
                        Email = user.Email,
                        ExpiresOn = jwtSecurityToken.ValidTo,
                        Roles = (List<string>)await _unitOfWork.AuthRepository.GetRolesAsync(user),
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        UserName = user.UserName,
                        Image = _imageService.GenerateUrl(user.Image)
                    }
                };
            }
            else
            {
                _imageService.DeleteImage(imgUrl);
                return result;
            }
        }

        public async Task<ResponseModel<AuthDto>> UpdateAsync(EditDto updateModel)
        {
            var user = await _unitOfWork.AuthRepository.GetUserByIdAsync(updateModel.Id);

            if (user is null)
            {
                return new ResponseModel<AuthDto> { Message = $"No doctor match the id: {updateModel.Id}" };
            }

            var oldUserImg = user.Image;

            _mapper.Map(updateModel, user);

            if (updateModel.ImageFile is not null)
            {
                user.Image = await _imageService.ValidateImage(updateModel.ImageFile);
            }

            Specialization specialize = new Specialization();
            var roles = (List<string>)await _unitOfWork.AuthRepository.GetRolesAsync(user);

            if (roles.Contains("Doctor"))
            {
                specialize = await _unitOfWork.Specializations.GetByIdAsync(updateModel.SpecializeId);

                if (specialize is null)
                {
                    return new ResponseModel<AuthDto> { Success = false, Message = "No specialize match that id: " + updateModel.SpecializeId };
                }
                user.Specialize = specialize;
            }

            var result = await _unitOfWork.AuthRepository.UpdateAsync(user);

            if (result.Success)
            {

                var jwtSecurityToken = await CreateJwtToken(user);

                _imageService.DeleteImage(oldUserImg);

                return new ResponseModel<AuthDto>
                {
                    Message = result.Message,
                    Success = result.Success,
                    Data = new AuthDto
                    {
                        Email = user.Email,
                        ExpiresOn = jwtSecurityToken.ValidTo,
                        Roles = (List<string>)await _unitOfWork.AuthRepository.GetRolesAsync(user),
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        UserName = user.UserName,
                        Image = _imageService.GenerateUrl(user.Image)
                    }
                };
            }
            else
            {
                return result;
            }
        }

        public async Task<ResponseModel<AuthDto>> DeleteAsync(string id, string role)
        {
            var user = await _unitOfWork.AuthRepository.GetUserByIdAsync(id);

            if (user is null)
            {
                return new ResponseModel<AuthDto> { Message = "No user matches that id: " + id };
            }

            var roles = await _unitOfWork.AuthRepository.GetRolesAsync(user);

            if (!roles.Contains(role))
            {
                return new ResponseModel<AuthDto> { Message = "This user is not a " + role };
            }

            return await _unitOfWork.AuthRepository.DeleteUserAsync(user);
        }

        public async Task<ResponseModel<AuthDto>> LoginAsync(LoginDto loginModel)
        {
            var user = await _unitOfWork.AuthRepository.GetUserByUsernameAsync(loginModel.Username);
            
            if (user is null) 
            {
                user = await _unitOfWork.AuthRepository.GetUserByEmailAsync(loginModel.Username);
            }

            if (user is null || !await _unitOfWork.AuthRepository.CheckPasswordAsync(user, loginModel.Password))
            {
                return new ResponseModel<AuthDto> { Message = "Username, Email or password is incorrect" };
            }

            var jwtSecurityToken = await CreateJwtToken(user);

            return new ResponseModel<AuthDto>
            {
                Message = "Logged in successfully",
                Success = true,
                Data = new AuthDto
                {
                    Email = user.Email,
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    Roles = (List<string>)await _unitOfWork.AuthRepository.GetRolesAsync(user),
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    UserName = user.UserName,
                    Image = _imageService.GenerateUrl(user.Image)
                }
            };
        }

        public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _unitOfWork.AuthRepository.GetClaimsAsync(user);
            var roles = await _unitOfWork.AuthRepository.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles) roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
