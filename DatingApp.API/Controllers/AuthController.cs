using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _config = config;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDTO userregisterDto)
        {
            // We will validate the request
            userregisterDto.Username = userregisterDto.Username.ToLower();
            if (await _repo.UserExists(userregisterDto.Username))
            {
                return BadRequest("User already exists");
            }

            var newUser = _mapper.Map<User>(userregisterDto);

            var newCreatedUser = await _repo.Register(newUser, userregisterDto.password);    

            var userToReturn = _mapper.Map<UserforDetailDTO>(newCreatedUser);
            // var abc = newCreatedUser.Id ;
            return CreatedAtRoute("GetUser" , new{controller = "Users" , id = newCreatedUser.id} , userToReturn);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
            //throw new Exception("Something went wrong");
            userLogin.username = userLogin.username.ToLower();
            var userFromrepo = await _repo.Login(userLogin.username, userLogin.password);
            if (userFromrepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier , userFromrepo.id.ToString()),
                new Claim(ClaimTypes.Name , userFromrepo.UserName)

            };

            // We are creating a key server will validate through this key each time user requests for 
            // any service(calls any api).
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                        (_config.GetSection("AppSettings:Token").Value));


            // We are encoding this key with security algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForListDTO>(userFromrepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });


        }

        // [HttpGet]

        // public async Task<bool> UserExists(string username){

        // }
    }
}