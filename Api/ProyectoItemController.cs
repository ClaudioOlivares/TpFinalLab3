using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProyectoItemController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration config;

        public ProyectoItemController(DataContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }
        // GET: api/ProyectoItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProyectoItem>>> Get()
        {
            try
            {
                var j = context.ProyectoItem.Include(x => x.Proyecto);
                return Ok(j);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        // GET: api/ProyectoItem/5
        [HttpGet("{id}", Name = "GetProyectoItem")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ProyectoItem
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ProyectoItem/5
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
