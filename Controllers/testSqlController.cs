using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection.Metadata;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class testSqlController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;
        private readonly IConfiguration _configuration;

        public testSqlController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<TestsqlDTO>> Get()
        {
            var result = await context.Testsqls
                .ToListAsync();

            return mapper.Map<IEnumerable<TestsqlDTO>>(result);
        }

        //public const string gString = "71930422-3B22-4ABA-8D49-5BC623067637";
        //string STORE_ID = gString.ToString();

        [HttpGet("{id:int}", Name = "obtenerConsulta")]
        public async Task<ActionResult<TestsqlDTO>> GetQuery(int id, string STORE_ID)
        {
            var testsql = await context.Testsqls.FirstOrDefaultAsync(x => x.Id == id);

            if (testsql == null)
            {
                return NotFound();
            }
            //return mapper.Map<TestsqlDTO>(result);

            var consulta = testsql.Quey;

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
        public async Task<ActionResult<TestsqlDTO>> GetQueryReport(int id, string storeId, string startDate, string endDate)
        {
            var testsql = await context.Testsqls.FirstOrDefaultAsync(x => x.Id == id);

            if (testsql == null)
            {
                return NotFound();
            }
            //return mapper.Map<TestsqlDTO>(result);

            var consulta = testsql.Quey;

            //// Ejecutar consulta usando FromSqlRaw (solo si mapea a una entidad conocida)
            //var resultado = await context.Stores.FromSqlRaw(consulta).ToListAsync();

            //return Ok(resultado);

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@storeId", storeId);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
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

    }
}
