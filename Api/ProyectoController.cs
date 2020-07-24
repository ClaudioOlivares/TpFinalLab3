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
        public async Task<ActionResult> Post(ProyectoMasImagenesView p)
        {
            int numeroimagen = 1;

            try
            {

                Proyecto proyecto = new Proyecto();

                var usuario = context.User.FirstOrDefault(x => x.Email == User.Identity.Name);

                proyecto.Titulo = p.Titulo;

                proyecto.Genero = p.Genero;

                proyecto.Status = p.Status;

                proyecto.Plataforma = p.Plataforma;

                proyecto.TextoResumen = p.TextoResumen;

                proyecto.TextoCompleto = p.TextoCompleto;

                proyecto.IdUser = usuario.IdUser;

                proyecto.FechaCreacion = DateTime.Now;

                context.Proyecto.Add(proyecto);

                context.SaveChanges();

                string wwwpath = enviroment.WebRootPath;

                string path = Path.Combine(wwwpath, "uploads");


                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                //-------------------------------PORTADA---------------------------//

                byte[] bytesImagenPortada = Convert.FromBase64String(p.Portada);

                string fileNamePortada = "portada_" + proyecto.IdProyecto.ToString() + ".jpg";

                string pathCompletoPortada = Path.Combine(path, fileNamePortada);


                using (var imageFilePortada = new FileStream(pathCompletoPortada, FileMode.Create))
                {
                    imageFilePortada.Write(bytesImagenPortada, 0, bytesImagenPortada.Length);

                    imageFilePortada.CopyTo(imageFilePortada);

                    imageFilePortada.Flush();

                    proyecto.Portada = "uploads/" + fileNamePortada;

                }

                //------------------------------Video TRAILER-------------------------//


                if (p.VideoTrailer != null)
                {
                    byte[] bytes = Convert.FromBase64String(p.VideoTrailer);

                    string fileName = "IPVideoTrailer_" + proyecto.IdProyecto.ToString() + ".mp4";

                    string pathCompleto = Path.Combine(path, fileName);
             

                    using (var videoFile = new FileStream(pathCompleto, FileMode.Create))
                    {
                        videoFile.Write(bytes, 0, bytes.Length);

                        videoFile.CopyTo(videoFile);

                        videoFile.Flush();

                        proyecto.VideoTrailer = "uploads/" + fileName;

                    }
                }


                //-------------------------------VideoThumbnail------------------------------//

                if (p.Video != null)
                {
                    byte[] bytesVideoCorto = Convert.FromBase64String(p.Video);

                    string fileNameVideoCorto = "video_" + proyecto.IdProyecto.ToString() + ".mp4";

                    string pathCompletoVideoCorto = Path.Combine(path, fileNameVideoCorto);

                    using (var videoFileVideoCorto = new FileStream(pathCompletoVideoCorto, FileMode.Create))
                    {
                        videoFileVideoCorto.Write(bytesVideoCorto, 0, bytesVideoCorto.Length);

                        videoFileVideoCorto.CopyTo(videoFileVideoCorto);

                        videoFileVideoCorto.Flush();

                        proyecto.Video = "uploads/" + fileNameVideoCorto;
                    }

                }

                //------------------------------IMAGENES DEL PROYECTO----------------------------------//

                List<ImagenProyecto> imagenes = new List<ImagenProyecto>();

                foreach (ImagenProyecto item in p.imagenes)
                {
                  
                    byte[] bytesImagen = Convert.FromBase64String(item.Url);

                    string ImagenName = "ProyectoImg" + numeroimagen + "_" + proyecto.IdProyecto.ToString() + ".jpg";

                    string pathCompletoimg = Path.Combine(path, ImagenName);


                    using (var imageFile = new FileStream(pathCompletoimg, FileMode.Create))
                    {
                        ImagenProyecto imagen = new ImagenProyecto();

                        imagen.IdProyecto = proyecto.IdProyecto;

                        imageFile.Write(bytesImagen, 0, bytesImagen.Length);

                        imageFile.CopyTo(imageFile);

                        imageFile.Flush();

                        imagen.Url = "uploads/" + ImagenName;

                        imagenes.Add(imagen);
                    }

                    numeroimagen++;
                }

                context.Proyecto.Update(proyecto);

                context.SaveChanges();


                foreach (ImagenProyecto item in imagenes)
                {
                    item.IdProyecto = proyecto.IdProyecto;

                    context.ImagenProyecto.Add(item);

                    context.SaveChanges();
                }
               
                return Ok(proyecto);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // PUT: api/Proyecto/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                string wwwpath = enviroment.WebRootPath;

                var devlogs = context.DevLog.Include(x => x.Proyecto).Where(x => x.IdProyecto == id).ToList();

                if(devlogs != null)
                {
                    foreach (DevLog item in devlogs)
                    {
                        var itemdls = context.DevLogItem.Include(x => x.DevLog).Where(x=>x.IdDevLog == item.IdDevLog).ToList();

                        if(itemdls != null)
                        {
                            foreach (DevLogItem item2 in itemdls)
                            {
                                String PathCompleto = wwwpath + "/" + item2.Multimedia;

                                System.IO.File.Delete(PathCompleto);

                                context.DevLogItem.Remove(item2);

                                context.SaveChanges();



                            }
                        }

                        context.DevLog.Remove(item);

                        context.SaveChanges();


                    }
                }

                //------------------------------------PROYECTO---------------------------------//


                var imagenesProyecto = context.ImagenProyecto.Include(x => x.Proyecto).Where(x => x.IdProyecto == id).ToList();
               
                if(imagenesProyecto != null)
                {

                    foreach ( ImagenProyecto item3 in imagenesProyecto)
                    {
                        String PathCompleto = wwwpath + "/" + item3.Url;

                        System.IO.File.Delete(PathCompleto);

                        context.ImagenProyecto.Remove(item3);

                        context.SaveChanges();

                    }
                }

                var proyectoAborrar = context.Proyecto.Include(x => x.User).FirstOrDefault(x => x.IdProyecto == id);

                if(proyectoAborrar != null)
                {
                    String PathPortada = wwwpath + "/" +proyectoAborrar.Portada;

                    System.IO.File.Delete(PathPortada);

                    String PathVideoTrailer = wwwpath+ "/" +proyectoAborrar.VideoTrailer;

                    System.IO.File.Delete(PathVideoTrailer);

                    String PathVideoThumbnail = wwwpath + "/" + proyectoAborrar.Video;

                    System.IO.File.Delete(PathVideoThumbnail);

                    context.Proyecto.Remove(proyectoAborrar);

                    context.SaveChanges();
                }

                var proyecto = context.Proyecto.Include(x=>x.User).Last();

                return Ok(proyecto);
            }
            catch (Exception ex)
            {

               return  BadRequest(ex);
            }
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
            int numeroimagen = 1;

            try
            {
                string wwwpath = enviroment.WebRootPath;

                string path = Path.Combine(wwwpath, "uploads");


                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                }

                //------------------------- Video-----------------------------------------//

                if (p.VideoTrailer != null)
                {
                    byte[] bytes = Convert.FromBase64String(p.VideoTrailer);

                    string fileName = "IPVideoTrailer_" + p.IdProyecto.ToString() + ".mp4";

                    string pathCompleto = Path.Combine(path, fileName);

                    System.IO.File.Delete(pathCompleto);


                    using (var videoFile = new FileStream(pathCompleto, FileMode.Create))
                    {
                        videoFile.Write(bytes, 0, bytes.Length);

                        videoFile.CopyTo(videoFile);

                        videoFile.Flush();

                    }
                }
                            

                //------------------------------IMAGENES DEL PROYECTO----------------------------------//

                foreach (ImagenProyecto item in p.imagenes)
                {
                    

                    byte[] bytesImagen = Convert.FromBase64String(item.Url);

                    string ImagenName = "ProyectoImg" + numeroimagen + "_" + p.IdProyecto.ToString() + ".jpg";

                    string pathCompletoimg = Path.Combine(path, ImagenName);

                    System.IO.File.Delete(pathCompletoimg);


                    using (var imageFile = new FileStream(pathCompletoimg, FileMode.Create))
                    {
                        imageFile.Write(bytesImagen, 0, bytesImagen.Length);

                        imageFile.CopyTo(imageFile);

                        imageFile.Flush();

                    }

                    numeroimagen++;
                }

                //---------------------------------------------PORTADA---------------------------------//

                byte[] bytesImagenPortada = Convert.FromBase64String(p.Portada);

                string fileNamePortada = "portada_" + p.IdProyecto.ToString() + ".jpg";

                string pathCompletoPortada = Path.Combine(path, fileNamePortada);

                System.IO.File.Delete(pathCompletoPortada);

                using (var imageFilePortada = new FileStream(pathCompletoPortada, FileMode.Create))
                {
                    imageFilePortada.Write(bytesImagenPortada, 0, bytesImagenPortada.Length);

                    imageFilePortada.CopyTo(imageFilePortada);

                    imageFilePortada.Flush();

                }

                //------------------------------------------- VIDEO CORTO --------------------------------------------------
                if(p.Video != null)
                {
                    byte[] bytesVideoCorto = Convert.FromBase64String(p.Video);

                    string fileNameVideoCorto = "video_" + p.IdProyecto.ToString() + ".mp4";

                    string pathCompletoVideoCorto = Path.Combine(path, fileNameVideoCorto);

                    System.IO.File.Delete(pathCompletoVideoCorto);

                    using (var videoFileVideoCorto = new FileStream(pathCompletoVideoCorto, FileMode.Create))
                    {
                        videoFileVideoCorto.Write(bytesVideoCorto, 0, bytesVideoCorto.Length);

                        videoFileVideoCorto.CopyTo(videoFileVideoCorto);

                        videoFileVideoCorto.Flush();

                    }

                }          

                var j = context.Proyecto.Include(x => x.User).FirstOrDefault(x => x.User.Email == User.Identity.Name && x.IdProyecto == p.IdProyecto);

                j.TextoCompleto = p.TextoCompleto;

                j.TextoResumen = p.TextoResumen;

                j.Titulo = p.Titulo;

                j.Genero = p.Genero;

                j.Plataforma = p.Plataforma;

                j.Status = p.Status;


                context.Proyecto.Update(j);

                context.SaveChanges();

                return Ok(j);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
            

        }

    }


 
}
