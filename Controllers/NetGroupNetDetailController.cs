using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NetGroupNetDetailController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public NetGroupNetDetailController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NetgroupNetDetailDTO>>> Get(int skip, int take, string userName, string searchTerm = "")
        {
            var data = await (from ngnd in context.NetgroupNetDetails
                              join ngn in context.NetgroupNets on ngnd.NetgroupNetId equals ngn.NetgroupNetId
                              select new
                              {
                                  ngnd.NetgroupNetDetailIdx,
                                  ngnd.NetgroupNetId,
                                  ngnd.NetgroupNetStore,
                                  ngn.NetgroupNetName
                              }).AsNoTracking().ToListAsync();

            var query = data.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.NetgroupNetName.ToLower().Contains(searchTerm));
            }
            var data2 = await query.Skip(skip).Take(take).ToListAsync();
            var ntotal = data2.Count();

            return Ok(new { data2, ntotal });
        }

    }
}
