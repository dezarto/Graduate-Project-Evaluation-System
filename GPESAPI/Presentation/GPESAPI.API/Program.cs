using GPESAPI.Application.Interfaces;
using GPESAPI.Application.Services;
using GPESAPI.Domain.Interfaces;
using GPESAPI.Domain.Services;
using GPESAPI.Infrastructure.Repositories;
using GraduateProjectEvaluationSystemAPI.API.Mapping;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Application.Services;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Services;
using GraduateProjectEvaluationSystemAPI.Infrastructure.Persistence;
using GraduateProjectEvaluationSystemAPI.Infrastructure.Repositories;
using GraduateProjectEvaluationSystemAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// CORS ayarları
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()    // Tüm kaynaklara izin ver
            .AllowAnyMethod()    // Tüm HTTP yöntemlerine izin ver
            .AllowAnyHeader());  // Tüm başlıklara izin ver
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SqlDbContext>(options =>
    options.UseSqlServer(connectionString));

// AutoMapper konfigürasyonu
builder.Services.AddAutoMapper(typeof(MappingProfile));

// JWT token servisi
builder.Services.AddScoped<ITokenService, TokenService>();

// Kullanıcı servisleri
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAppService, UserAppService>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Profesör servisleri
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<IProfessorService, ProfessorService>();
builder.Services.AddScoped<IProfessorAppService, ProfessorAppService>();

// Profesör-Kullanıcı ilişkisi servisleri
builder.Services.AddScoped<IProfessorsUsersRepository, ProfessorsUsersRepository>();
builder.Services.AddScoped<IProfessorsUsersService, ProfessorsUsersService>();
builder.Services.AddScoped<IProfessorsUsersAppService, ProfessorsUsersAppService>();

// ProfessorAvailability
builder.Services.AddScoped<IProfessorAvailabilityRepository, ProfessorAvailabilityRepository>();
builder.Services.AddScoped<IProfessorAvailabilityService, ProfessorAvailabilityService>();
builder.Services.AddScoped<IProfessorAvailabilityAppService, ProfessorAvailabilityAppService>();

// Team servisleri
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamAppService, TeamAppService>();

// TeamPresentation servisleri
builder.Services.AddScoped<ITeamPresentationAppService, TeamPresentationAppService>();
builder.Services.AddScoped<ITeamPresentationService, TeamPresentationService>();
builder.Services.AddScoped<ITeamPresentationRepository, TeamPresentationRepository>();


// Project servisleri
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectAppService, ProjectAppService>();

// TeamMember servisleri
builder.Services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();
builder.Services.AddScoped<ITeamMemberAppService, TeamMemberAppService>();




// **JWT Authentication configuration**
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = jwtSettings["Key"]; // appsettings.json'dan JWT anahtarını al

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Swagger/OpenAPI yapılandırması
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// **CORS Middleware**
app.UseCors("AllowAllOrigins");

// **JWT Authentication Middleware**
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
