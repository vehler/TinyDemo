using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TinyDemo.Common.Data;
using TinyDemo.Common.Helpers;
using TinyDemo.Common.Models;
using TinyDemo.Models;

namespace TinyDemo.Services
{

    public class AuthenticationService
    {

        private readonly DataContext context;
        private readonly JWTSettings jwtSettings;

        public AuthenticationService(DataContext dataContext, IOptions<JWTSettings> jwtSettingsOptions)
        {
            context = dataContext;
            jwtSettings = jwtSettingsOptions.Value;
        }

        public async Task<AuthResponseViewModel> AuthenticateAsync(AuthRequestViewModel model, string ipAddress)
        {
            var user = await GetAuthUserWithRole(null, model.Email);

            if (user == null)
            {
                throw new UserNotFoundException("User Not Found");
            }

            // Check password
            if (!Hash.Validate(model.Password, user.Salt, user.Password))
            {
                throw new IncorrectCredendentialsException("Incorrect User Credentials");
            }

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(ipAddress);

            // save refresh token
            user.Tokens.Add(refreshToken);

            context.Update(user);
            context.SaveChanges();

            return new AuthResponseViewModel(user, jwtToken, refreshToken.Token);
        }

        public async Task<AuthResponseViewModel> RefreshTokenAsync(string token, string ipAddress)
        {
            User user = await GetAuthUserWithRole(token, null);

            if (user == null)
            {
                throw new UserNotFoundException("User Not Found");
            }

            var refreshToken = user.Tokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                throw new InvalidTokenException("Invalid Token");
            }

            // replace old refresh token with a new one and save
            var newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            user.Tokens.Add(newRefreshToken);

            context.Update(user);
            context.SaveChanges();

            var jwtToken = GenerateJwtToken(user);

            return new AuthResponseViewModel(user, jwtToken, newRefreshToken.Token);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var user = context.Users.SingleOrDefault(u => u.Tokens.Any(t => t.Token == token));

            if (user == null)
            {
                throw new UserNotFoundException("User Not Found");
            }

            var refreshToken = user.Tokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                throw new InvalidTokenException("Invalid Token");
            }

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            context.Update(user);
            context.SaveChanges();

            return true;
        }

        #region helpers

        private async Task<User> GetAuthUserWithRole(string token, string email)
        {
            User user = null;
            if (!string.IsNullOrEmpty(email))
            {
                user = context.Users.SingleOrDefault(x => x.Email == email);
            }
            else
            {
                user = context.Users.SingleOrDefault(u => u.Tokens.Any(t => t.Token == token));
            }
            
            await context.Entry(user).Reference(u => u.Role).LoadAsync();

            return user;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var claims = GetClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.Expiry),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private ClaimsIdentity GetClaims(User user)
        {
            return new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                });
        }

        private AccessToken GenerateRefreshToken(string ipAddress)
        {
            var randomBytes = new byte[64];
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            rngCryptoServiceProvider.GetBytes(randomBytes);

            return new AccessToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        #endregion
    }
}