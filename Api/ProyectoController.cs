using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TpFinalLab3.Models;

namespace TpFinalLab3.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProyectoController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration config;
        private readonly IHostingEnvironment enviroment;

        public ProyectoController(DataContext context, IConfiguration config,IHostingEnvironment enviroment )
        {
            this.context = context;
            this.config = config;
            this.enviroment = enviroment;
        }
        // GET: api/Proyecto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proyecto>>> Get()
        {
            var j = context.User.FirstOrDefault(x => x.Email == User.Identity.Name);

            return context.Proyecto.Where(x => x.IdUser == j.IdUser).ToList();
        }

        // GET: api/Proyecto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proyecto>> Get(int id)
        {
            try
            {
                var j = context.Proyecto.Include(x => x.User).FirstOrDefault(x => x.IdProyecto == id);

                return Ok(j);

            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        // POST: api/Proyecto
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Proyecto/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("checkear")]
        public async Task<IActionResult> checkear([FromBody] int id)
        {
            try
            {
                var j = context.Proyecto.FirstOrDefault(x => x.User.Email == User.Identity.Name && x.IdProyecto == id);

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
        public async Task<IActionResult> actualizar(ProyectoMasImagenesView p)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(p.Video);

                string wwwpath = enviroment.WebRootPath;

                string path = Path.Combine(wwwpath, "uploads");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = "IPVideoTrailer_" + p.IdProyecto.ToString() + ".mp4";

                string pathCompleto = Path.Combine(path, fileName);

                System.IO.File.Delete(pathCompleto);


                using (var videoFile = new FileStream(pathCompleto, FileMode.Create))
                {
                    videoFile.Write(bytes, 0, bytes.Length);

                    videoFile.CopyTo(videoFile);

                    videoFile.Flush();

                }

                foreach (ImagenProyecto item in p.imagenes)
                {
                    byte[] bytesImagen = Convert.FromBase64String(item.Url);

                    string ImagenName = "ProyectoImg" + item.IdImagenProyecto.ToString() + "_" + p.IdProyecto.ToString();

                    string pathCompletoimg = Path.Combine(path, ImagenName);

                    System.IO.File.Delete(pathCompletoimg);


                    using (var imageFile = new FileStream(pathCompletoimg, FileMode.Create))
                    {
                        imageFile.Write(bytesImagen, 0, bytesImagen.Length);

                        imageFile.CopyTo(imageFile);

                        imageFile.Flush();

                    }
                }

                var j = context.Proyecto.Include(x => x.User).FirstOrDefault(x => x.User.Email == User.Identity.Name);

                j.TextoCompleto = p.TextoCompleto;

                j.TextoResumen = p.TextoResumen;

                j.Titulo = p.Titulo;

                j.Genero = p.Genero;

                j.Plataforma = p.Plataforma;

                j.Status = p.Status;

                context.Proyecto.Update(j);

                context.SaveChanges();

                return Ok("Perfil Actualizado");
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
            

        }

    }


 
}
