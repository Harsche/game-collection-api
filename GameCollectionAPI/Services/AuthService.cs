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

    /// <summary>
    /// Logs in a user by validating credentials and generating a JWT token.
    /// </summary>
    /// <param name="authDto">The authentication data.</param>
    /// <returns>A task containing the JWT token string.</returns>
    public async Task<string> LoginUserAsync(AuthDto authDto)
    {
        var user = await _userRepository.GetByUsernameAsync(authDto.Username);
        if (user == null)
        {
            throw new InvalidDataException("User with this username does not exist.");
        }

        var passwordHasher = new PasswordHasher<User>();
        if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, authDto.Password) != PasswordVerificationResult.Success)
        {
            throw new InvalidDataException("Invalid password.");
        }

        var token = GenerateJwtToken(user);
        return token;
    }

    /// <summary>
    /// Registers a new user and returns the created user's read DTO.
    /// </summary>
    /// <param name="authDto">The authentication data.</param>
    /// <returns>A task containing the created user's read DTO.</returns>
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

    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user entity.</param>
    /// <returns>A JWT token as a string.</returns>
    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.Username),
            new (ClaimTypes.Role, user.Role?.Name ?? "User")
        };

        // Signing key
        var jwtConfigurations = _configuration.GetSection("Jwt");
        var tokenSecret = jwtConfigurations["Key"];
        if (string.IsNullOrWhiteSpace(tokenSecret))
        {
            // TODO - Log warning!
            throw new InvalidOperationException("JWT secret key is missing in the app configuration.");
        }
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
