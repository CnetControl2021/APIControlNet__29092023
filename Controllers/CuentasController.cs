using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Xml;
using System.Xml.Linq;


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
        ////[AllowAnonymous]
        //public IActionResult ListUsers()
        //{
        //    var users = userManager.Users;
        //    return Ok(users);
        //}


        [HttpGet("listadoUsuarios")]
        public async Task<ActionResult<List<CredencialesUsuariosDTO>>> ListadoUsuarios([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = userManager.Users.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            //var usuarios = await queryable.Paginar(paginacionDTO).ToListAsync();
            var usuarios = await queryable.Paginar(paginacionDTO).Select(x => new CredencialesUsuariosDTO { Email = x.Email, Id = x.Id })
                .OrderBy(x => x.Email)
                .ToListAsync();
            return Ok(usuarios);
        }


        [HttpPost("AsignarRol/{storeId?}")] ///se guarda relacion en AspNetUserRoles
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


        [HttpPost("registrar/{CompanyId?}")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Registrar(CredencialesUsuariosDTO credencialesUsuariosDTO, Guid CompanyId)
        {
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {
                    var usuario = new IdentityUser
                    {
                        UserName = credencialesUsuariosDTO.Email,
                        Email = credencialesUsuariosDTO.Email,
                    };

                    var resultado = await userManager.CreateAsync(usuario, credencialesUsuariosDTO.Password); //Respuesta Identity
                    var emailconfimed = credencialesUsuariosDTO.EmailConfirmed;

                    if (CompanyId != Guid.Empty)
                    {
                        var dbCompany = await context.Stores.FirstOrDefaultAsync(x => x.CompanyId == CompanyId);
                        var StoreId = dbCompany.StoreId;

                        UserStore us = new()
                        {
                            UserId = usuario.Id,
                            CompanyId = CompanyId,
                            StoreId = StoreId,
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
                        var storeId2 = StoreId;
                        await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name2, storeId2);

                        context.SaveChanges();
                        await transaccion.CommitAsync();

                        if (resultado.Succeeded)
                        {

                            return ConstruirToken(credencialesUsuariosDTO, new List<string>(), usuario.Id, emailconfimed, dbCompany.CompanyId);
                            //return Ok(resultado);
                        }

                    }
                    else
                    {
                        return BadRequest(resultado.Errors + "Seleccionar Empresa");
                    }
                    return BadRequest("Revisar que empresa y sucursal existan");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "Revisar que empresa y sucursal existan");
            }
        }

        [HttpPost("registrar")]
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

                    return ConstruirToken2(credencialesUsuariosDTO, new List<string>(), usuario.Id, emailconfimed);
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

                if (credencialesUsuariosDTO.Email != "masterSupport@controlnet.com.mx")
                {
                    var dbUserStore = await context.UserStores.FirstOrDefaultAsync(x => x.UserId == usuarioIni.Id);
                    var dbComany = dbUserStore.CompanyId;
                    await servicioBinnacle.loginBinnacle(usuarioId, ipUser, name);

                    return ConstruirToken(credencialesUsuariosDTO, roles, usuario.Id, emailConfirmed, dbComany);
                }
                else
                {
                    return ConstruirToken2(credencialesUsuariosDTO, roles, usuario.Id, emailConfirmed);
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


        [HttpDelete("{id}/{storeId?}")]
        public async Task<IActionResult> Delete(string id, Guid storeId)
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
                dbComany = (Guid)dbComany
            };
        }

        private RespuestaAutenticacionDTO ConstruirToken2(CredencialesUsuariosDTO credencialesUsuariosDTO,
            IList<string> roles, string usuarioId, bool emailConfirmed)
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
                EmailConfirmed = emailConfirmed
            };
        }
    }
}
