using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PetitionCustomController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public PetitionCustomController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet("byGuid/{idGuid}")]
        [AllowAnonymous]
        public async Task<IEnumerable<PetitionCustomDTO>> Get(Guid idGuid)
        {
            var dataDB = await context.InventoryInDocuments.Where(x => x.InventoryInId == idGuid).FirstOrDefaultAsync();
            var petitionCustomId = dataDB.PetitionCustomsId;

            var listPetitions = await context.PetitionCustoms.Where(x => x.PetitionCustomsId == petitionCustomId).ToListAsync();

            return mapper.Map<List<PetitionCustomDTO>>(listPetitions);
        }

        [HttpPost("{storeId?}/{idGuid}")]
        public async Task<ActionResult> Post(PetitionCustomDTO petitionCustomDTO, Guid storeId, Guid idGuid)
        {
            var petitionnCus = mapper.Map<PetitionCustom>(petitionCustomDTO);
            petitionnCus.PetitionCustomsId = Guid.NewGuid();
            var petCusId = petitionnCus.PetitionCustomsId;

            var dataDB = await context.InventoryInDocuments.Where(x => x.InventoryInId == idGuid).FirstOrDefaultAsync();
            dataDB.PetitionCustomsId = (petCusId);
            
            var udemed = dataDB.JsonClaveUnidadMedidaId;
            petitionnCus.JsonClaveUnidadMedidadId = udemed;
           
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = "Petition Custom";
            var storeId2 = storeId;

            context.Add(petitionnCus);
            context.Update(dataDB);

            await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);

            await context.SaveChangesAsync();
            return Ok();                    
        }

        [HttpGet("{id2:int}", Name = "obtainPetitionCustom")]
        [AllowAnonymous]
        public async Task<ActionResult<PetitionCustomDTO>> Get(int id2)
        {
            var data = await context.PetitionCustoms.FirstOrDefaultAsync(x => x.PetitionCustomsIdx == id2);
            if (data == null)
            {
                return NotFound();
            }
            return mapper.Map<PetitionCustomDTO>(data);
        }

        [HttpPut("{storeId}")]
        public async Task<ActionResult> Put(Guid storeId, PetitionCustomDTO petCus)
        {
            var dataDB = await context.PetitionCustoms.FirstOrDefaultAsync
                (c => c.PetitionCustomsIdx == petCus.PetitionCustomsIdx);

            if (dataDB is null)
            {
                return NotFound();
            }
            try
            {
                dataDB = mapper.Map(petCus, dataDB);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = "SupplierTransportResgister";

                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
