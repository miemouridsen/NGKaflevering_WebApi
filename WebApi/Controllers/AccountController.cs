﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;
using WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using WebApi.Utilities;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    { 
        const int BcryptWorkfactor = 10;
        private readonly MeasurementDbContext _context;
        private readonly AppSettings _appSettings;

        public AccountController(MeasurementDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        // POST (register) new user
        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<UserDto>> Register(UserDto regUser)
        {
            regUser.Email = regUser.Email.ToLower();
            var emailExist = await _context.User.Where(u =>
            u.Email == regUser.Email).FirstOrDefaultAsync();
            if (emailExist != null)
                return BadRequest(new { errorMessage = "Email already in use" });
            User user = new User()
            {
                Email = regUser.Email,
                FirstName = regUser.FirstName,
                LastName = regUser.LastName
            };
            user.PwHash = HashPassword(regUser.Password, BcryptWorkfactor);
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = user.UserId }, regUser);
        }

        // GET by id
        [HttpGet("{id}", Name = "GetAccount")]
        public async Task<ActionResult<UserDto>> Get(long id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = new UserDto();
            userDto.Email = user.Email;
            userDto.FirstName = user.FirstName;
            userDto.LastName = user.LastName;
            return userDto;
        }

        // POST (login with user)
        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<TokenDto>> Login(UserDto login)
        {
            login.Email = login.Email.ToLower();
            var user = await _context.User.Where(u =>
            u.Email == login.Email).FirstOrDefaultAsync();
            if (user != null)
            {
                var validPwd = Verify(login.Password, user.PwHash);
                if (validPwd)
                {
                    var token = new TokenDto();
                    token.JWT = GenerateToken(user);
                    return token;

                }
            }
            ModelState.AddModelError(string.Empty, "Forkert brugernavn eller password");
            return BadRequest(ModelState);
        }
        private string GenerateToken(User user)
        {
            var claims = new Claim[]
            {
             new Claim("Email", user.Email),
             new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
             new Claim("UserId", user.UserId.ToString()),
             new Claim(JwtRegisteredClaimNames.Exp,
             new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var token = new JwtSecurityToken(
            new JwtHeader(new SigningCredentials(
            new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)),
            new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    } 
}

