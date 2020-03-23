using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AppTool.Dtos;
using AppTool.Model;
using AppTool.Repository;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AppTool.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [Route("api/user/get")]
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _userService.GetAll();
        }

        //[Authorize]
        [Route("api/user/get/{id}")]
        [HttpGet]
        public async Task<IEnumerable<User>> Get(int id)
        {
            return await _userService.GetUser(id);
        }

        [Route("api/user/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.Login(dto);
            if (user == null)
            {
                return BadRequest(new { message = "Your login was incorrect" });
            }
            else
            {
                var token = await _userService.GenerateTokenJwt(user);
                var userSession = new UserSessionDto();
                userSession.ID = user.ID;
                userSession.UserName = user.UserName;
                userSession.Password = user.Password;
                userSession.RoleID = user.RoleID;
                userSession.Token = token.Token;
                return Ok(token);
            }
        }
    }
}