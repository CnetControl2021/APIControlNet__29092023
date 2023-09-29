using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BinnacleController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public BinnacleController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }



        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<BinnacleDTO>> Get5(Guid storeId, DateTime dateIni, DateTime dateFin, [FromQuery] string nombre)
        {
            List<BinnacleDTO> list = new List<BinnacleDTO>();

            if (string.IsNullOrEmpty(nombre))
            {
                list = await (from bn in context.Binnacles
                              join btp in context.BinnacleTypes on bn.BinnacleTypeId equals btp.BinnacleTypeId
                              where (btp.IsVolumetric == 1 && bn.Date >= dateIni && bn.Date <= dateFin)
                              orderby bn.BinnacleIdx descending

                              select new BinnacleDTO
                              {
                                  StoreId = storeId,
                                  BinnacleIdx = bn.BinnacleIdx,
                                  Date = bn.Date,
                                  UserId = bn.UserId,
                                  ValueName = bn.ValueName,  //Typo de evento Add, edit del etc
                                  Name = bn.Name,
                                  Description = bn.Description,
                                  Response = bn.Response,
                                  IpAddress = bn.IpAddress,
                                  MacAddress = bn.MacAddress

                              }).AsNoTracking().ToListAsync();
            }
            else
            {
                list = await (from bn in context.Binnacles
                              join btp in context.BinnacleTypes on bn.BinnacleTypeId equals btp.BinnacleTypeId
                              where (btp.IsVolumetric == 1 && bn.Date >= dateIni && bn.Date <= dateFin)
                              orderby bn.BinnacleIdx descending

                              select new BinnacleDTO
                              {
                                  BinnacleIdx = bn.BinnacleIdx,
                                  Date = bn.Date,
                                  UserId = bn.UserId,
                                  ValueName = bn.ValueName,  //Typo de evento Add, edit del etc
                                  Description = bn.Description,
                                  Response = bn.Response,
                                  IpAddress = bn.IpAddress,
                                  MacAddress = bn.MacAddress
                              }).AsNoTracking().ToListAsync();
            }
            return Ok(list);
        }


        [HttpPost("{storeId?}")]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] BinnacleDTO binnacleDTO, Guid? storeId)
        {
            //var binnacle = mapper.Map<Binnacle>(binnacleDTO);

            //context.Add(binnacle);

            var usuarioId = obtenerUsuarioId();
            //var usuarioId = "Prueebas";
            var ipUser = obtenetIP();
            var name = binnacleDTO.Name;
            var storeId2 = storeId;
            await servicioBinnacle.ManualBinnacle(usuarioId, ipUser, name, storeId2);
            await context.SaveChangesAsync();
            return Ok();
        }

    }
    
}
