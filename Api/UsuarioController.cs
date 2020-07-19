using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TpFinalLab3.Models;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace TpFinalLab3.Api
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly DataContext context;
        private readonly IConfiguration config;
        private readonly IHostingEnvironment enviroment;


        public UsuarioController(DataContext context, IConfiguration config, IHostingEnvironment enviroment)
        {
            this.enviroment = enviroment;
            this.context = context;
            this.config = config;
        }
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> Get()
        {

            try
            {
                var j = context.User.FirstOrDefault(x => x.Email == User.Identity.Name);

                return Ok(j);

            }
            catch (System.Exception ex)
            {

                return BadRequest(ex);
            }
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            return context.User.FirstOrDefault(x => x.IdUser == id);
        }

        // POST: api/User
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginView loginView)
        {
            try
            {
                if (loginView.Email == null || loginView.Clave == null)
                {
                    return BadRequest("Ingrese todos los campos");
                }
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var p = context.User.FirstOrDefault(x => x.Email == loginView.Email);
                if (p == null || p.Clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, p.Email),
                        new Claim("FullName", p.Nombre + " " + p.Apellido),

                    };

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpPost( "checkear")]
        public async Task<IActionResult> checkear (CheckPerfilView perfil)
        {
            try
            {

                var j = context.User.FirstOrDefault(x => x.Email == perfil.Email);
                if (j == null)
                {
                    return Ok("false");
                }
                else
                {
                    return Ok("true");
                }

            }
            catch (Exception ex)
            {

               return BadRequest(ex);
            }
        }


        [HttpPut("actualizar")]
        public async Task<IActionResult> actualizar(Usuario perfil)
        {
            try
            {
                var x = context.User.AsNoTracking().FirstOrDefault(e => e.Email == User.Identity.Name);

                if(x != null && x.Email == perfil.Email)
                {
                    byte[] bytes = Convert.FromBase64String(perfil.Avatar);

                    string wwwpath = enviroment.WebRootPath;

                    string path = Path.Combine(wwwpath, "uploads");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "Avatar_" + x.IdUser.ToString() + ".jpg";

                    string pathCompleto = Path.Combine(path, fileName);

                    System.IO.File.Delete(pathCompleto);

                    using (var imageFile = new FileStream(pathCompleto, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);

                        imageFile.CopyTo(imageFile);

                        imageFile.Flush();

                    }

                    perfil.IdUser = x.IdUser;

                    perfil.Clave = x.Clave;

                    perfil.Avatar = "uploads/" + fileName;

                    context.User.Update(perfil);

                    context.SaveChanges();

                    return Ok(perfil);
                }
                else
                {
                    return BadRequest("datos invalidos");
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }
    }

}
