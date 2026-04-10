using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using ProfessionalsSiancaValley.Api.Data;
using ProfessionalsSiancaValley.Api.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================= DATABASE =================
//builder.Services.AddSingleton<NpgsqlConnection>(sp =>
//{
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//return new NpgsqlConnection(connectionString);
//});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ================= CONTROLLERS =================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Ingrese el token JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});

// ================= JWT =================
var jwt = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)),

            RoleClaimType = ClaimTypes.Role // 👈 CLAVE PARA ROLES
        };
    });

builder.Services.AddScoped<PublicationService>();

builder.Services.AddAuthorization();

builder.Services.AddControllers();

// ================= AUTHORIZATION =================
builder.Services.AddAuthorization(options =>
{
    // ================= CLAIMS =================
    options.AddPolicy("MayorDeEdad", policy =>
        policy.RequireClaim("estadoEdad", "True"));

    // ================= ROLES BASE =================
    options.AddPolicy("SoloAdmin", policy =>
        policy.RequireRole("Admin"));

    // ================= POLÍTICAS FUNCIONALES =================
    options.AddPolicy("PuedeModerarMiniaturas", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("PuedePublicarMiniaturas", policy =>
        policy.RequireRole("Admin", "Editor"));

    options.AddPolicy("AccesoProfesionales", policy =>
        policy.RequireRole("Admin", "Profesional"));
});

// ================= AGREGAR CORS =================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

// ================= PIPELINE =================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();   // <-- IMPORTANTE
app.UseAuthorization();

app.MapControllers();

app.Run();

