using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SqlReportController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;
        private readonly IConfiguration _configuration;

        public SqlReportController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
            _configuration = configuration;
        }

        [HttpGet("noPage")]
        [AllowAnonymous]
        public async Task<IEnumerable<SqlReportDTO>> Get()
        {
            var result = await context.SqlReports
                .ToListAsync();

            return mapper.Map<IEnumerable<SqlReportDTO>>(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SqlReportDTO>>> Get(int skip, int take, string searchTerm = "")
        {
            var query = context.SqlReports.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.ToLower().Contains(searchTerm) || c.Query.ToLower().Contains(searchTerm));
            }

            var data = await query.ToListAsync();
            var ntotal = query.Count();
            return Ok(new { data, ntotal });
        }


        [HttpGet("{id:int}", Name = "obtenerConsulta")]
        public async Task<ActionResult<SqlReportDTO>> GetQuery(int id, string STORE_ID)
        {
            var testsql = await context.SqlReports.FirstOrDefaultAsync(x => x.Id == id);

            if (testsql == null)
            {
                return NotFound();
            }
            //return mapper.Map<TestsqlDTO>(result);

            var consulta = testsql.Query;

            //// Ejecutar consulta usando FromSqlRaw (solo si mapea a una entidad conocida)
            //var resultado = await context.Stores.FromSqlRaw(consulta).ToListAsync();

            //return Ok(resultado);

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@parametro", STORE_ID);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        var result = DataTableToList(dataTable);
                        return Ok(result);
                    }
                }
            }
        }

        private List<Dictionary<string, object>> DataTableToList(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in table.Columns)
                {
                    dict[column.ColumnName] = row[column];
                }
                list.Add(dict);
            }
            return list;
        }


        [HttpGet("report/{id:int}", Name = "obtenerConsulta_tablaReport")]
        [AllowAnonymous]
        public async Task<ActionResult<SqlReportDTO>> GetQueryReport(int id, string parameter1, string parameter2, string parameter3)
        //public async Task<ActionResult<SqlReportDTO>> GetQueryReport(int id)
        {
            var testsql = await context.SqlReports.FirstOrDefaultAsync(x => x.Id == id);

            if (testsql == null)
            {
                return NotFound();
            }
            //return mapper.Map<TestsqlDTO>(result);

            var consulta = testsql.Query;

            //// Ejecutar consulta usando FromSqlRaw (solo si mapea a una entidad conocida)
            //var resultado = await context.Stores.FromSqlRaw(consulta).ToListAsync();

            //return Ok(resultado);

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@parameter1", parameter1);
                    command.Parameters.AddWithValue("@parameter2", parameter2);
                    command.Parameters.AddWithValue("@parameter3", parameter3);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        var result = DataTableToList2(dataTable);
                        return Ok(result);
                    }
                }
            }
        }
        private List<Dictionary<string, object>> DataTableToList2(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in table.Columns)
                {
                    dict[column.ColumnName] = row[column];
                }
                list.Add(dict);
            }
            return list;
        }



        [HttpGet("byId/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<SqlReportDTO>> Get(int id)
        {
            var report = await context.SqlReports.FirstOrDefaultAsync(x => x.Id == id);

            if (report == null)
            {
                return NotFound();
            }
            return mapper.Map<SqlReportDTO>(report);
        }


        [HttpPut("{storeId?}")]
        public async Task<ActionResult> Put(SqlReportDTO sqlReportDTO, Guid storeId)
        {
            var sqlreportDb = await context.SqlReports.FirstOrDefaultAsync(r => r.Id == sqlReportDTO.Id);

            if (sqlreportDb is null)
            {
                return NotFound();
            }
            try
            {
                sqlreportDb = mapper.Map(sqlReportDTO, sqlreportDb);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = sqlreportDb.Name;

                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest($"Ya existe isla {sqlReportDTO.Name} en esa sucursal ");
            }
        }


        [HttpPost("{storeId?}")]
        public async Task<ActionResult> Post(SqlReportDTO sqlReportDTO, Guid storeId)
        {
            var existe = await context.SqlReports.AnyAsync(x => x.Id == sqlReportDTO.Id );

            var sqlreport = mapper.Map<SqlReport>(sqlReportDTO);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = sqlReportDTO.Name;
            var storeId2 = storeId;

            if (existe)
            {
                return BadRequest($"Ya existe {sqlReportDTO.Name} ");
            }
            else
            {
                context.Add(sqlreport);
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
            }
            return Ok();
        }
    }
}
