﻿using System;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DevLogItemController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration config;

        public DevLogItemController(DataContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
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
        public async Task<ActionResult<IEnumerable<DevLogItem>>> Get (int id)
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
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/DevLogItem/5
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
