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

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TankController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public TankController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("notPage")]
        [AllowAnonymous]
        public async Task<IEnumerable<TankDTO>> Get3([FromQuery] Guid storeId)
        {
            var queryable = context.Tanks.Where(x => x.Active == true).AsQueryable();
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var t = await queryable.OrderBy(x => x.TankIdi)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<TankDTO>>(t);
        }


        [HttpGet("byStore/{id2}")] //por store
        //[AllowAnonymous]
        public async Task<ActionResult<List<TankDTO>>> Get2([FromRoute] Guid id2)
        {
            var RSs = await context.Tanks.Where(e => e.StoreId.Equals((Guid)id2))
                .ToListAsync();
            return mapper.Map<List<TankDTO>>(RSs);
        }


        [HttpGet("{id:int}", Name = "obtenerTank")]
        public async Task<ActionResult<TankDTO>> Get(int id)
        {
            var tank = await context.Tanks.FirstOrDefaultAsync(x => x.TankIdx == id);

            if (tank == null)
            {
                return NotFound();
            }
            return mapper.Map<TankDTO>(tank);
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<TankDTO>>> Get([FromRoute] string nombre)
        {
            var RSs = await context.Tanks.Where(TankDB => TankDB.Name.Contains(nombre)).ToListAsync();
            return mapper.Map<List<TankDTO>>(RSs);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid storeId, List<TankDTO> tankDTOs)
        {
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = tankDTOs.LastOrDefault().Name;
            var storeId2 = storeId;
            var tabla = "Tanks";

            if (tankDTOs == null || !tankDTOs.Any())
            {
                return BadRequest("Sin datos");
            }
            foreach (var dto in tankDTOs)
            {
                var existingEntity = await context.Tanks
                    .FindAsync(dto.TankIdx);

                if (existingEntity != null)
                {

                    context.Entry(existingEntity).CurrentValues.SetValues(dto);
                    existingEntity.Updated = DateTime.Now;
                    context.Tanks.Update(existingEntity);
                    await servicioBinnacle.EditBinnacle2(usuarioId, ipUser, name, storeId2, tabla);
                }
                else if (!dto.TankIdx.HasValue || dto.TankIdx == 0)
                {
                    var newEntity = new Tank
                    {
                        StoreId = storeId,
                        TankIdi = dto.TankIdi,
                        ProductId = dto.ProductId,
                        TankCpuAddress = dto.TankCpuAddress,
                        PortIdi = dto.PortIdi,
                        TankBrandId = dto.TankBrandId,
                        Name = dto.Name,
                        CapacityTotal = dto.CapacityTotal,
                        CapacityOperational = dto.CapacityOperational,
                        CapacityMinimumOperating = dto.CapacityMinimumOperating,
                        CapacityUseful = dto.CapacityUseful,
                        Fondage = dto.Fondage,
                        SatDateCalibration = dto.SatDateCalibration,
                        SatTypeMeasurement = dto.SatTypeMeasurement,
                        SatTankType = dto.SatTankType,
                        SatTypeMediumStorage = dto.SatTypeMediumStorage,
                        SatDescriptionMeasurement = dto.SatDescriptionMeasurement,
                        SatPercentageUncertaintyMeasurement = dto.SatPercentageUncertaintyMeasurement,
                        EnableGetInventory = dto.EnableGetInventory,
                        Date = DateTime.Now,
                        Updated = DateTime.Now,
                        Active = true,
                        Locked = false,
                        Deleted = false,
                        UtilityPercentaje = dto.UtilityPercentaje,
                        TankShapeId = dto.TankShapeId,
                        DiameterOrWidth = dto.DiameterOrWidth,
                        Length = dto.Length,
                        Height = dto.Height,
                        HeightStart = dto.HeightStart,
                        HeightNotFuel = dto.HeightNotFuel,
                        MultiplicationFactor = dto.MultiplicationFactor,
                        CalculateQuantityWithTable = dto.CalculateQuantityWithTable,
                        TankCpuAddressNew = dto.TankCpuAddressNew,
                        CapacityGastalon = dto.CapacityGastalon
                    };
                    context.Tanks.Add(newEntity);
                    await servicioBinnacle.AddBinnacle2(usuarioId, ipUser, name, storeId2, tabla);
                }
            }
            await context.SaveChangesAsync();
            return Ok();
        }



        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.Tanks.AnyAsync(x => x.TankIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.Tanks.FirstOrDefaultAsync(x => x.TankIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Name;
            var storeId2 = storeId;
            await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name, storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}/{storeId?}")]
        public async Task<IActionResult> Delete(int id, Guid storeId)
        {
            try
            {
                var existe = await context.Tanks.AnyAsync(x => x.TankIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.Tanks.FirstOrDefaultAsync(x => x.TankIdx == id);
                context.Remove(name2);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = name2.Name;
                var storeId2 = storeId;
                var tabla = "Tanks";
                await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2, tabla);

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return BadRequest("ERROR DE DATOS RELACIONADOS");
            }

        }

        [HttpGet("/tankShape/noPage")]
        public async Task<ActionResult<IEnumerable<TankShape>>> Get5()
        {
            var ts = await context.TankShapes.ToListAsync();
            return Ok(ts);
        }

        [HttpGet("/tankBrand/noPage")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TankBrand>>> Get6()
        {
            var tb = await context.TankBrands.ToListAsync();
            return Ok(tb);
        }
    }
}
