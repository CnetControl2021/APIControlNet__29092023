using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AspNetRolController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public AspNetRolController(UserManager<IdentityUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager, CnetCoreContext context, IMapper mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.context = context;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Crear(AspNetRolesDTO aspNetRolesDTO)
        {
            var resultado = await roleManager.CreateAsync(new IdentityRole(aspNetRolesDTO.Name));
            if (resultado.Succeeded)
            {
                return Ok(aspNetRolesDTO.Id);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }


        //[HttpPost]
        //public async Task<ActionResult> AsignarPaginaRol(AspNetRolesDTO oaspNetRolesDTO)
        //{
        //    using var transaccion = await context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        if (oaspNetRolesDTO.Id is not null)
        //        {
        //            var orole = await roleManager.FindByIdAsync(oaspNetRolesDTO.Id);
        //            orole.Name = oaspNetRolesDTO.Name;
        //            context.SaveChanges();


        //            //List<ChPageDTO> chPageDTOs = (from ch in oaspNetRolesDTO.ChPageRols 
        //            //                              where oaspNetRolesDTO.Id.ToString() = orole.Id select ch).ToList();

        //            if (oaspNetRolesDTO.ArrayId.Count > 0)
        //            {
        //                foreach (string num in oaspNetRolesDTO.ArrayId)
        //                {
        //                    string nveces = context.ChPageRols.Where(r => r.IdRol as oaspNetRolesDTO.id)
        //                }
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        transaccion.Rollback();
        //    }
        //    return Ok(transaccion);
        //}





        //[HttpPost]
        //public async Task<ActionResult> Crear(AspNetRolesDTO aspNetRolesDTO)
        //{
        //    var resultado = await roleManager.CreateAsync(new IdentityRole(aspNetRolesDTO.Name));

        //    if (aspNetRolesDTO.ChPagesIds == null)
        //    {
        //        return BadRequest("No se puede crear rol sin paginas");
        //    }

        //    var paginasIds = await context.ChPages
        //        .Where(chpageDB => aspNetRolesDTO.ChPagesIds.Contains(chpageDB.Id)).Select(x => x.Id).ToListAsync();


        //    if (aspNetRolesDTO.ChPagesIds.Count != paginasIds.Count)
        //    {
        //        return BadRequest("No existe alguna de las paginas enviadas");
        //    }

        //    var aspNetRole = mapper.Map<AspNetRole>(aspNetRolesDTO);
        //    context.Add(aspNetRole);
        //    await context.SaveChangesAsync();
        //    return Ok();

        //}




        //[HttpGet]
        //public async Task<ActionResult<List<AspNetRolesDTO>>> ListadoRoles([FromQuery] PaginacionDTO paginacionDTO)
        //{
        //    var queryable = roleManager.Roles.AsQueryable();
        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var roles = await queryable.Paginar(paginacionDTO).ToListAsync();
        //    return Ok(roles);
        //}

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AspNetRolesDTO>>> Get5()
        {
            var data = await (from rol in roleManager.Roles        
                              //join rolPer in context.RolePermissions on rol.Id equals rolPer.RoleId
                              select new AspNetRolesDTO
                              {
                                  Id = rol.Id,
                                  Name = rol.Name,
                                  //RoleId = rolPer.RoleId,
                                  //Description = rolPer.Description
                              }).AsNoTracking().ToListAsync();
            return data;
        }


        [HttpGet("{id}", Name = "obtenerRol")]
        public async Task<ActionResult<AspNetRolesDTO>> Get(string id)
        {
            //var user = await userManager.Users.FirstOrDefaultAsync(claim => claim.Id == id);
            var rol = await roleManager.FindByIdAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            return Ok(rol);
        }


        [HttpPut]
        public async Task<ActionResult> Actualizar(AspNetRolesDTO aspNetRolesDTO)
        {
            var role = await roleManager.FindByIdAsync(aspNetRolesDTO.Id);
            role.Name = aspNetRolesDTO.Name;
            var result = await roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<AspNetRolesDTO>> Delete(string id)
        {
            var rol = await roleManager.FindByIdAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            else
            {
                var resultado = await roleManager.DeleteAsync(rol);
            }
            return NoContent();
        }

    }
}
