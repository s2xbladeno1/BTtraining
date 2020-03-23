using AppTool.Dtos;
using AppTool.Model;
using AppTool.Services.SessionServices;
using AppTool.Services.UserServices;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppTool.Repository
{
    public class UserService: IUserService
    {
        private string connectionString;
        private readonly ISessionService _sessionService;
        public UserService(ISessionService sessionService)
        {
            connectionString = "Server=S2XNGOCLINH\\SQLEXPRESS;Database=TestUser;Trusted_Connection=True;MultipleActiveResultSets=true";
            _sessionService = sessionService;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }
        
        public IEnumerable<User> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = @"select * from [User]";
                dbConnection.Open();
                return dbConnection.Query<User>(sql);
            }
        }
        public async Task<User> Login(LoginDto dto)
        {
            using (IDbConnection dbConnection = Connection)
            {
                try
                {
                    string sql = @"select * from [User] where UserName = @user and Password = @pass";
                    dbConnection.Open();
                    return  dbConnection.QueryFirstOrDefault<User>(sql, new { user = dto.UserName, pass = dto.Password });
                }
                catch(Exception ex)
                {
                    throw ex;
                }

            }
        }

        public async Task<LoginResultDto> GenerateTokenJwt(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKey010203"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                claims: new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.RoleID)
                },
                expires: DateTime.Now.AddDays(2),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new LoginResultDto
            {
                ID = user.ID,
                UserName = user.UserName,
                RoleID = user.RoleID,
                Token = tokenString
            };
        }

        public async Task<IEnumerable<User>> GetUser(int? id)
        {
            try
            {
                var userId = id.HasValue ? id.GetValueOrDefault() : _sessionService.UserId;
                using (IDbConnection dbConnection = Connection)
                {
                    string sql = "select * from [User] where ID = @Id";
                    dbConnection.Open();
                    return dbConnection.Query<User>(sql, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
