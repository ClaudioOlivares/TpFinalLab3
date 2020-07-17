using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        public DevLogController(DataContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
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

        // POST: api/DevLog
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/DevLog/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
