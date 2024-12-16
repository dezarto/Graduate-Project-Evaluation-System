using GPESAPI.Application.Interfaces;
using GPESAPI.Application.Services;
using GPESAPI.Domain.Interfaces;
using GPESAPI.Domain.Services;
using GPESAPI.Infrastructure.Repositories;
using GPESAPI.API.Mapping;
using GPESAPI.Infrastructure.Persistence;
using GPESAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.Features;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// 1. **Controllers & CORS**
builder.Services.AddControllers();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

// File upload settings
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 209715200; // 200 MB
});

// 2. **Database Setup**
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SqlDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. **AutoMapper Configuration**
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 4. **JWT Authentication Setup**
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = jwtSettings["Key"];

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

// 5. **Dependency Injection - Scoped Services**
// User services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAppService, UserAppService>();

// Generic repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Professor services
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<IProfessorService, ProfessorService>();
builder.Services.AddScoped<IProfessorAppService, ProfessorAppService>();

// Professors-Users relationship services
builder.Services.AddScoped<IProfessorsUsersRepository, ProfessorsUsersRepository>();
builder.Services.AddScoped<IProfessorsUsersService, ProfessorsUsersService>();
builder.Services.AddScoped<IProfessorsUsersAppService, ProfessorsUsersAppService>();

// ProfessorAvailability services
builder.Services.AddScoped<IProfessorAvailabilityRepository, ProfessorAvailabilityRepository>();
builder.Services.AddScoped<IProfessorAvailabilityService, ProfessorAvailabilityService>();
builder.Services.AddScoped<IProfessorAvailabilityAppService, ProfessorAvailabilityAppService>();

// Team services
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamAppService, TeamAppService>();

// TeamPresentation services
builder.Services.AddScoped<ITeamPresentationAppService, TeamPresentationAppService>();
builder.Services.AddScoped<ITeamPresentationService, TeamPresentationService>();
builder.Services.AddScoped<ITeamPresentationRepository, TeamPresentationRepository>();

// Project services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectAppService, ProjectAppService>();

// TeamMember services
builder.Services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();
builder.Services.AddScoped<ITeamMemberAppService, TeamMemberAppService>();

// Checklist services
builder.Services.AddScoped<IChecklistItemDetailService, ChecklistItemDetailService>();
builder.Services.AddScoped<IChecklistItemsService, ChecklistItemsService>();
builder.Services.AddScoped<IChecklistItemsRepository, ChecklistItemsRepository>();
builder.Services.AddScoped<IChecklistItemsAppService, ChecklistItemsAppService>();

// Evaluation criteria services
builder.Services.AddScoped<IEvaluationCriteriaDetailService, EvaluationCriteriaDetailService>();
builder.Services.AddScoped<IEvaluationCriteriaService, EvaluationCriteriaService>();
builder.Services.AddScoped<IEvaluationCriteriaRepository, EvaluationCriteriaRepository>();

// Evaluation services
builder.Services.AddScoped<IEvaluationService, EvaluationService>();
builder.Services.AddScoped<IEvaluationAppService, EvaluationAppService>();
builder.Services.AddScoped<IEvaluationCriteriaAppService, EvaluationCriteriaAppService>();

// Report services
builder.Services.AddScoped<IReportAppService, ReportAppService>();
builder.Services.AddScoped<IReportService, ReportService>();

// 6. **Swagger / OpenAPI Configuration**
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Bearer token ile giriş yapın",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// **Middleware Configuration**
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Configure endpoints
app.MapControllers();

app.Run();
