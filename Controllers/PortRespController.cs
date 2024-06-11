using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortRespController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public PortRespController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        //[HttpGet("notPage")]
        //[AllowAnonymous]
        //public async Task<IEnumerable<PortResponseDTO>> Get3([FromQuery] Guid storeId)
        //{
        //    var queryable = context.PortResponses.Where(x => x.Active == true).AsQueryable();
        //    if (storeId != Guid.Empty)
        //    {
        //        queryable = queryable.Where(x => x.StoreId == storeId);
        //    }
        //    var pr = await queryable.OrderBy(x => x.PortIdi)
        //        .AsNoTracking()
        //        .ToListAsync();
        //    return mapper.Map<List<PortResponseDTO>>(pr);
        //}


        [HttpGet("notPage")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PortResponseDTO>>> Get([FromQuery] Guid storeId)
        {
            var data = await (from pr in context.PortResponses
                              where pr.StoreId == storeId
                              join dispb in context.DispensaryBrands on pr.DeviceBrand
                              equals dispb.DispensaryBrandId.ToString()
                              orderby pr.PortIdi, pr.DeviceName, pr.DeviceName, pr.CpuNumberLoop 
                              select new PortResponseDTO
                              {
                                  PortResponseIdx = pr.PortResponseIdx,
                                  StoreId = pr.StoreId,
                                  PortIdi = pr.PortIdi,
                                  DeviceName = pr.DeviceName,
                                  DeviceBrandName = dispb.Name,
                                  CpuAddress = pr.CpuAddress,
                                  CpuNumberLoop = pr.CpuNumberLoop,
                                  CommPercentage = pr.CommPercentage,
                                  Response = pr.Response,
                                  Updated = pr.Updated,
                                  Active = pr.Active,
                                  Response2 = pr.Response2,
                                  InUse = pr.InUse
                              }).AsNoTracking().ToListAsync();
            var prDTOs = mapper.Map<IEnumerable<PortResponseDTO>>(data);
            return Ok(prDTOs);
        }
    }
}
