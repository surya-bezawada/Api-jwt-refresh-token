using jsontoken.Context;
using jsontoken.Entities;
using jsontoken.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace jsontoken.Services
{
    public class JwtService : IJwtService
    {
        private readonly ApplicationContext _appcontext;
        private readonly IConfiguration _config;

        public JwtService(ApplicationContext appcontext,IConfiguration config)
        {
            _appcontext = appcontext;
             _config = config;
        }


        //public async Task<string> GetTokenAsync(AuthRequest authRequest)
        //{
        //   // ClaimModel model = new ClaimModel();
        //    var user = _appcontext.users.FirstOrDefault(x => x.UserName.Equals(authRequest.UserName) && x.Password.Equals(authRequest.Password));

        //    if (user == null)
        //        return await Task.FromResult<string>(null);
        //    var jwtkey = _config.GetValue<string>("JwtSettings:Key");
        //    var keyBytes = Encoding.ASCII.GetBytes(jwtkey);
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    //var claims = new List<Claim>();
        //    //{
        //    //    claims.Add(new Claim("userName", user.UserName));
        //    //    claims.Add(new Claim("email", user.Email));


        //    //    claims.Add(new Claim("firstName", user.FirstName));

        //    //    claims.Add(new Claim("lastname", user.LastName));

        //    //    claims.Add(new Claim("mobile", user.Mobile));

        //    //    claims.Add(new Claim("DateofBirth", user.DOB));


        //    //    foreach (var role in user.Roles)
        //    //    {
        //    //        claims.Add(new Claim(ClaimTypes.Role, role.RoleId));
        //    //    }


        //    //}
        //    var descriptor = new SecurityTokenDescriptor()
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //         {
        //            new Claim(ClaimTypes.NameIdentifier,user.UserName),
        //            new Claim(ClaimTypes.Email,user.Email),
        //            new Claim(ClaimTypes.NameIdentifier,user.FirstName),
        //            new Claim(ClaimTypes.NameIdentifier,user.LastName),
        //            new Claim(ClaimTypes.MobilePhone,user.Mobile),
        //            new Claim(ClaimTypes.DateOfBirth,user.DOB),
        //            //new Claim(ClaimTypes.Role,user.Roles.RoleId)




        //         }),

        //        Expires = DateTime.UtcNow.AddSeconds(300),

        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
        //        SecurityAlgorithms.HmacSha256)
        //    };

        //    var token = tokenHandler.CreateToken(descriptor);
        //    return await Task.FromResult(tokenHandler.WriteToken(token));
        //}


       

        public async Task<AuthResponse> GetRefreshTokenAsync(string ipAddress, int userId, string userName)
        {
            var refreshToken = GenerateRefreshToken();
            var accessToken = GenerateToken(userName);
            return await SaveTokenDetails(ipAddress, userId, accessToken, refreshToken);
        }

        public async Task<AuthResponse> GetTokenAsync(AuthRequest authRequest, string ipAddress)
        {
            var user = _appcontext.users.FirstOrDefault(x => x.UserName.Equals(authRequest.UserName)
            && x.Password.Equals(authRequest.Password));
            if (user == null)
                return await Task.FromResult<AuthResponse>(null);
            string tokenString = GenerateToken(user.UserName);
            string refreshToken = GenerateRefreshToken();
            return await SaveTokenDetails(ipAddress, user.UserId, tokenString, refreshToken);

        }

        private async Task<AuthResponse> SaveTokenDetails(string ipAddress, int userId, string tokenString, string refreshToken)
        {
            var userRefreshToken = new UserRefreshToken
            {
                CreatedDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(5),
                IpAddress = ipAddress,
                IsInvalidated = false,
                RefreshToken = refreshToken,
                Token = tokenString,
                UserId = userId
            };
            await _appcontext.UserRefreshTokens.AddAsync(userRefreshToken);
            await _appcontext.SaveChangesAsync();
            return new AuthResponse { Token = tokenString, RefreshToken = refreshToken, IsSuccess = true };
        }

        private string GenerateRefreshToken()
        {
            var byteArray = new byte[64];
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                cryptoProvider.GetBytes(byteArray);

                return Convert.ToBase64String(byteArray);
            }
        }

        private string GenerateToken(string userName)
        {
            var jwtKey = _config.GetValue<string>("JwtSettings:Key");
            var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var user = new User();
            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userName),
                    

                }),
                Expires = DateTime.UtcNow.AddSeconds(300),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
               SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(descriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public async Task<bool> IsTokenValid(string accessToken, string ipAddress)
        {
            var isValid = _appcontext.UserRefreshTokens.FirstOrDefault(x => x.Token == accessToken
            && x.IpAddress == ipAddress) != null;
            return await Task.FromResult(isValid);
        }
    }
}

