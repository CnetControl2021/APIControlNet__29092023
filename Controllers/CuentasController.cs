using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[DisableCors]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CuentasController : CustomBaseController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly ServicioBinnacle servicioBinnacle;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly CnetCoreContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        //public bool DisplayConfirmAccountLink { get; private set; }

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration, ServicioBinnacle servicioBinnacle,
            SignInManager<IdentityUser> signInManager, CnetCoreContext context, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.servicioBinnacle = servicioBinnacle;
            this.signInManager = signInManager;
            this.context = context;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        //[HttpGet("listadoUsuarios")]
        //[AllowAnonymous]
        //public IActionResult ListUsers()
        //{
        //    var users = userManager.Users;

        //    //foreach (var usuasio  in users)
        //    //{

        //    //}
        //    //    .Select(user => new
        //    //    {
        //    //        UserId = user.Id,
        //    //        UserName = user.Email,
        //    //        Roles = userManager.GetRolesAsync(user).Result
        //    //    })
        //    //    .ToListAsync();
        //    //return Ok(users);
        //}

        [HttpGet("ByRolNG")]
        //[AllowAnonymous]
        public async Task<ActionResult> ListUsersByRole(int skip, int take, string searchTerm = "")
        {
            try
            {
                var usersQuery = userManager.Users.AsQueryable();
                var rolesQuery = roleManager.Roles.AsQueryable();

                var result = from user in usersQuery
                             join userRole in context.UserRoles on user.Id equals userRole.UserId
                             join role in rolesQuery on userRole.RoleId equals role.Id
                             where role.Name == "RolByNetGroup"
                             select new
                             {
                                 Id = user.Id,
                                 UserName = user.UserName,
                                 Email = user.Email,
                                 RoleName = role.Name
                             };

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    result = result.Where(c => c.Email.ToLower().Contains(searchTerm) || c.UserName.ToLower().Contains(searchTerm));
                }

                var query = await result.Skip(skip).Take(take).OrderByDescending(x => x.UserName).ToListAsync();
                var ntotal = await result.CountAsync();

                return Ok(new { query, ntotal });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("registrarNG")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> RegistrarNG(CredencialesUsuariosDTO credencialesUsuariosDTO,
          int? netgroupidi, string? netgroupname, int? netgroupUserTypeId)
        {
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {
                    var aspNetUserDb = await context.Users.AnyAsync(x => x.Email == credencialesUsuariosDTO.Email);
                    if (aspNetUserDb) { return BadRequest("Usuario ya existe"); }

                    var usuario = new IdentityUser
                    {
                        UserName = credencialesUsuariosDTO.Email,
                        Email = credencialesUsuariosDTO.Email,
                    };

                    var resultado = await userManager.CreateAsync(usuario, credencialesUsuariosDTO.Password); //Respuesta Identity
                    usuario.EmailConfirmed = true;
                    var emailconfimed = usuario.EmailConfirmed;

                    string roleName = "RolByNetGroup"; //  rol RolByNetGroup

                    await userManager.AddToRoleAsync(usuario, roleName);

                    //context.SaveChanges();

                    //Netgroup oNg = new()
                    //{
                    //    NetgroupId = Guid.NewGuid(),
                    //    NetgroupIdi = netgroupidi,
                    //    NetgroupName = netgroupname,
                    //    ShortDescription = netgroupname,
                    //    Date = DateTime.Now,
                    //    Updated = DateTime.Now,
                    //    Active = true,
                    //    Locked = false,
                    //    Deleted = false
                    //};
                    //context.Netgroups.Add(oNg);
                    ////context.SaveChanges();

                    //NetgroupUser oNgUser = new()
                    //{
                    //    NetgroupId = oNg.NetgroupId,
                    //    UserId = usuario.Id,
                    //    NetgroupUserId = Guid.NewGuid(),
                    //    Name = usuario.Email,
                    //    Description = roleName,
                    //    NetgroupUserTypeId = netgroupUserTypeId,
                    //    Date = DateTime.Now,
                    //    Updated = DateTime.Now,
                    //    Active = true,
                    //    Locked = false,
                    //    Deleted = false
                    //};
                    //context.NetgroupUsers.Add(oNgUser);

                    context.SaveChanges();
                    await transaccion.CommitAsync();

                    if (resultado.Succeeded)
                    {
                        return ConstruirToken2(credencialesUsuariosDTO, new List<string>(), usuario.Id, emailconfimed, usuario.Id);
                    }
                    return Ok(resultado);

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message /*+ "Revisar que empresa y sucursal existan"*/);
            }
        }


        [HttpGet("listadoUsuarios")]
        [AllowAnonymous]
        public async Task<ActionResult<List<CredencialesUsuariosDTO>>> ListadoUsuarios([FromQuery] PaginacionDTO paginacionDTO, Guid companyId)
        {
            var userStoreCompany = await context.UserStores.FirstOrDefaultAsync(x => x.CompanyId == companyId);
            var queryable = userManager.Users.AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            //var usuarios = await queryable.Paginar(paginacionDTO).ToListAsync();
            var usuarios = await queryable.Paginar(paginacionDTO).Select(x => new CredencialesUsuariosDTO { Email = x.Email, Id = x.Id })
                .OrderBy(x => x.Email)
                .ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> Get(Guid storeId)
        {
            var storeDb = await context.Stores.FirstOrDefaultAsync(x => x.StoreId == storeId);
            var companyId = storeDb.CompanyId;
            try
            {
                var result = await (from us in context.UserStores
                                    join u in context.Users on us.UserId equals u.Id
                                    join c in context.Companies on us.CompanyId equals c.CompanyId
                                    where us.CompanyId == companyId
                                    orderby u.Email
                                    select new
                                    {
                                        us.UserStoreIdx,
                                        u.Id,
                                        u.Email,
                                        us.Date,
                                        c.Name
                                    }).ToListAsync();
                var conteo = result.Count();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("support")]
        [AllowAnonymous]
        public async Task<IActionResult> Get2(Guid storeId)
        {
            try
            {
                var result = await (from u in context.Users
                                    orderby u.Email
                                    select new
                                    {
                                        u.Id,
                                        u.Email,
                                    }).ToListAsync();


                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("byName/{textSearch?}")] // BlazoredTypeaheadNG
        [AllowAnonymous]
        public async Task<ActionResult<List<CredencialesUsuariosDTO>>> Get2(string textSearch)
        {
            //var queryable = userManager.Users.AsQueryable();

            var usersQuery = userManager.Users.AsQueryable();
            var rolesQuery = roleManager.Roles.AsQueryable();

            var result = from user in usersQuery
                         join userRole in context.UserRoles on user.Id equals userRole.UserId
                         join role in rolesQuery on userRole.RoleId equals role.Id
                         where role.Name == "RolByNetGroup"
                         select new CredencialesUsuariosDTO
                         {
                             Id = user.Id,
                             EmailConfirmed = user.EmailConfirmed,
                             Email = user.Email,
                             Roles = new List<string> { role.Name }
                         };

            if (!string.IsNullOrEmpty(textSearch))
            {
                result = result.Where(x => x.Email.ToLower().Contains(textSearch));

            }
            var s = await result
               .AsNoTracking().ToListAsync();
            return Ok(s);
        }


        //[HttpGet("listadoUsuarios2")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetUsersWithRoles()
        //{
        //    var users = userManager.Users.ToList();
        //    var userRoles = new List<CredencialesUsuariosDTO>();

        //    foreach (var user in users)
        //    {
        //        var roles = await userManager.GetRolesAsync(user);
        //        userRoles.Add(item: new CredencialesUsuariosDTO { Email = user.Email, Roles = (List<string>)roles });
        //    }

        //    return Ok(userRoles);
        //}

        [HttpGet("listadoUsuarios2")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<object>>> GetUsersAndRoles()
        {
            var users = await userManager.Users.ToListAsync();
            var userRoles = new List<object>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRoles.Add(new { User = user.UserName, Roles = roles });
            }

            return Ok(userRoles);
        }



        [HttpPost("AsignarRol/{storeId?}")] ///se guarda relacion en AspNetUserRoles
        //[AllowAnonymous]
        public async Task<ActionResult> AsignarRolaUsuario(EditarRolDTO editarRolDTO, Guid storeId)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UserId);
            if (usuario == null)
            {
                return NotFound();
            }

            var roles = await userManager.GetRolesAsync(usuario);
            //if (roles.Count == 0)
            //{
            //    return Ok();
            //}

            var deleteRoles = await userManager.RemoveFromRolesAsync(usuario, roles);
            if (!deleteRoles.Succeeded)
            {
                return BadRequest(deleteRoles.Errors);
            }

            await userManager.AddToRoleAsync(usuario, editarRolDTO.RoleId);

            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;

            var usuarioId = email;
            var ipUser = obtenetIP();
            var name = editarRolDTO.RoleId.ToString();
            var storeId2 = storeId;
            await servicioBinnacle.AsignarRol(usuarioId, ipUser, name, storeId2);

            return NoContent();
        }


        [HttpPost("RemoverRol")]
        public async Task<ActionResult> RemoverRolaUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UserId);
            await userManager.RemoveFromRoleAsync(usuario, editarRolDTO.RoleId);
            return NoContent();
        }

        [HttpGet("{id}", Name = "obteneruser")]
        ////[AllowAnonymous]
        public async Task<ActionResult<CredencialesUsuariosDTO>> Get(string id)
        {
            //var Store = await context.Stores.FirstOrDefaultAsync(x => x.StoreIdx == id);
            var user = await userManager.Users.FirstOrDefaultAsync(claim => claim.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        //[HttpGet("{user}")]
        ////[AllowAnonymous]
        //public async Task<ActionResult<CredencialesUsuariosDTO>> Get(string use)
        //{
        //    //var Store = await context.Stores.FirstOrDefaultAsync(x => x.StoreIdx == id);
        //    var user = await userManager.Users.FirstOrDefaultAsync(claim => claim.Id == ema);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}


        //[HttpPost("registrar")] //   api/cuentas/registrar
        //public async Task<ActionResult<RespuestaAutenticacionDTO>> Registrar(CredencialesUsuariosDTO credencialesUsuariosDTO)
        //{
        //    var usuario = new IdentityUser { UserName = credencialesUsuariosDTO.Email,
        //        Email = credencialesUsuariosDTO.Email };

        //    var resultado = await userManager.CreateAsync(usuario, credencialesUsuariosDTO.Password); //Respuesta Identity

        //    //var ChPerson = mapper.Map<ChPerson>(ChPersonDTO); //respuesta contextbd

        //    if (resultado.Succeeded)
        //    {
        //        return ConstruirToken(credencialesUsuariosDTO);

        //    }
        //    else
        //    {
        //        return BadRequest(resultado.Errors);
        //    }
        //}


        [HttpPost("registrar/{storeId?}")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Registrar(CredencialesUsuariosDTO credencialesUsuariosDTO, Guid storeId)
        {
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {
                    var aspNetUserDb = await context.Users.AnyAsync(x => x.Email == credencialesUsuariosDTO.Email);
                    if (aspNetUserDb) { return BadRequest("Usuario ya existe"); }

                    var usuario = new IdentityUser
                    {
                        UserName = credencialesUsuariosDTO.Email,
                        Email = credencialesUsuariosDTO.Email,
                    };

                    var resultado = await userManager.CreateAsync(usuario, credencialesUsuariosDTO.Password); //Respuesta Identity
                    var emailconfimed = credencialesUsuariosDTO.EmailConfirmed;

                    var storedb = await context.Stores.FirstOrDefaultAsync(x => x.StoreId == storeId);
                    var companyId = storedb.CompanyId;

                    if (companyId != Guid.Empty)
                    {
                        UserStore us = new()
                        {
                            UserId = usuario.Id,
                            CompanyId = companyId,
                            StoreId = storeId,
                            Date = DateTime.Now,
                            Updated = DateTime.Now,
                            Active = true,
                            Locked = false,
                            Deleted = false
                        };
                        context.UserStores.Add(us);

                        var usuarioId = obtenerUsuarioId();
                        var ipUser = obtenetIP();
                        var name2 = credencialesUsuariosDTO.Email;
                        var storeId2 = storeId;
                        await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name2, storeId2);

                        context.SaveChanges();
                        await transaccion.CommitAsync();

                        if (resultado.Succeeded)
                        {

                            return ConstruirToken(credencialesUsuariosDTO, new List<string>(), usuario.Id, emailconfimed, storedb.CompanyId);
                            //return Ok(resultado);
                        }
                        return Ok(resultado);

                    }
                    else
                    {
                        return BadRequest(resultado.Errors /*+ "Seleccionar Empresa"*/);
                    }
                    //return BadRequest("Revisar que empresa y sucursal existan");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message /*+ "Revisar que empresa y sucursal existan"*/);
            }
        }

        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Registrar(CredencialesUsuariosDTO credencialesUsuariosDTO)
        {
            try
            {
                var usuario = new IdentityUser
                {
                    UserName = credencialesUsuariosDTO.Email,
                    Email = credencialesUsuariosDTO.Email,
                };

                var resultado = await userManager.CreateAsync(usuario, credencialesUsuariosDTO.Password); //Respuesta Identity
                var emailconfimed = credencialesUsuariosDTO.EmailConfirmed;

                if (resultado.Succeeded)
                {

                    return ConstruirToken2(credencialesUsuariosDTO, new List<string>(), usuario.Id, emailconfimed, usuario.Id);
                    //return Ok(resultado);
                }
                else
                {
                    return BadRequest(resultado.Errors + "Seleccionar Empresa");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "Revisar que empresa y sucursal existan");
            }
        }


        [HttpPost("regUserPromo")]
        //[AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Registrar2(CredencialesUsuariosDTO credencialesUsuariosDTO, int netgroupIdi, string name)
        {
            var netGroupIdiDB = await context.Netgroups.FirstOrDefaultAsync(x => x.NetgroupIdi == netgroupIdi);
            var emailconfimed = credencialesUsuariosDTO.EmailConfirmed;
            if (netGroupIdiDB == null)
            {
                return Content("no existe NetGroup");
            }

            var netGroupUserMap = new Dictionary<string, string>();

            var usuario = new IdentityUser
            {
                UserName = credencialesUsuariosDTO.Email,
                Email = credencialesUsuariosDTO.Email
            };

            var resultado = await userManager.CreateAsync(usuario, credencialesUsuariosDTO.Password); //Respuesta Identity

            //var usuarioId = obtenerUsuarioId();
            //var ipUser = obtenetIP();
            //var name = credencialesUsuariosDTO.Email;
            //await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name);

            //var ChPerson = mapper.Map<ChPerson>(ChPersonDTO); //respuesta contextbd

            if (resultado.Succeeded)
            {
                var usuario2 = usuario;
                var roleid2 = "Promociones";
                await userManager.AddToRoleAsync(usuario2, roleid2);

                var netGroupUser = new NetgroupUser()
                {
                    NetgroupId = netGroupIdiDB.NetgroupId,
                    UserId = usuario2.Id,
                    NetgroupUserId = Guid.NewGuid(),
                    Name = name,
                    Description = "userRolProciones",
                    MakeInvoice = false,
                    Date = DateTime.Now,
                    Updated = DateTime.Now,
                    Active = true,
                    Locked = false,
                    Deleted = false
                };
                context.Add(netGroupUser);
                await context.SaveChangesAsync();

                var dbUserStore = await context.UserStores.FirstOrDefaultAsync(x => x.UserId == usuario.Id);
                var dbComany = dbUserStore.CompanyId;

                return ConstruirToken(credencialesUsuariosDTO, new List<string>(), usuario.Id, emailconfimed, dbComany);

                //return NoContent();

                //return Ok(resultado);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Login(CredencialesUsuariosDTO credencialesUsuariosDTO)
        {
            var usuarioIni = await userManager.FindByEmailAsync(credencialesUsuariosDTO.Email);
            if (usuarioIni == null) return BadRequest("Usuario no existe");
            //else if (usuarioIni.EmailConfirmed is false) return BadRequest("Restablece tu contraseña");

            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuariosDTO.Email, credencialesUsuariosDTO.Password,
            isPersistent: false, lockoutOnFailure: false);

            var usuarioId = credencialesUsuariosDTO.Email;
            var ipUser = obtenetIP();
            var name = credencialesUsuariosDTO.Email;

            if (resultado.Succeeded)
            {
                var usuario = await userManager.FindByEmailAsync(credencialesUsuariosDTO.Email);
                var roles = await userManager.GetRolesAsync(usuario);
                var emailConfirmed = usuario.EmailConfirmed;

                if (credencialesUsuariosDTO.Email != "masterSupport@controlnet.com.mx" && roles.FirstOrDefault() != "RolByNetGroup")
                {
                    var dbUserStore = await context.UserStores.FirstOrDefaultAsync(x => x.UserId == usuarioIni.Id);
                    var dbComany = dbUserStore!.CompanyId;
                    await servicioBinnacle.loginBinnacle(usuarioId, ipUser, name);

                    return ConstruirToken(credencialesUsuariosDTO, roles, usuario.Id, emailConfirmed, dbComany);
                }
                else if (roles.FirstOrDefault() == "RolByNetGroup")
                {
                    await servicioBinnacle.loginBinnacle(usuarioId, ipUser, name);
                    return ConstruirToken2(credencialesUsuariosDTO, roles, usuario.Id, emailConfirmed, usuario.Id);
                }
                else
                {
                    await servicioBinnacle.loginBinnacle(usuarioId, ipUser, name);
                    return ConstruirToken2(credencialesUsuariosDTO, roles, usuario.Id, emailConfirmed, usuario.Id);
                }
            }
            else
            {
                await servicioBinnacle.errorLoginBinnacle(usuarioId, ipUser, name);
                return BadRequest("Login incorrecto");
            }
        }


        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> LoginOut(Guid storeId, CredencialesUsuariosDTO credencialesUsuariosDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;

            var usuario2 = email;
            var ipUser = obtenetIP();
            var name = credencialesUsuariosDTO.Email;
            var storeId2 = storeId;
            await servicioBinnacle.logoutBinnacle(usuario2, ipUser, name, storeId2);
            return NoContent();
        }

        [HttpPost("logout2")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> LoginOut2(Guid storeId, CredencialesUsuariosDTO credencialesUsuariosDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;

            var usuario2 = email;
            var ipUser = obtenetIP();
            var name = credencialesUsuariosDTO.Email;
            var storeId2 = storeId;
            await servicioBinnacle.logoutBinnacle2(usuario2, ipUser, name, storeId2);
            return NoContent();
        }


        //[HttpGet("RenovarToken")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<ActionResult<RespuestaAutenticacionDTO>> RenovarAsync()
        //{
        //    var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
        //    var email = emailClaim.Value;
        //    var credencialesUsuario = new CredencialesUsuariosDTO()

        //    {
        //        Email = email,
        //    };

        //    return ConstruirToken(credencialesUsuario);
        //}


        [HttpGet("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;

            var idClaim = HttpContext.User.Claims.Where(claim => claim.Type == "id").FirstOrDefault();
            var usuarioId = emailClaim.Value;
            var emailConfirmed = usuarioId.ToString() == emailClaim.Value;

            var credencialesUsuariosDTO = new CredencialesUsuariosDTO()
            {
                Email = email
            };

            var usuario = await userManager.FindByEmailAsync(credencialesUsuariosDTO.Email);
            var roles = await userManager.GetRolesAsync(usuario);

            var dbUserStore = await context.UserStores.FirstOrDefaultAsync(x => x.UserId == usuario.Id);
            var dbComany = dbUserStore.CompanyId;

            return ConstruirToken(credencialesUsuariosDTO, roles, usuarioId, emailConfirmed, dbComany);
        }



        //[HttpPost("ResetPassword")]
        //public async Task<ActionResult> ResetPassword(CredencialesUsuariosDTO credencialesUsuariosDTO)
        //{

        //    var user = await userManager.FindByEmailAsync(credencialesUsuariosDTO.Email);
        //    var tokenTemp = await userManager.GeneratePasswordResetTokenAsync(user);
        //    var resultado = await userManager.ResetPasswordAsync(user, tokenTemp, credencialesUsuariosDTO.Password);
        //    if (resultado.Succeeded)
        //    {
        //        return Ok(resultado);
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }

        //}

        [HttpPut("ResetPassword/{storeId?}")]
        ////[AllowAnonymous]
        public async Task<ActionResult> ResetPassword(CredencialesUsuariosDTO credencialesUsuariosDTO, Guid storeId)
        {
            var user = await userManager.FindByEmailAsync(credencialesUsuariosDTO.Email);
            var emailConfirm = user.EmailConfirmed = true;
            var tokenTemp = await userManager.GeneratePasswordResetTokenAsync(user);
            var resultado = await userManager.ResetPasswordAsync(user, tokenTemp,
                credencialesUsuariosDTO.Password);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = credencialesUsuariosDTO.Email;
            var storeId2 = storeId;
            await servicioBinnacle.changePasswordBinnacle(usuarioId, ipUser, name, storeId2);

            if (resultado.Succeeded)
            {
                return Ok(resultado);
            }
            else
            {
                return BadRequest();
            }
        }


        //[HttpDelete("{id}/{storeId?}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id, Guid? storeId)
        {
            //var existe = await context.Stores.AnyAsync(x => x.StoreIdx == id);
            var user = await userManager.Users.FirstOrDefaultAsync(claim => claim.Id == id);
            //if (!existe)
            //{
            //    return NotFound();
            //}
            var db = await context.UserStores.FirstOrDefaultAsync(x => x.UserId == id);
            if (db == null)
            {
            }
            else
            {
                context.Remove(db);
            }


            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = user.Email;
            var storeId2 = storeId;
            await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2);

            var resultado = await userManager.DeleteAsync(user);


            //context.Remove(new Store { StoreIdx = id });
            //await context.SaveChangesAsync();
            return NoContent();
        }


        //private RespuestaAutenticacionDTO ConstruirToken(CredencialesUsuariosDTO credencialesUsuariosDTO)
        //{
        //    var claims = new List<Claim>()
        //    {
        //        new Claim("email", credencialesUsuariosDTO.Email),
        //        new Claim("lo que yo quiera", "cualquier otro valor"),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };


        //    var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
        //    var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

        //    var expiracion = DateTime.UtcNow.AddMinutes(59);

        //    var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
        //        expires: expiracion, signingCredentials: creds);

        //    return new RespuestaAutenticacionDTO()
        //    {
        //        Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
        //        Expiracion = expiracion
        //    };
        //}

        private RespuestaAutenticacionDTO ConstruirToken(CredencialesUsuariosDTO credencialesUsuariosDTO,
            IList<string> roles, string usuarioId, bool emailConfirmed, Guid? dbComany)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuariosDTO.Email),
                new Claim(ClaimTypes.Name, credencialesUsuariosDTO.Email),
                new Claim("emailConfirmed", credencialesUsuariosDTO.EmailConfirmed.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", usuarioId)
            };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.Now.AddHours(8);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            //var dbUsrStore = context.UserStores.Where(x => x.UserId == credencialesUsuariosDTO.Id).FirstOrDefaultAsync();

            return new RespuestaAutenticacionDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion,
                EmailConfirmed = emailConfirmed,
                dbComany = (Guid)dbComany,
                Roles = roles.ToList()
            };
        }

        private RespuestaAutenticacionDTO ConstruirToken2(CredencialesUsuariosDTO credencialesUsuariosDTO,
            IList<string> roles, string usuarioId, bool emailConfirmed, string? userId)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuariosDTO.Email),
                new Claim(ClaimTypes.Name, credencialesUsuariosDTO.Email),
                new Claim("emailConfirmed", credencialesUsuariosDTO.EmailConfirmed.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", usuarioId)
            };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.Now.AddHours(8);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            //var dbUsrStore = context.UserStores.Where(x => x.UserId == credencialesUsuariosDTO.Id).FirstOrDefaultAsync();

            return new RespuestaAutenticacionDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion,
                EmailConfirmed = emailConfirmed,
                userId = userId,
                Roles = roles.ToList()
            };
        }

    }
}
