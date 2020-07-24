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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DevLogItemController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration config;
        private readonly IHostingEnvironment enviroment;

        public DevLogItemController(DataContext context, IConfiguration config, IHostingEnvironment enviroment)
        {
            this.context = context;
            this.config = config;
            this.enviroment = enviroment;
        }
        // GET: api/DevLogItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DevLogItem>>> Get()
        {
            try
            {
                var j = context.DevLogItem.Include(x => x.DevLog);

                return Ok(j);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/DevLogItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<DevLogItem>>> Get(int id)
        {
            try
            {

                var j = context.DevLogItem.Include(x => x.DevLog).Where(x => x.IdDevLog == id);

                return Ok(j);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }

        // POST: api/DevLogItem
        [HttpPost]
        public async Task<ActionResult> Post (DevLogItem dli)
        {
            try
            {
                string wwwpath = enviroment.WebRootPath;

                string path = Path.Combine(wwwpath, "uploads");


                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                }
                byte[] bytes = Convert.FromBase64String(dli.Multimedia);

                dli.Multimedia = "A"; // para que no guarde semejante cadena

                context.DevLogItem.Add(dli);

                context.SaveChanges();

                string fileName = "ItemDevLog" + dli.IdDevLogItem.ToString() + "_" + dli.IdDevLog + ".jpg";

                string pathCompleto = Path.Combine(path, fileName);

                using (var imageFile = new FileStream(pathCompleto, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);

                    imageFile.CopyTo(imageFile);

                    imageFile.Flush();

                }

                dli.Multimedia = "uploads/" + fileName;

                context.DevLogItem.Update(dli);

                context.SaveChanges();

                return Ok(dli);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        // PUT: api/DevLogItem/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var devlogitem = context.DevLogItem.Include(x => x.DevLog).FirstOrDefault(x => x.IdDevLogItem == id);

                string wwwpath = enviroment.WebRootPath;

                String PathCompleto = wwwpath + "/" + devlogitem.Multimedia;

                System.IO.File.Delete(PathCompleto);

                context.DevLogItem.Remove(devlogitem);

                context.SaveChanges();

                var j = context.DevLogItem.Include(x => x.DevLog).Last();

                return Ok(j);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        [HttpPost("checkear")]
        public async Task<IActionResult> checkear([FromBody] int id)
        {
            try
            {
                var j = context.DevLogItem.Include(x => x.DevLog).ThenInclude(x => x.Proyecto).ThenInclude(x => x.User).FirstOrDefault(x => x.IdDevLogItem == id && User.Identity.Name == x.DevLog.Proyecto.User.Email);

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


        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<DevLogItem>>> GetDevlogItem([FromBody]int id)
        {
            try
            {
                var j = context.DevLogItem.Include(x => x.DevLog).ThenInclude(x => x.Proyecto).ThenInclude(x => x.User).FirstOrDefault(x => x.IdDevLogItem == id);

                return Ok(j);
            }

            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> actualizar(DevLogItem d)
         {
            try
            {
                var j = context.DevLogItem.Include(x => x.DevLog).ThenInclude(x=>x.Proyecto).FirstOrDefault(x => x.IdDevLogItem == d.IdDevLogItem);

                j.Texto = d.Texto;

                j.Titulo = d.Titulo;

                string wwwpath = enviroment.WebRootPath;

                string path = Path.Combine(wwwpath, "uploads");


                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                }

                byte[] bytes = Convert.FromBase64String(d.Multimedia);

                string fileName = "ItemDevLog" + d.IdDevLogItem.ToString()+ "_" + j.IdDevLog + ".jpg";

                string pathCompleto = Path.Combine(path, fileName);

                System.IO.File.Delete(pathCompleto);


                using (var imageFile = new FileStream(pathCompleto, FileMode.Create))
                {
                   imageFile.Write(bytes, 0, bytes.Length);

                   imageFile.CopyTo(imageFile);

                   imageFile.Flush();

                }

                context.DevLogItem.Update(j);

                context.SaveChanges();

                return Ok(j);
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }
        }
    }
}
