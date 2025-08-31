using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GameCollectionAPI.DTOs;
using GameCollectionAPI.DTOs.Users;
using GameCollectionAPI.Exceptions;
using GameCollectionAPI.Models;
using GameCollectionAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace GameCollectionAPI.Services;

public interface IAuthService
{
    Task<UserReadDto> RegisterUserAsync(AuthDto authDto);
    Task<string> LoginUserAsync(AuthDto authDto);
}

public class AuthService : IAuthService
{
    private IUserRepository _userRepository;
    private IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string> LoginUserAsync(AuthDto authDto)
    {
        var user = await _userRepository.GetByUsernameAsync(authDto.Username);
        if (user == null)
        {
            throw new InvalidDataException("User with this username does not exists.");
        }

        var passwordHasher = new PasswordHasher<User>();
        if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, authDto.Password) != PasswordVerificationResult.Success)
        {
            throw new InvalidDataException("Invalid password.");
        }

        var token = GenerateJwtToken(user);
        return token;
    }

    public async Task<UserReadDto> RegisterUserAsync(AuthDto authDto)
    {
        if (await _userRepository.GetByUsernameAsync(authDto.Username) != null)
        {
            throw new DuplicateUsernameException();
        }

        var createdUser = new UserCreateDto
        {
            Username = authDto.Username,
            Password = authDto.Password
        };

        var user = User.FromCreateDto(createdUser);
        await _userRepository.AddAsync(user);
        return user.ToReadDto();
    }

    private string GenerateJwtToken(User user)
    {
        // Define as informações do usuário (claims) no token
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.Username),
            new (ClaimTypes.Role, user.Role?.Name ?? "User")
        };

        // Signing key
        var jwtConfigurations = _configuration.GetSection("Jwt");
        var tokenSecret = jwtConfigurations["Key"];
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Expiration
        var expires = DateTime.Now.AddHours(1);

        // Create token
        var token = new JwtSecurityToken(
            issuer: jwtConfigurations["Issuer"],
            audience: jwtConfigurations["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
