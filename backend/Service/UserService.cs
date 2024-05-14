using backend.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using backend.Exceptions;
using backend.Service.Interface;
using backend.Dtos.UserDto;
using System.Security.Cryptography;

namespace backend.Service
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly SmtpClient _smtpClient;
        private readonly LMSContext _context;

        public UserService(IConfiguration configuration, SmtpClient smtpClient, LMSContext context)
        {
            _config = configuration;
            _smtpClient = new SmtpClient
            {
                Host = _config["SmtpConfig:SmtpServer"],
                Port = int.Parse(_config["SmtpConfig:Port"]),
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                   _config["SmtpConfig:Username"],
                   _config["SmtpConfig:Password"]
               ),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            _context = context;
        }

        public async Task<List<User>> GetListUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users ?? throw new NotFoundException("there are no users at all");
        }
        public async Task<User?> Create(User registerViewModel)
        {
            var existingUserByUsername = await GetByUsername(registerViewModel.Username ?? "");
            if (existingUserByUsername != null)
            {
                throw new BadRequestException("Username already exists");
            }

            var existingUserByEmail = await GetByEmail(registerViewModel.Email ?? "");
            if (existingUserByEmail != null)
            {
                throw new BadRequestException("Email already exists");
            }
            var newUser = new User
            {
                Username = registerViewModel.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(registerViewModel.Password),
                Email = registerViewModel.Email,
                PhoneNumber = registerViewModel.PhoneNumber,
                Avatar = registerViewModel.Avatar,
                RoleId = 2
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<User?> GetByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username));
        }

        public async Task<Tuple<Tokens,User>> Login(UserLoginDto loginViewModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginViewModel.Username);
            
            if (user != null && BCrypt.Net.BCrypt.Verify(loginViewModel.Password, user.Password))
            {
                var roleName = await _context.Roles.Where(r => r.Id == user.RoleId).Select(r => r.RoleName).FirstOrDefaultAsync();
                // Nếu xác thực thành công, tạo JWT token
                //var tokenHandler = new JwtSecurityTokenHandler();
                //var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"] ?? "");
                //var tokenDescriptor = new SecurityTokenDescriptor
                //{
                //    Subject = new ClaimsIdentity(new Claim[]
                //    {
                //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //        new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //        new Claim(JwtRegisteredClaimNames.Aud, _config["JWT:Audience"]),
                //        new Claim(JwtRegisteredClaimNames.Iss, _config["JWT:Issuer"])
                //    }),
                //    Expires = DateTime.UtcNow.AddDays(30),
                //    SigningCredentials = new SigningCredentials
                //        (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                //};
                //var token = tokenHandler.CreateToken(tokenDescriptor);
                //var jwtToken = tokenHandler.WriteToken(token);
                //var rfToken = GenerateRefreshToken();
                //var tokens = new Tokens
                //{
                //    AccessToken = jwtToken,
                //    RefreshToken = GenerateRefreshToken()
                //};
                var jwtToken = GenerateJWTTokens(user.Id.ToString(),user.RoleId.ToString());
                UserRefreshTokens rfToken = new UserRefreshTokens()
                {
                    RefreshToken = jwtToken.RefreshToken,
                    UserName = user.Username,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(24)
                };
                await _context.UserRefreshTokens.AddAsync(rfToken);
                await _context.SaveChangesAsync();
                var userInfo = new User
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    PhoneNumber = user.PhoneNumber,
                };
                return Tuple.Create(jwtToken,userInfo);
            }
            else
            {
                throw new ArgumentException("Wrong email or password");
            }
        }

        public async Task<Tokens> Refresh(Tokens tokens)
        {
            var principal = GetPrincipalFromExpiredToken(tokens.AccessToken);
            var userId = int.Parse(principal.Identity.Name);
            var user = await GetById(userId);
            var roleId = user.RoleId;
            var savedRefreshToken = await _context.UserRefreshTokens.FirstOrDefaultAsync(
                u => u.UserName == user.Username && u.RefreshToken == tokens.RefreshToken && u.IsActived == true
            );
            if (
                savedRefreshToken?.RefreshToken != tokens.RefreshToken
                || savedRefreshToken?.RefreshTokenExpiryTime <= DateTime.UtcNow
            )
            {
               throw new BadRequestException( "Invalid attempt!" );
            }
            var newJwtToken = GenerateJWTTokens(user.Id.ToString(), roleId.ToString());
            if (newJwtToken == null)
            {
                throw new BadRequestException("Invalid attempt!");
            }
            UserRefreshTokens rfToken = new UserRefreshTokens
            {
                RefreshToken = newJwtToken.RefreshToken,
                UserName = user.Username,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(24)
            };
            UserRefreshTokens item = await _context.UserRefreshTokens.FirstOrDefaultAsync(
                u => u.UserName == user.Username && u.RefreshToken == tokens.RefreshToken
            );
            if (item != null)
            {
                _context.UserRefreshTokens.Remove(item);
                await _context.UserRefreshTokens.AddAsync(rfToken);
                await _context.SaveChangesAsync();
            }
            return newJwtToken;
        }

        public async Task Logout(Tokens tokens)
        {
            var rfToken = await _context.UserRefreshTokens.FirstOrDefaultAsync( u => u.RefreshToken == tokens.RefreshToken);
            if (rfToken == null)
            {
                throw new NotFoundException("Invalid refresh token");
            }
            _context.UserRefreshTokens.Remove(rfToken);
            await _context.SaveChangesAsync();
        }

        public async Task AddRequest(ForgotPasswordRequest request)
        {
            await _context.ForgotPasswordRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExpiredRequests()
        {
            var currentTime = DateTime.UtcNow;
            var expiredRequests = await _context.ForgotPasswordRequests
                .Where(f => f.ExpirationTime < currentTime)
                .ToListAsync();
            if (expiredRequests.Count == 0)
            {
                throw new NotFoundException("Code is not found");
            }
            if (expiredRequests.Any())
            {
                _context.ForgotPasswordRequests.RemoveRange(expiredRequests);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveConfirmCode(ForgotPasswordRequest confirmCode)
        {
            if (confirmCode is null)
            {
                throw new NotFoundException("confirm code invalid");
            }
            _context.ForgotPasswordRequests.Remove(confirmCode);
            await _context.SaveChangesAsync();
        }

        public async Task<ForgotPasswordRequest> GetByUserNameAndCode(string username, string code)
        {
            return await _context.ForgotPasswordRequests.FirstOrDefaultAsync(f => f.Username == username && f.Code == code);
        }

        //public async Task<bool> IsUserExists(string username)
        //{
        //    var userExist = await _context.ForgotPasswordRequests.FirstOrDefaultAsync(f => f.Username == username);
        //    if (userExist == null)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        public void SendConfirmationEmail(string email, string confirmationCode)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["SmtpConfig:Username"]),
                Subject = "Xác nhận quên mật khẩu",
                Body = $"Mã xác nhận của bạn là: {confirmationCode}",
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            _smtpClient.Send(mailMessage);
        }

        

        public async Task ChangePassword(ChangePassworkDto user)
        {
            var existUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == user.Username);
            if (existUser != null && BCrypt.Net.BCrypt.Verify(user.OldPassword, existUser.Password))
            {
                existUser.Password = BCrypt.Net.BCrypt.HashPassword(user.NewPassword);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Invalid password or user does not exist.");
            }
        }

        public async Task ResetPassword(ResetPasswordDto user)
        {
            var ValidUser = await _context.Users.FirstOrDefaultAsync(
                u => u.Username == (user.Username)
            );
            if (ValidUser == null)
            {
                throw new NotFoundException("user not found");
            }
            ValidUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Entry(ValidUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        private Tokens GenerateJWTTokens(string userId, string roleId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, userId),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Aud, _config["JWT:Audience"]),
                            new Claim(JwtRegisteredClaimNames.Iss, _config["JWT:Issuer"]),
                            new Claim(ClaimTypes.Role, roleId.ToString())
                        }
                    ),
                    Expires = DateTime.Now.AddHours(3),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(secretKey),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var refreshToken = GenerateRefreshToken();
                return new Tokens
                {
                    AccessToken = tokenHandler.WriteToken(token),
                    RefreshToken = refreshToken
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var secretKey = Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]);
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken securityToken
            );

            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

            if (
                jwtSecurityToken == null
                || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase
                )
            )
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
        public string GenerateRandomCode()
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
