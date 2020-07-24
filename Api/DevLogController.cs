using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DevLogController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration config;
        private readonly IHostingEnvironment enviroment;

        public DevLogController(DataContext context, IConfiguration config, IHostingEnvironment enviroment)
        {
            this.context = context;
            this.config = config;
            this.enviroment = enviroment;
        }
        // GET: api/DevLog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DevLog>>> Get()
        {
            try
            {
                var j = context.DevLog.Include(x => x.Proyecto);

                return Ok(j);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // GET: api/DevLog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<DevLog>>> Get(int id)
        {
            try
            {
                var j = context.DevLog.Include(x => x.Proyecto).Where(x => x.IdProyecto == id);

                return Ok(j);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<DevLog>>> GetDevlog([FromBody]int id)
        {
            try
            {
                var j = context.DevLog.Include(x => x.Proyecto).FirstOrDefault(x => x.IdDevLog == id);

                return Ok(j);
            }

            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // POST: api/DevLog
        [HttpPost]
        public async Task<ActionResult> Post(DevLog devlog)
        {
            try
            {
                devlog.FechaCreacion = DateTime.Now;

                context.DevLog.Add(devlog);

                context.SaveChanges();

                return CreatedAtAction(nameof(Get), new { id = devlog.IdDevLog}, devlog);

            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        // PUT: api/DevLog/5
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
                string wwwpath = enviroment.WebRootPath;

                var devlogs = context.DevLog.Include(x => x.Proyecto).FirstOrDefault(x => x.IdDevLog == id);

                if (devlogs != null)
                {
                   
                        var itemdls = context.DevLogItem.Include(x => x.DevLog).Where(x => x.IdDevLog == devlogs.IdDevLog).ToList();

                        if (itemdls != null)
                        {
                            foreach (DevLogItem item2 in itemdls)
                            {
                                String PathCompleto = wwwpath + "/" + item2.Multimedia;

                                System.IO.File.Delete(PathCompleto);

                                context.DevLogItem.Remove(item2);

                                context.SaveChanges();

                            }
                        }

                        context.DevLog.Remove(devlogs);

                        context.SaveChanges();

                }
                var devlog = context.DevLog.Include(x => x.Proyecto).Last();

                return Ok(devlog);
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
                var j = context.DevLog.Include(x => x.Proyecto).ThenInclude(x => x.User).FirstOrDefault(x => x.Proyecto.User.Email == User.Identity.Name && x.Proyecto.IdProyecto == id);

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
        public async Task<IActionResult> actualizar(DevLog devlog)
        {
            try
            {
                var j = context.DevLog.Include(x => x.Proyecto).ThenInclude(x => x.User).FirstOrDefault(x => x.IdDevLog == devlog.IdDevLog);

                j.Titulo = devlog.Titulo;

                j.Resumen = devlog.Resumen;

                context.DevLog.Update(j);

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
