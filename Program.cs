using APIControlNet.DTOs.Seeding;
using APIControlNet.Models;
using APIControlNet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; //para cors

// Add services to the container.
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); 


builder.Services.AddControllers();

/////Agrege conexion de appjson, nueva manera en net 6
builder.Services.AddDbContext<CnetCoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

/////Modifique para ignorar referencias de cliclos, cuando traes data relacionada y NewtonsoftJson
builder.Services.AddControllers().AddJsonOptions(x =>
x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

///Automapper//servicio de automapper
builder.Services.AddAutoMapper(typeof(CnetCoreContext));

//jwtbearrer
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["llavejwt"])),
        ClockSkew = TimeSpan.Zero
    });


//identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<CnetCoreContext>()
    .AddDefaultTokenProviders();

//Repositorio Generico
//builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));  

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
        .WithExposedHeaders(new string[] { "totalPaginas", "conteo" });
    });
});

builder.Services.AddScoped<ServicioBinnacle>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//probar token con swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                                    {
                                        Name = "Authorization",
                                        Type = SecuritySchemeType.ApiKey,
                                        Scheme = "Bearer",
                                        BearerFormat = "JWT",
                                        In = ParameterLocation.Header
                                    });

c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
           new OpenApiSecurityScheme
                {
                 Reference = new OpenApiReference
                 {
                   Type = ReferenceType.SecurityScheme,
                   Id = "Bearer"
                 }
           },
           new string[]{}
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Para swagger en production
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();  //quite para usr http

//app.UseCors(MyAllowSpecificOrigins);
app.UseCors();

//app.UseAuthentication();
app.UseAuthorization();

//app.UseCors(MyAllowSpecificOrigins);                          //agregre corrs

app.MapControllers();

//var supportedCultures = new[]
//{
//    //new CultureInfo("en-US"),
//    new CultureInfo("es-MX"),
//};

//app.UseRequestLocalization(new RequestLocalizationOptions
//{
//    DefaultRequestCulture = new RequestCulture("en-US"),
//    SupportedCultures = supportedCultures,
//    SupportedUICultures = supportedCultures
//});

app.Run();


