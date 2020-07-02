using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TpFinalLab3.Models;

namespace TpFinalLab3.Api
{
  

    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly DataContext context;
        private readonly IConfiguration config;

        public UsuarioController(DataContext context,IConfiguration config)
        {
           
            this.context = context;
            this.config = config;
        }
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> Get()
        {
            
            try
            {
                return Ok(context.User);
               
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
            return context.User.FirstOrDefault(x=>x.IdUser == id);
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
    }
}
