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
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProyectoController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration config;

        public ProyectoController(DataContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }
        // GET: api/Proyecto
        [HttpGet ]
        public async Task<ActionResult<IEnumerable<Proyecto>>> Get()
        {
           
            return context.Proyecto.Include(x=>x.User).ToList();
         
        }   

        // GET: api/Proyecto/5
        [HttpGet("{id}", Name = "GetProyecto")]
        public string Get(int id)
        {
            return "value";
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
    }
}
